using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEmulator
{
    public interface IPlanesService
    {
        Task StartFlight(PlaneModel plane);

        void ResetFlight(PlaneModel plane);

        PathPoint GetCurrentLocation(PlaneModel plane);

        IList<PlaneModel> GetAllPlanes();

        PlaneModel GetPlaneById(int id);

    }
}
