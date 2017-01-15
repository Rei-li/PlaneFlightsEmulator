using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEmulator
{
    public interface IPlanesRepository
    {
        IList<PlaneModel> GetAllPlanes();
        PlaneModel GetPlaneById(int id);
    }
}
