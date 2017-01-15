using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightsEmulator
{
    class PlanesService : IPlanesService
    {
        readonly IPlanesRepository _repository;
        readonly Dictionary<int, InterprocessDTO> _dtos = new Dictionary<int, InterprocessDTO>();

        private const string DISPLAY_APP_SETTINGS = "FlightDisplayApp";
        private readonly static string Display = ConfigurationManager.AppSettings[DISPLAY_APP_SETTINGS];

        public PlanesService()
        {
            _repository = new PlanesRepository();
        }

        /// <summary>
        /// starts new process, in wich plane starts flight
        /// </summary>
        /// <param name="planeId">id of plane to start flight</param>
        /// <returns>task object wich starts process for flight</returns>
        public Task StartFlight(int planeId)
        {
            if (!CheckIfActive(planeId))
            {
                var plane = GetPlaneById(planeId);
                var planeString = plane.JsonOut();

                return Task.Run(() =>
                {
                    var fileName = Guid.NewGuid().ToString();
                    var mutexName = Guid.NewGuid().ToString();
                    var bufferSize = Encoding.Default.GetByteCount(planeString);
                    
                    using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew(fileName, bufferSize))
                    {
                        bool mutexCreated;
                        Mutex mutex = new Mutex(true, mutexName, out mutexCreated);
                        using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                        {
                            BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write(planeString);
                        }
                        mutex.ReleaseMutex();
                       
                        InterprocessDTO dto = new InterprocessDTO()
                        {
                            MemoryMappedFileName = fileName,
                            MutexName = mutexName
                        };

                        _dtos.Add(planeId, dto);

                        var process = Process.Start(Display.ToString(), fileName + " " + mutexName);
                        process?.WaitForExit();

                        _dtos.Remove(planeId);
                    }
                });

            }
            return null;
        }
        
        /// <summary>
        /// check if plane is active or is waiting
        /// </summary>
        /// <param name="planeId">plane id to chech</param>
        /// <returns></returns>
        public bool CheckIfActive(int planeId)
        {
          return _dtos.ContainsKey(planeId);
        }

        /// <summary>
        /// get curren plane location
        /// </summary>
        /// <param name="id">id of plane to find location</param>
        /// <returns>current location</returns>
        public PathPoint GetCurrentLocation(int id)
        {
            PathPoint point = null;
            if (CheckIfActive(id))
            {
                InterprocessDTO dto = null;
                if (_dtos.TryGetValue(id, out dto) && dto != null)
                {
                    using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(dto.MemoryMappedFileName))
                    {
                        Mutex mutex = Mutex.OpenExisting(dto.MutexName);
                        mutex.WaitOne();
                        using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                        {
                            BinaryReader reader = new BinaryReader(stream);
                            var result = reader.ReadString();
                            point = JsonConvert.DeserializeObject<PathPoint>(result);
                        }

                        mutex.ReleaseMutex();
                    }
                }
            }
            else
            {
                point =  GetPlaneById(id).CurrentLocation;
            }
            return point;
        }

        /// <summary>
        /// get list of all planes available
        /// </summary>
        /// <returns>planes' list</returns>
        public IList<PlaneModel> GetAllPlanes()
        {
            return _repository.GetAllPlanes();
        }

        /// <summary>
        /// get particular plane
        /// </summary>
        /// <param name="id">id of plane to get</param>
        /// <returns>plane</returns>
        public PlaneModel GetPlaneById(int id)
        {
            return _repository.GetPlaneById(id);
        }

    }
}
