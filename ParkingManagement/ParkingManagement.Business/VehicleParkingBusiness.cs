using ParkingManagement.DataAccess;
using ParkingManagement.Utils;
using ParkingManagement.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Business
{
    public class VehicleParkingBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleParkingModel"></param>
        /// <returns>
        ///     The function takes a custome vehicle parking model which contains all details of new vehicle being parked 
        ///     after that it checks whether any other vehicle is parked on that space where it is going to be parked and  same regiatration no is not parked
        ///     if its true then it throws an exception 
        ///     else it updates the data in the database 
        ///     it uses mutex so that if process is using the function the other process should wait else two vehicle will parked at same spot.
        /// </returns>
        public static int SetNewVehicleBookingDetails(VehicleParkingModel vehicleParkingModel)
        {
            return VehicleParkingData.SetNewVehicleBookingDetails(vehicleParkingModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleParkingId"></param>
        /// <returns>
        /// it takes vehicle parking id as input and searches the database for that id and
        /// returns the vehicle details regarding that id
        /// if it is not found then throws an exception that the id does not exist.
        /// </returns>
        public static VehicleParkingModel GetVehicleDetailsById(int vehicleParkingId)
        {
            return VehicleParkingData.GetVehicleDetailsById(vehicleParkingId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingSpaceId"></param>
        /// <returns>
        ///  It Searches the vehicle parking database and matches the is parking space id  
        ///     if it exist it returns Vehicle Information in that parking space
        ///     else throws an exception that it does not exist;
        /// </returns>
        public static VehicleParkingModel GetDetailsOfVechileInSpaceId(int parkingSpaceId)
        {
            return VehicleParkingData.GetDetailsOfVechileInSpaceId(parkingSpaceId);
        }
        /// <summary>
        ///  it takes vehicle parking id as input and searches the database for that id and
        /// then it updates the actual release time of that id so that the space will be free
        /// if id does not exist it throws an exception.
        /// </summary>
        /// <param name="vehicleParkingId"></param>
        public static void ReleaseParkingSpace(int vehicleParkingId)
        {
            VehicleParkingData.ReleaseVehicleParkingSlot(vehicleParkingId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns>
        /// It takes as an input and find all the vehicle being parked on that date and how many are still 
        /// in parking and return the data as list of custome ReportParking Spcae model
        /// </returns>
        public static List<ReportParkingZoneModel> GenerateReports(string date)
        {
            return VehicleParkingData.GenerateReports(date);
        }
    }
}
