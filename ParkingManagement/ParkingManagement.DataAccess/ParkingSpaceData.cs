using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.DataAccess
{
    public class ParkingSpaceData
    {
        /// <summary>
        /// it takes the zoneId as input and returns all details of the parking spaces included in that zone.
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<ParkingSpaceModel> GetParkingSpaceDetailsByZone(int zoneId)
        {
            List<ParkingSpaceModel> parkingSpaceList = new List<ParkingSpaceModel>();
            using (var dbContext = new ParkingManagementEntities())
            {
                if(!(dbContext.Parking_Space.Where(i => i.Parking_Zone_Id == zoneId).Any()))
                {
                    throw new Exception("Zone Id does not exist");
                }
                var spaces = dbContext.Parking_Space.Where(i => i.Parking_Zone_Id == zoneId).ToList();
                foreach (var space in spaces)
                {
                    ParkingSpaceModel parkingSpaceModel = new ParkingSpaceModel();
                    parkingSpaceModel.Parking_Space_Id = space.Parking_Space_Id;
                    parkingSpaceModel.Parking_Space_Title = space.Parking_Space_Title;
                    parkingSpaceModel.Parking_Zone_Id = space.Parking_Zone_Id;
                    parkingSpaceModel.IsBooked = VehicleParkingData.IsAnyVehiclePresentInSpace(space.Parking_Space_Id);
                    if (parkingSpaceModel.IsBooked == "YES")
                    {
                        parkingSpaceModel.BookedRegistrationNumber = VehicleParkingData.GetvehicleRegistrationNumberBySpace(space.Parking_Space_Id);
                        parkingSpaceModel.BookedId = VehicleParkingData.GetvehicleIdBySpace(space.Parking_Space_Id);
                        parkingSpaceModel.ReleaseTime = VehicleParkingData.GetVehicleReleaseTimeBySpace(space.Parking_Space_Id); ;
                    }
                    parkingSpaceList.Add(parkingSpaceModel);
                }
            }
            return parkingSpaceList;
        }
        /// <summary>
        /// It return the name of parking space whose id is provided as a parameter.
        /// </summary>
        /// <param name="parkingSpaceId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetParkingSpaceNameById(int parkingSpaceId)
        {
            var parkingSpaceName = "";
            using (var dbContext = new ParkingManagementEntities())
            {
                if(!dbContext.Parking_Space.Where(i => i.Parking_Space_Id == parkingSpaceId).Any())
                {
                    throw new Exception("Parking space Id does not Exist");
                }

                parkingSpaceName = dbContext.Parking_Space.Where(i => i.Parking_Space_Id == parkingSpaceId).Select(i => i.Parking_Space_Title).Single().ToString();
            }
            return parkingSpaceName;
        }
    }
}
