using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightsEmulator
{

    class Program
    {
        private readonly static string _display= ConfigurationManager.AppSettings["FlightDisplayApp"];

        static void Main(string[] args)
        {
            IPlanesService planesService = new PlanesService();
            var plane = planesService.GetPlaneById(3);
            var t = plane.JsonOut();


           


            //var t2 = JsonConvert.DeserializeObject<PlaneModel>(t);
            //var plane5 = planesService.GetPlaneById(5);
            //var location = planesService.GetCurrentLocation(plane);
            ////Console.WriteLine("lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);

            //planesService.StartFlight(plane).ContinueWith((result) =>
            //{
            //    Console.WriteLine("Plane finished");
            //});
            //bool t = true;
            //planesService.StartFlight(plane5).ContinueWith((result) =>
            //{
            //    t = false;
            //    Console.WriteLine("Plane5 finished");
            //    location = planesService.GetCurrentLocation(plane5);
            //    Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
            //    planesService.ResetFlight(plane5);
            //    location = planesService.GetCurrentLocation(plane5);
            //    Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);

            //});

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;


            var task = Task.Run(() =>
            {
                var fileName = Guid.NewGuid().ToString();

                var bufferSize = Encoding.Default.GetByteCount(t);

                Console.WriteLine(fileName);
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew(fileName, bufferSize))
                {
                    bool mutexCreated;
                    Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryWriter writer = new BinaryWriter(stream);
                        writer.Write(t);
                    }
                    mutex.ReleaseMutex();

                    Console.WriteLine(fileName);
                    var process = Process.Start(@"D:\Study\University\Projects\PlaneFlightsEmulator\FlightsEmulator\FlightDisplay\FlightDisplay\bin\Debug\FlightDisplay.exe", fileName);
                    process.WaitForExit();
                    var exitCode = process.ExitCode;

                }

                return fileName;
            });
            //.ContinueWith((b) => {
            //    Console.WriteLine(b.Result);
            //    var process = Process.Start(@"D:\Study\University\Projects\PlaneFlightsEmulator\FlightsEmulator\FlightDisplay\FlightDisplay\bin\Debug\FlightDisplay.exe", b.Result);
            //    process.WaitForExit();
            //    var exitCode = process.ExitCode;
            //});
            //var c = task.ContinueWith((result) =>
            //{
            //    var process = Process.Start(_display.ToString(), fileName);

            //    process.WaitForExit();

            //    var exitCode = process.ExitCode;
            //}); ;


            //var task = new Task(() =>
            //{
            //   //var fileName =  Guid.NewGuid().ToString();

            //   //var bufferSize = Encoding.Default.GetByteCount(t);

              
            //   // using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew(fileName, bufferSize))
            //   // {
            //   //     bool mutexCreated;
            //   //     Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
            //   //     using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            //   //     {
            //   //         BinaryWriter writer = new BinaryWriter(stream);
            //   //         writer.Write(t);
            //   //     }
            //   //     mutex.ReleaseMutex();
            //   // }

            //   // return fileName;

            //    //ProcessStartInfo info = new ProcessStartInfo(@"D:\Study\University\Projects\PlaneFlightsEmulator\FlightsEmulator\FlightDisplay\FlightDisplay\bin\Debug\FlightDisplay.exe");
            //    //    var process = Process.Start(_display.ToString(), fileName);

            //    //process.WaitForExit();

            //    //var exitCode = process.ExitCode;
            //    //info.UseShellExecute = false;
            //    //info.RedirectStandardInput = true;
            //    //info.RedirectStandardOutput = true;


            //    //using (Process process = Process.Start(info))
            //    //{
            //    //    StreamWriter sw = process.StandardInput;
            //    //    StreamReader sr = process.StandardOutput;

            //    //    while (t)
            //    //    {
            //    //        sw.WriteLine("test");
            //    //        location = planesService.GetCurrentLocation(plane5);
            //    //        Thread.Sleep(1000);
            //    //        sw.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
            //    //    }

            //    //    //foreach (string command in commands)
            //    //    //{
            //    //    //    sw.WriteLine(command);
            //    //    //}

            //    //    sw.Close();
            //    //    //returnvalue = sr.ReadToEnd();
            //    //}

            //    //return returnvalue;

            //    //AllocConsole();
            //    //Console.WriteLine("test");
            //    //location = planesService.GetCurrentLocation(plane5);
            //    //Thread.Sleep(1000);
            //    //Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
            //    //i++;


            //}, token);
            //task.Start();
            //task.ContinueWith((result) =>
            //{
            //    var process = Process.Start(_display.ToString(), fileName);

            //    process.WaitForExit();

            //    var exitCode = process.ExitCode;
            //});



        //    var task2 = new Task(() =>
        //    {

        //        //ProcessStartInfo info = new ProcessStartInfo(@"D:\Study\University\Projects\PlaneFlightsEmulator\FlightsEmulator\FlightDisplay\FlightDisplay\bin\Debug\FlightDisplay.exe");
        //        var process = Process.Start(_display.ToString(), t);

        //        process.WaitForExit();

        //        var exitCode = process.ExitCode;
             


        //    }, token);
        //    task2.Start();
        //    //task2.ContinueWith((result) =>
        //    //{
        //    //    Console.WriteLine("2 Finished!");
        //    //});


        //    var task3 = new Task(() =>
        //    {

        //        //ProcessStartInfo info = new ProcessStartInfo(@"D:\Study\University\Projects\PlaneFlightsEmulator\FlightsEmulator\FlightDisplay\FlightDisplay\bin\Debug\FlightDisplay.exe");
        //        var process = Process.Start(@"D:\Study\University\Projects\PlaneFlightsEmulator\FlightsEmulator\FlightDisplay\FlightDisplay\bin\Debug\FlightDisplay.exe", t);

        //        process.WaitForExit();

        //        var exitCode = process.ExitCode;



        //    }, token);
        //    task3.Start();
        //    //task3.ContinueWith((result) =>
        //    //{
        //    //    Console.WriteLine("3 Finished!");
        //    //});
        //    //var i = 1;

        //    //i = 1;
        //    //while (i < 100)
        //    //{
        //    //    location = planesService.GetCurrentLocation(plane);
        //    //    Thread.Sleep(1000);
        //    //    Console.WriteLine("plane - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
        //    //    i++;
        //    //}
        //    //location = planesService.GetCurrentLocation(plane);
        //    //Console.WriteLine("plane - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
        //    //planesService.ResetFlight(plane);
        //    //location = planesService.GetCurrentLocation(plane);
        //    //Console.WriteLine("plane - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);

        //    //location = planesService.GetCurrentLocation(plane5);
        //    //Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
        //    //planesService.ResetFlight(plane5);
        //    //location = planesService.GetCurrentLocation(plane5);
        //    //Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);

           Console.ReadLine();
      }





        //    #region Constants

        //    private const string PRINT_PLANES_CMD = "print";
        //    private const string SELECT_PLANE_CMD = "select";
        //    private const string FLY_UP_CMD = "flyup";
        //    private const string RESET_FLIGHT_CMD = "reset";
        //    private const string EXIT_CMD = "q";
        //    private const string HELP_CMD = "-help";

        //    private const string IS_BINARY_FLAG = "-b";

        //    private const int CMD_WITHOUT_PARAM_MAX_ITEM_COUNT = 1;
        //    private const int CMD_WITH_PARAM_MAX_ITEM_COUNT = 2;
        //    private const int CMD_WITH_FLAG_MAX_ITEM_COUNT = 3;
        //    private const int CMD_PARAM_ARG_NUMBER = 1;
        //    private const int CMD_FLAG_ARG_NUMBER = 2;


        //    #endregion

        //    static readonly IPlanesService PlanesService = new PlanesService();
        //    static PlaneModel _plane = null;
        //    private static void Main(string[] args)
        //    {
        //        Console.WriteLine(Resources.StartMsg);
        //        RunCommand();
        //    }


        //    private static void RunCommand()
        //    {
        //        var command = Console.ReadLine();
        //        List<string> commandElements = new List<string>();
        //        if (command != null)
        //        {
        //            commandElements = command.Split(' ').ToList();
        //        }


        //        if (commandElements.Any())
        //        {
        //            switch (commandElements[0])
        //            {
        //                case PRINT_PLANES_CMD:
        //                    if (commandElements.Count == CMD_WITHOUT_PARAM_MAX_ITEM_COUNT)
        //                    {
        //                        var planes = PlanesService.GetAllPlanes();
        //                        foreach (var plane in planes)
        //                        {
        //                            PrintPlaneInfo(plane);
        //                        }
        //                    }
        //                    else if (commandElements.Count == CMD_WITH_PARAM_MAX_ITEM_COUNT)
        //                    {
        //                        int id;
        //                        if (int.TryParse(commandElements[CMD_PARAM_ARG_NUMBER], out id))
        //                        {
        //                            var plane = PlanesService.GetPlaneById(id);
        //                            PrintPlaneInfo(plane);
        //                        }
        //                    }
        //                    RunCommand();
        //                    break;

        //                case SELECT_PLANE_CMD:
        //                    if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT)
        //                    {
        //                        int id;
        //                        if (int.TryParse(commandElements[CMD_PARAM_ARG_NUMBER], out id))
        //                        {
        //                            _plane = PlanesService.GetPlaneById(id);
        //                        }
        //                    }
        //                    RunCommand();
        //                    break;

        //                case FLY_UP_CMD:
        //                    if (_plane != null)
        //                    {
        //                        var currentPlane = _plane;
        //                        PlanesService.StartFlight(currentPlane).ContinueWith((result) =>
        //                        {
        //                            Console.WriteLine("Plane number" + currentPlane.Id + " landed");
        //                        });
        //                    }
        //                    RunCommand();
        //                    break;

        //                case RESET_FLIGHT_CMD:
        //                    if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT)
        //                    {
        //                        var name = commandElements[CMD_PARAM_ARG_NUMBER];

        //                    }
        //                    RunCommand();
        //                    break;

        //                case EXIT_CMD:
        //                    break;

        //                case HELP_CMD:

        //                    Console.WriteLine();
        //                    RunCommand();
        //                    break;

        //                default:
        //                    Console.WriteLine(Resources.WrongCommandMsg);
        //                    RunCommand();
        //                    break;
        //            }
        //        }
        //    }

        //    private static void PrintPlaneInfo(PlaneModel plane)
        //    {
        //        Console.WriteLine("Plane number: " + plane.Id + "; Current position: lat - " + plane.CurrentLocation.lat +
        //                          ", lng - " + plane.CurrentLocation.lng + ", alt - " + plane.CurrentLocation.alt);
        //    }

        
    }


}
