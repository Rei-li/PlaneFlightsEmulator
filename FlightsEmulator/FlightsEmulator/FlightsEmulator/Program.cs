using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightsEmulator
{
    class Program
    {


        //static void Main(string[] args)
        //{
        //    IPlanesService planesService = new PlanesService();
        //    var plane = planesService.GetPlaneById(3);
        //    var plane5 = planesService.GetPlaneById(5);
        //    var location = planesService.GetCurrentLocation(plane);
        //    //Console.WriteLine("lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);

        //    planesService.StartFlight(plane).ContinueWith((result) => {
        //        Console.WriteLine("Plane finished");
        //    });
        //    bool t = true;
        //    planesService.StartFlight(plane5).ContinueWith((result) =>
        //    {
        //        t = false;
        //        Console.WriteLine("Plane5 finished");
        //        location = planesService.GetCurrentLocation(plane5);
        //        Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
        //        planesService.ResetFlight(plane5);
        //        location = planesService.GetCurrentLocation(plane5);
        //        Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);

        //    });
        //    //var i = 1;
        //    while (t)
        //    {
        //         location = planesService.GetCurrentLocation(plane5);
        //        Thread.Sleep(1000);
        //        Console.WriteLine("plane5 - lat:" + location.lat + "; lng:" + location.lng + "; alt:" + location.alt);
        //        //i++;
        //    }
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
        //}





        #region Constants

        private const string CREATE_CMD = "create";
        private const string SET_LIBRARY_CMD = "lb";
        private const string GET_ALBOMS_BY_YEAR_CMD = "alb";
        private const string GET_ALBOMS_BY_SINGER_CMD = "albn";
        private const string GET_ALBOMS_GROUPPED_CMD = "gr";
        private const string EXIT_CMD = "q";
        private const string HELP_CMD = "-help";

        private const string IS_BINARY_FLAG = "-b";

        private const int CMD_WITH_PARAM_MAX_ITEM_COUNT = 2;
        private const int CMD_WITH_FLAG_MAX_ITEM_COUNT = 3;
        private const int CMD_PARAM_ARG_NUMBER = 1;
        private const int CMD_FLAG_ARG_NUMBER = 2;


        #endregion

        static IPlanesService _planesService = new PlanesService();
        private static void Main(string[] args)
        {
            Console.WriteLine(Resources.StartMsg);
            RunCommand();
        }


        private static void RunCommand()
        {
            var command = Console.ReadLine();
            List<string> commandElements = new List<string>();
            if (command != null)
            {
                commandElements = command.Split(' ').ToList();
            }


            if (commandElements.Any())
            {
                switch (commandElements[0])
                {
                    case CREATE_CMD:
                        if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT)
                        {
                            var name = commandElements[CMD_PARAM_ARG_NUMBER];
                        }

                        RunCommand();
                        break;

                    case SET_LIBRARY_CMD:
                        if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT)
                        {
                            var name = commandElements[CMD_PARAM_ARG_NUMBER];
                           
                        }
                        RunCommand();
                        break;

                    case GET_ALBOMS_BY_YEAR_CMD:
                          int year;
                            if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT && int.TryParse(commandElements[CMD_PARAM_ARG_NUMBER], out year))
                            {

                        
                            }
                            
                        

                        RunCommand();
                        break;
                        
                    case EXIT_CMD:
                        break;

                    case HELP_CMD:
   
                        Console.WriteLine();
                        RunCommand();
                        break;

                    default:
                        Console.WriteLine(Resources.WrongCommandMsg);
                        RunCommand();
                        break;
                }
            }
        }


    }
    

}
