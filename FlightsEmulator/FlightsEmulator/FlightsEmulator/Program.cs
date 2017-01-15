using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsEmulator
{

    class Program
    {
        #region Constants

        private const string PRINT_PLANES_CMD = "print";
        private const string GET_CURRENT_LOCATION_CMD = "locate";
        private const string GET_STATE_CMD = "status";
        private const string FLY_UP_CMD = "start";
        private const string EXIT_CMD = "q";
        private const string HELP_CMD = "-help";

        private const int CMD_WITHOUT_PARAM_MAX_ITEM_COUNT = 1;
        private const int CMD_WITH_PARAM_MAX_ITEM_COUNT = 2;
        private const int CMD_PARAM_ARG_NUMBER = 1;

        #endregion

        static IPlanesService _planesService = null;

        private static void Main(string[] args)
        {
            try
            {
                _planesService = new PlanesService();
                Console.WriteLine(Resources.StartMsg);
                RunCommand();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();

            }
        }


        private static void RunCommand()
        {
            string command = Console.ReadLine();
           
            List<string> commandElements = new List<string>();
            if (command != null)
            {
                commandElements = command.Split(' ').ToList();
            }


            if (commandElements.Any())
            {
                int id = 0;
                switch (commandElements.First())
                {
                    case PRINT_PLANES_CMD:
                        if (commandElements.Count == CMD_WITHOUT_PARAM_MAX_ITEM_COUNT)
                        {
                            var planes = _planesService.GetAllPlanes();
                            foreach (var plane in planes)
                            {
                                
                                PrintPlaneInfo(plane.Id);
                            }
                        }
                        else if (commandElements.Count == CMD_WITH_PARAM_MAX_ITEM_COUNT && int.TryParse(commandElements[CMD_PARAM_ARG_NUMBER], out id))
                        {
                            var plane = _planesService.GetPlaneById(id);
                            PrintPlaneInfo(plane.Id);
                        }
                        else
                        {
                            Console.WriteLine(Resources.WrongPlaneNumberdMsg);
                        }
                        RunCommand();
                        break;

                    case GET_CURRENT_LOCATION_CMD:

                        if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT && int.TryParse(commandElements[CMD_PARAM_ARG_NUMBER], out id))
                        {
                            var location = _planesService.GetCurrentLocation(id);
                            PrintPlaneInfo(id, location.lat, location.lng, location.alt);
                          
                        }
                        else
                        {
                            Console.WriteLine(Resources.WrongPlaneNumberdMsg);
                        }
                        RunCommand();
                        break;

                    case GET_STATE_CMD:
                        if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT && int.TryParse(commandElements[CMD_PARAM_ARG_NUMBER], out id))
                        {
                            Console.WriteLine(
                                _planesService.CheckIfActive(id) ? Resources.PlaneActiveMsg : Resources.PlaneWaitingMsg,
                                id);
                        }
                        else
                        {
                            Console.WriteLine(Resources.WrongPlaneNumberdMsg);
                        }
                        RunCommand();
                        break;

                    case FLY_UP_CMD:
                        if (commandElements.Count >= CMD_WITH_PARAM_MAX_ITEM_COUNT && int.TryParse(commandElements[CMD_PARAM_ARG_NUMBER], out id))
                        {
                            var startTask = _planesService.StartFlight(id);
                            if (startTask != null)
                            {
                                startTask.ContinueWith(
                                    prev =>
                                    {
                                        if (prev.Exception != null)
                                        {
                                            Console.WriteLine(Resources.SmthWentWrongMsg);
                                            foreach (var ex in prev.Exception.InnerExceptions)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine(Resources.SmthWentWrongMsg);
                                        }
                                    },
                                TaskContinuationOptions.OnlyOnFaulted);
                            }
                            else
                            {
                                Console.WriteLine(Resources.AlreadyActiveMsg);
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine(Resources.WrongPlaneNumberdMsg);
                        }
                        RunCommand();
                        break;

                    case EXIT_CMD:
                        break;

                    case HELP_CMD:

                        Console.WriteLine();
                        Console.WriteLine(Resources.HelpPrintCmd);
                        Console.WriteLine();
                        Console.WriteLine(Resources.HelpStatusCmd);
                        Console.WriteLine();
                        Console.WriteLine(Resources.HelpStratCmd);
                        Console.WriteLine();
                        Console.WriteLine(Resources.HelpLocationCmd);
                        Console.WriteLine();
                        Console.WriteLine(Resources.HelpExitCmd);
                        Console.WriteLine();
                        Console.WriteLine(Resources.HelpCmd);
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

        private static void PrintPlaneInfo(int id)
        {
            var status = _planesService.CheckIfActive(id) ? Resources.ActiveStatusMsg : Resources.WaitingStatusMsg;
            Console.WriteLine(Resources.PlaneInfo, id, status);
        }

        private static void PrintPlaneInfo(int id, decimal lat, decimal lng, decimal alt)
        {
            Console.WriteLine(Resources.PlaneCurrentLocationInfo, id, lat, lng, alt);

            
        }


    }


}
