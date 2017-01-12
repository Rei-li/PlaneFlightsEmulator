using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;

namespace FlightsEmulator
{
    class PlanesService : IPlanesService
    {
        IPlanesRepository _repository;
        //List<ActivePlaneModel> _activePlanes = new List<ActivePlaneModel>();
        Dictionary<int, CancellationTokenSource> _activePlanes = new Dictionary<int, CancellationTokenSource>();
        Dictionary<int, InterprocessDTO> _dtos = new Dictionary<int, InterprocessDTO>();

        private readonly static string _display = ConfigurationManager.AppSettings["FlightDisplayApp"];

        public PlanesService()
        {
            _repository = new PlanesRepository();
        }

        public void StartFlight(int planeId)
        {
            if (!CheckIfActive(planeId))
            {

                var plane = GetPlaneById(planeId);
                var planeString = plane.JsonOut();

                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;


                var task = Task.Run(() =>
                {
                    var fileName = Guid.NewGuid().ToString();
                    var mutexName = Guid.NewGuid().ToString();

                    var bufferSize = Encoding.Default.GetByteCount(planeString);

                    Console.WriteLine(fileName);
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

                        Console.WriteLine(fileName);
                        InterprocessDTO dto = new InterprocessDTO()
                        {
                            MemoryMappedFileName = fileName,
                            MutexName = mutexName
                        };
                        _dtos.Add(planeId, dto);

                        var process = Process.Start(_display.ToString(), dto.JsonOut().ToString());
                        process.WaitForExit();
                        var exitCode = process.ExitCode;

                    }
                }, token).ContinueWith((result) =>
                {
                    _activePlanes.Remove(planeId);
                });
                _activePlanes.Add(planeId, tokenSource);
            }
        }
        
        public void ResetFlight(int planeId)
        {
            //var activePlane = _activePlanes.Single(p => p.Plane.Id == plane.Id);
            //activePlane.TokenSource.Cancel();
            //_activePlanes.Remove(activePlane);
            if (CheckIfActive(planeId))
            {
                CancellationTokenSource cancellationSource = null;
                _activePlanes.TryGetValue(planeId, out cancellationSource);
                cancellationSource?.Cancel();
                _activePlanes.Remove(planeId);
                ResetLocation(planeId);
            }
        }

        public bool CheckIfActive(int planeId)
        {
          return  _activePlanes.ContainsKey(planeId);
        }

        public PathPoint GetCurrentLocation(int id)
        {
            //if (CheckIfActive(id))
            //{
               
            //}
            //else
            //{
                return GetPlaneById(id).CurrentLocation;
            //}
        }

        public IList<PlaneModel> GetAllPlanes()
        {
            return _repository.GetAllPlanes();
        }


        public PlaneModel GetPlaneById(int id)
        {
            return _repository.GetPlaneById(id);
        }

        private void ResetLocation(int id)
        {
            _repository.ResetLocation(id);
        }

        private void ResetAll()
        {
            _repository = new PlanesRepository();
        }



    }
}
