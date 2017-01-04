using System;
using System.Collections.Generic;
using System.IO;
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
        List<ActivePlaneModel> _activePlanes = new List<ActivePlaneModel>();

        public PlanesService()
        {
            _repository = new PlanesRepository();
        }

        public Task StartFlight(PlaneModel plane)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;


            var t = new Task(() =>
            {
                foreach (var point in plane.Path)
                {
                    plane.CurrentLocation = point;
                    Thread.Sleep(1000);
                }

            }, token);

           
            _activePlanes.Add(new ActivePlaneModel()
            {
                Plane = plane,
                TokenSource = tokenSource
            });

            t.Start();
            return t;
        }

        private void SetNextLocation(ActivePlaneModel activePlane)
        {
            var nextPointIndex = activePlane.Plane.Path.IndexOf(activePlane.Plane.CurrentLocation) + 1;
            if (activePlane.Plane.Path.Count > nextPointIndex)
            {
                activePlane.Plane.CurrentLocation = activePlane.Plane.Path[nextPointIndex];
            }

        }

        public void ResetFlight(PlaneModel plane)
        {
            var activePlane = _activePlanes.Single(p => p.Plane.Id == plane.Id);
            activePlane.TokenSource.Cancel();
            _activePlanes.Remove(activePlane);
            ResetLocation(plane.Id);
        }

        public PathPoint GetCurrentLocation(PlaneModel plane)
        {
            return plane.CurrentLocation;
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
