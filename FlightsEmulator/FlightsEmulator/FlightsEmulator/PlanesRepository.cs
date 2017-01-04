using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FlightsEmulator
{
    internal class PlanesRepository : IPlanesRepository
    {
        private IList<PlaneModel> _planes = new List<PlaneModel>();

        private readonly string _file = ConfigurationManager.AppSettings["SourceFile"];
        private readonly string _directoryPath = ConfigurationManager.AppSettings["RepositoryPath"];

        public PlanesRepository()
        {
            var file = _directoryPath + _file;
            if (File.Exists(file))
            {
                var text = File.ReadAllText(file);
                JArray obj = JArray.Parse(text);
                var planeId = 1;
                foreach (var path in obj)
                {
                    var plane = new PlaneModel();
                    plane.Path = new List<PathPoint>();
                    JArray points = JArray.Parse(path.ToString());
                    foreach (var point in points)
                    {
                         
                        var p =  JObject.Parse(point.ToString()).ToObject<PathPoint>();
                        plane.Path.Add(p);
                    }
                    plane.Id = planeId;
                    plane.CurrentLocation = plane.Path.First();
                    //plane.CurrentLat = plane.Path.First().lat;
                    //plane.CurrentLng = plane.Path.First().lng;
                    //plane.CurrentAlt = plane.Path.First().alt;
                    _planes.Add(plane);
                    planeId++;
                }

            }
        }


        public IList<PlaneModel> GetAllPlanes()
        {
            return _planes;
        }

        public PlaneModel GetPlaneById(int id)
        {
            return _planes.FirstOrDefault(p => p.Id == id);
        }

        public void ResetLocation(int id)
        {
            var plane = _planes.FirstOrDefault(p => p.Id == id);
            if (plane != null)
            {
                plane.CurrentLocation = plane.Path.First();
                //plane.CurrentLat = plane.Path.First().lat;
                //plane.CurrentLng = plane.Path.First().lng;
                //plane.CurrentAlt = plane.Path.First().alt;
            }
           
        }

    }
}
