using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEmulator
{
    public interface IPlanesService
    {
        void StartFlight(int id);

        void ResetFlight(int id);

        PathPoint GetCurrentLocation(int id);

        IList<PlaneModel> GetAllPlanes();

        PlaneModel GetPlaneById(int id);

    }
}
