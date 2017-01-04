using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightsEmulator
{
   public class ActivePlaneModel
    {
        public CancellationTokenSource TokenSource { set; get; }
        public PlaneModel Plane { set; get; }
    }
}
