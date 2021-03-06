﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FlightsEmulator;

namespace FlightDisplay
{
    class Program
    {

        #region Constants

        private const int FILE_NAME_ARG_NUMBER = 0;
        private const int MUTEX_ARG_NUMBER = 1;


        #endregion

        static void Main(string[] args)
        {
            if (args.Any())
            {
                var memoryMappedFileName = args[FILE_NAME_ARG_NUMBER];
                var mutexName = args[MUTEX_ARG_NUMBER];

                if (!string.IsNullOrEmpty(mutexName) && !string.IsNullOrEmpty(memoryMappedFileName))
                {
                    try
                    {
                        using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(memoryMappedFileName))
                        {

                            Mutex mutex = Mutex.OpenExisting(mutexName);
                            mutex.WaitOne();
                            PlaneModel plane = null;
                            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                            {
                                BinaryReader reader = new BinaryReader(stream);
                                var result = reader.ReadString();
                                plane = JsonConvert.DeserializeObject<PlaneModel>(result);
                            }

                            mutex.ReleaseMutex();


                            if (plane != null)
                            {
                                Console.WriteLine(Resources.PlaneNumberMsg, plane.Id);
                                foreach (var point in plane.Path)
                                {
                                    plane.CurrentLocation = point;

                                    mutex.WaitOne();
                                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                                    {
                                        BinaryWriter writer = new BinaryWriter(stream);
                                        writer.Write(plane.CurrentLocation.JsonOut());
                                    }
                                    mutex.ReleaseMutex();

                                    Thread.Sleep(1000);
                                    Console.WriteLine(Resources.CurrentPositionMsg, point.lat, point.lng, point.alt);
                                }
                            }


                        }
                        Environment.Exit(0);
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine(Resources.FileNotExistMsg);
                    }
                }
            }

        }

    }
}
