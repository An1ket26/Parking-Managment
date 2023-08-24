using ParkingManagement.DataAccess;
using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Business
{
    public class ParkingZoneBusiness
    {
        /// <summary>
        /// It returns the details of the zone in the database.
        /// </summary>
        /// <returns>
        /// It returns the details of the zone in the database.
        /// </returns>
        public static List<ParkingZoneModel> GetDetailsOfAllZones()
        {
            return ParkingZoneData.GetDetailsOfAllZones();
        }

	}
}
