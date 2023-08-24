using ParkingManagement.DataAccess;
using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Business
{
    public class ParkingSpaceBusiness
    {
        /// <summary>
        /// it takes the zoneId as input and returns all details of the parking spaces included in that zone.
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<ParkingSpaceModel> GetParkingSpaceDetailsByZone(int zoneId)
        {
            return ParkingSpaceData.GetParkingSpaceDetailsByZone(zoneId);
        }
    }
}
