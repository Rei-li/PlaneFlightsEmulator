using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEmulator
{
    public interface IPlanesService
    {
        Task StartFlight(int id);
  
        PathPoint GetCurrentLocation(int id);

        bool CheckIfActive(int planeId);

        IList<PlaneModel> GetAllPlanes();

        PlaneModel GetPlaneById(int id);

    }
}
