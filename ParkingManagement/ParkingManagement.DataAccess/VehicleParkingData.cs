using ParkingManagement.Utils;
using ParkingManagement.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingManagement.DataAccess
{
    public class VehicleParkingData
    {
        private static Mutex mutex = new Mutex();
        /// <summary>
        /// 
        /// </summary>
        /// Parmaters: 
        ///     -Parking Space Id
        /// <returns>
        ///     It Searches the vehicle parking database and matches the is parking space id  
        ///     if it exist it returns register vehicle no that parking space
        ///     else throws an exception that it does not exist;
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static string GetvehicleRegistrationNumberBySpace(int parkingSpaceId)
        {
            string vehicleRegistrationNumber = "";
            using (var dbContext = new ParkingManagementEntities())
            {
                if(!dbContext.Vehicle_Parking.Where(i=>i.Parking_Space_Id==parkingSpaceId).Any())
                {
                    throw new Exception("Parking Space Id does not exist");
                }
                if (dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId && i.Actual_Release_Time==null).Select(i => i.Registration_No).Any())
                {
                    vehicleRegistrationNumber = dbContext.Vehicle_Parking.
                        Where(i => i.Parking_Space_Id == parkingSpaceId && i.Actual_Release_Time == null).
                        Select(i => i.Registration_No).
                        FirstOrDefault();
                }
            }
            return vehicleRegistrationNumber;
        }
        /// <summary>
        /// 
        /// </summary>
        /// Parmaters: 
        ///     -Parking Space Id
        /// <returns>
        ///     It Searches the vehicle parking database and matches the is parking space id  
        ///     if it exist it returns register vehicle ID in that parking space
        ///     else throws an exception that it does not exist;
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static int GetvehicleIdBySpace(int parkingSpaceId)
        {
            int VehicleBookedId = 0;
            using (var dbContext = new ParkingManagementEntities())
            {
                if (!dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId).Any())
                {
                    throw new Exception("Parking Space Id does not exist");
                }
                if (dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId && i.Actual_Release_Time == null).Select(i => i.Registration_No).Any())
                {
                    VehicleBookedId = dbContext.Vehicle_Parking.
                        Where(i => i.Parking_Space_Id == parkingSpaceId && i.Actual_Release_Time == null).
                        Select(i => i.Vehicle_Parking_Id).
                        SingleOrDefault();
                }
            }
            return VehicleBookedId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// Parmaters: 
        ///     -Parking Space Id
        /// <returns>
        ///     It Searches the vehicle parking database and matches the is parking space id  
        ///     if it exist it returns Vehicle Information in that parking space
        ///     else throws an exception that it does not exist;
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static VehicleParkingModel GetDetailsOfVechileInSpaceId(int parkingSpaceId)
        {
            VehicleParkingModel vehicleParkingModel = new VehicleParkingModel();
            using (var dbContext = new ParkingManagementEntities())
            {
                if (!dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId).Any())
                {
                    throw new Exception("Parking Spce Id does not exist");
                }
                var item = dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId).FirstOrDefault();
                vehicleParkingModel.Parking_Space_Id = item.Parking_Space_Id;
                vehicleParkingModel.Parking_Zone_Id = item.Parking_Zone_Id;
                vehicleParkingModel.Booking_Date_Time = item.Booking_Date_Time;
                vehicleParkingModel.Release_Date_Time = item.Release_Date_Time;
                vehicleParkingModel.Registration_No = item.Registration_No;
                vehicleParkingModel.Vehicle_Parking_Id = item.Vehicle_Parking_Id;
            }
            return vehicleParkingModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// Parmaters: 
        ///     -Parking Space Id
        /// <returns>
        ///     It Searches the vehicle parking database and matches the is parking space id  
        ///     if it exist it returns register whether vehicle is present in that space or not as YES or NO
        ///     else throws an exception that it does not exist;
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static string IsAnyVehiclePresentInSpace(int parkingSpaceId)
        {
            string flag = "NO";
            using (var dbContext = new ParkingManagementEntities())
            {
                if (dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId && i.Actual_Release_Time==null).Select(i => i.Registration_No).Any())
                {
                    flag = "YES";
                }
            }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// Parmaters: 
        ///     -vehicleParkingModel
        /// <returns>
        ///     The function takes a custome vehicle parking model which contains all details of new vehicle being parked 
        ///     after that it checks whether any other vehicle is parked on that space where it is going to be parked and  same regiatration no is not parked
        ///     if its true then it throws an exception 
        ///     else it updates the data in the database 
        ///     it uses mutex so that if process is using the function the other process should wait else two vehicle will parked at same spot.
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static int SetNewVehicleBookingDetails(VehicleParkingModel vehicleParkingModel)
        {
            mutex.WaitOne();
            int vehicleParkingId = 0;
            using(var dbContext = new ParkingManagementEntities())
            {
                if(dbContext.Vehicle_Parking.Where(i=>i.Parking_Space_Id==vehicleParkingModel.Parking_Space_Id && i.Actual_Release_Time == null).Any())
                {
                    //throw exception;
                    mutex.ReleaseMutex();
                    throw new Exception("Parking Space not free to park");
                }
                if(dbContext.Vehicle_Parking.Where(i=>i.Registration_No==vehicleParkingModel.Registration_No && i.Actual_Release_Time == null).Any())
                {
                    mutex.ReleaseMutex();
                    throw new Exception("Vehicle With Same Registration no Already Parked");
                }
                Vehicle_Parking obj = new Vehicle_Parking();
                obj.Parking_Space_Id=vehicleParkingModel.Parking_Space_Id;
                obj.Parking_Zone_Id = vehicleParkingModel.Parking_Zone_Id;
                obj.Registration_No = vehicleParkingModel.Registration_No;
                obj.Booking_Date_Time = DateTime.Now.ToString("yyyy-MM-dd HH-mm");
                obj.Release_Date_Time=vehicleParkingModel.Release_Date_Time;
                dbContext.Vehicle_Parking.Add(obj);
                dbContext.SaveChanges();
                vehicleParkingId = obj.Vehicle_Parking_Id;
            }
            mutex.ReleaseMutex();
            return vehicleParkingId;  
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
        /// <exception cref="Exception"></exception>
        public static VehicleParkingModel GetVehicleDetailsById(int vehicleParkingId)
        {
            VehicleParkingModel vehicleParkingModel = new VehicleParkingModel();
            using (var dbContext = new ParkingManagementEntities())
            {
                if (!dbContext.Vehicle_Parking.Where(i => i.Vehicle_Parking_Id == vehicleParkingId).Any())
                {
                    throw new Exception("Vehicle Parking Id does not exist");
                }
                var item = dbContext.Vehicle_Parking.Where(i => i.Vehicle_Parking_Id == vehicleParkingId).FirstOrDefault();
                vehicleParkingModel.Parking_Space_Id = item.Parking_Space_Id;
                vehicleParkingModel.Parking_Zone_Id = item.Parking_Zone_Id;
                vehicleParkingModel.Booking_Date_Time = item.Booking_Date_Time;
                vehicleParkingModel.Release_Date_Time = item.Release_Date_Time;
                vehicleParkingModel.Registration_No = item.Registration_No;
                vehicleParkingModel.Vehicle_Parking_Id= item.Vehicle_Parking_Id;
                
                var y = DateTime.ParseExact(item.Release_Date_Time, "yyyy-MM-dd HH-mm", System.Globalization.CultureInfo.InvariantCulture);
                var x = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH-mm"), "yyyy-MM-dd HH-mm", System.Globalization.CultureInfo.InvariantCulture);
                vehicleParkingModel.TimeLeft =(y-x).ToString();
            }
            return vehicleParkingModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleParkingId"></param>
        ///  /// <returns>
        /// it takes vehicle parking id as input and searches the database for that id and
        /// then it updates the actual release time of that id so that the space will be free
        /// if id does not exist it throws an exception.
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static void ReleaseVehicleParkingSlot(int vehicleParkingId)
        {
            
            using(var dbContext = new ParkingManagementEntities())
            {
                if (!dbContext.Vehicle_Parking.Where(i => i.Vehicle_Parking_Id == vehicleParkingId).Any())
                {
                    throw new Exception("Vehicle Parking Id does not exist");
                }
                Vehicle_Parking obj =  dbContext.Vehicle_Parking.Where(i=>i.Vehicle_Parking_Id==vehicleParkingId).FirstOrDefault();
                obj.Actual_Release_Time= DateTime.Now.ToString("yyyy-MM-dd HH-mm");
                dbContext.SaveChanges();
            }
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
            List<ReportParkingZoneModel> parkingZoneList = new List<ReportParkingZoneModel>(); 
            using(var dbContext = new ParkingManagementEntities())
            {
                var items = dbContext.Vehicle_Parking.GroupBy(i=>i.Parking_Zone_Id);
                
                foreach (var item in items)
                {
                    ReportParkingZoneModel reportParkingZoneModel = new ReportParkingZoneModel();
                    int zoneId  = item.Key;
                    string zoneName = ParkingZoneData.GetParkingZoneNameById(zoneId);
                    var parkingSpaceGrouplist = item.GroupBy(i => i.Parking_Space_Id);
                    List<ReportParkingSpaceModel> reportParkingSpaceModelList = new List<ReportParkingSpaceModel>();
                    foreach(var item2 in parkingSpaceGrouplist)
                    {
                        ReportParkingSpaceModel reportParkingSpaceModel = new ReportParkingSpaceModel();
                        var parkingSpaceId = item2.Key;
                        var parkingSpaceName=ParkingSpaceData.GetParkingSpaceNameById(parkingSpaceId);
                        var countOfOccupiedSpace = 0;
                        var countOfBookedSpace = 0;
                        foreach (var item3 in item2)
                        {
                            if (item3.Booking_Date_Time.Split(' ')[0].Equals(date))
                            {
                                countOfBookedSpace++;
                            }
                            if (item3.Booking_Date_Time.Split(' ')[0].Equals(date) && item3.Actual_Release_Time == null)
                            {
                                countOfOccupiedSpace++;
                            }
                        }
                        if (countOfBookedSpace != 0)
                        {
                            reportParkingZoneModel.ZoneName = zoneName;
                            reportParkingSpaceModel.ParkingSpaceName = parkingSpaceName;
                            reportParkingSpaceModel.NoOfBooking = countOfBookedSpace;
                            reportParkingSpaceModel.IsCurrentlyVehicleparked = countOfOccupiedSpace == 0 ? "NO" : "YES";

                            reportParkingSpaceModelList.Add(reportParkingSpaceModel);
                        }
                    }
                    if (reportParkingZoneModel.ZoneName != null)
                    {
                        reportParkingZoneModel.ParkingSpaceList = reportParkingSpaceModelList;
                        parkingZoneList.Add(reportParkingZoneModel);
                    }
                }
                
            }
            return parkingZoneList;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingSpaceId"></param>
        /// <returns>
        ///  It Searches the vehicle parking database and matches the is parking space id  
        ///     if it exist it returns release time of vehicle that parking space
        ///     else throws an exception that it does not exist;
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static string GetVehicleReleaseTimeBySpace(int parkingSpaceId)
        {
            string vehicleReleaseTime = "";

            using (var dbContext = new ParkingManagementEntities())
            {
                if (!dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId).Any())
                {
                    throw new Exception("Parking Spce Id does not exist");
                }
                if (dbContext.Vehicle_Parking.Where(i => i.Parking_Space_Id == parkingSpaceId && i.Actual_Release_Time == null).Select(i => i.Registration_No).Any())
                {
                    vehicleReleaseTime = dbContext.Vehicle_Parking.
                    Where(i => i.Parking_Space_Id == parkingSpaceId && i.Actual_Release_Time == null).
                    Select(i => i.Release_Date_Time).
                    FirstOrDefault();
                }
            }
            return vehicleReleaseTime;
        }

    }
}
