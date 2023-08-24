using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.DataAccess
{
    public class ParkingZoneData
    {
        /// <summary>
        /// it return the id of zone whose name is provided as a parameter.
        /// </summary>
        /// <param name="zoneTitle"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetParkingZoneIdByName(string zoneTitle)
        {
            int zoneId = 0; 
            using(var dbContext = new ParkingManagementEntities())
            {
                if(!dbContext.Parking_Zone.Where(i => i.Parking_Zone_Title == zoneTitle).Any())
                {
                    throw new Exception("Zone Title does not Exist");
                }
                zoneId=dbContext.Parking_Zone.Where(i=>i.Parking_Zone_Title== zoneTitle).Select(i=>i.Parking_Zone_Id).SingleOrDefault();
            }
            return zoneId;
        }
        /// <summary>
        /// it return the name of the zone pf provided id as parameter.
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetParkingZoneNameById(int zoneId)
        {
            string zoneName = "";
            using (var dbContext = new ParkingManagementEntities())
            {

                if(!dbContext.Parking_Zone.Where(i => i.Parking_Zone_Id == zoneId).Any())
                {
                    throw new Exception("Parking Zone Id Does not Exist where zoneId="+zoneId.ToString());
                }
                zoneName = dbContext.Parking_Zone.Where(i => i.Parking_Zone_Id == zoneId).Select(i => i.Parking_Zone_Title).SingleOrDefault();
            }
            return zoneName;
        }
        /// <summary>
        /// It returns the details of the zone in the database.
        /// </summary>
        /// <returns>
        /// It returns the details of the zone in the database.
        /// </returns>
        public static List<ParkingZoneModel> GetDetailsOfAllZones()
        {
            List <ParkingZoneModel> parkingZoneList = new List <ParkingZoneModel>();    
            using (var dbContext = new ParkingManagementEntities())
            {
                var items = dbContext.Parking_Zone;
                foreach (var item in items)
                {
                    ParkingZoneModel parkingZoneModel = new ParkingZoneModel();
                    parkingZoneModel.Parking_Zone_Title = item.Parking_Zone_Title;
                    parkingZoneModel.Parking_Zone_Id = item.Parking_Zone_Id;
                    parkingZoneList.Add(parkingZoneModel);
                }
            }
            return parkingZoneList;
        }

    }
}
