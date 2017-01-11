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
            if (args.Any())
            {
                Console.WriteLine(args[0]);
                Console.ReadLine();
            }
        


            PlaneModel plane = null;
            try
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(args[0]))
                {

                    Mutex mutex = Mutex.OpenExisting("testmapmutex");
                    mutex.WaitOne();
                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        var result = reader.ReadString();
                        plane = JsonConvert.DeserializeObject<PlaneModel>(result);
                    }

                     
                    mutex.ReleaseMutex();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Memory-mapped file does not exist. Run Process A first, then B.");
            }

            if (plane != null)
            {
                Console.WriteLine("Plane number: " + plane.Id);
                foreach (var point in plane.Path)
                {
                    plane.CurrentLocation = point;
                    Thread.Sleep(1000);
                    PrintPlaneInfo(point);
                }
            }
           

            //var t = 0;
            //while (t < 10)
            //{
            //    Thread.Sleep(1000);
            //    Console.WriteLine("Test succeeded!");
            //    t++;
            //}
            Console.ReadLine();

        }

        private static void PrintPlaneInfo(PathPoint point)
        {
            Console.WriteLine("Current position: lat - " + point.lat +
                              ", lng - " + point.lng + ", alt - " + point.alt);
        }
    }
}
