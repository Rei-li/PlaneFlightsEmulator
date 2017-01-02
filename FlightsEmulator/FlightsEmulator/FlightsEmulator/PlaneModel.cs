using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEmulator
{
    class PlaneModel
    {
        public int Id { set; get; }
        public decimal CurrentLat { set; get; }
        public decimal CurrentLng { set; get; }
        public decimal CurrentAlt { set; get; }
        public IList<PathPoint> Path { set; get; }
    }
}
