using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightsEmulator
{
    public class PlaneModel
    {
        public int Id { set; get; }
        public PathPoint CurrentLocation { set; get; }
        public IList<PathPoint> Path { set; get; }
        public string JsonOut()
        {
            //Return json
            return JsonConvert.SerializeObject(this);
        }
    }
}
