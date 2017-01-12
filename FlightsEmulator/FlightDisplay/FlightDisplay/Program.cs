using System;
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
        static void Main(string[] args)
        {
            PlaneModel plane = null;
            if (args.Any())
            {
                
                Console.WriteLine(args[0]);
                var dto = JsonConvert.DeserializeObject<InterprocessDTO>(args[0]);

                if (!string.IsNullOrEmpty(dto.MutexName) && !string.IsNullOrEmpty(dto.MemoryMappedFileName))
                {
                    try
                    {
                        using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(dto.MemoryMappedFileName))
                        {

                            Mutex mutex = Mutex.OpenExisting(dto.MutexName);
                            mutex.WaitOne();
                            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                            {
                                BinaryReader reader = new BinaryReader(stream);
                                var result = reader.ReadString();
                                plane = JsonConvert.DeserializeObject<PlaneModel>(result);
                            }
                            
                            mutex.ReleaseMutex();


                            if (plane != null)
                            {
                                Console.WriteLine("Plane number: " + plane.Id);
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
                                    PrintPlaneInfo(point);
                                }
                            }


                        }
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("Memory-mapped file does not exist. Run Process A first, then B.");
                    }
                }
            }
           

         
            Console.ReadLine();

        }

        private static void PrintPlaneInfo(PathPoint point)
        {
            Console.WriteLine("Current position: lat - " + point.lat +
                              ", lng - " + point.lng + ", alt - " + point.alt);
        }
    }
}
