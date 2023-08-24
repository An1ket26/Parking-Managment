using ParkingManagement.Business;
using ParkingManagement.Utils;
using ParkingManagement.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.Caching;
using System.Web.Hosting;

namespace ParkingManagement.Web
{
    public partial class Dashboard : Auth
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Auth.IsUserBookingAgent() == "false")
            {
                try
                {
                    ReportsUserControl temp = (ReportsUserControl)LoadControl("ReportsUserControl.ascx");
                    reportsDiv.Controls.Add(temp);
                }
                catch (Exception ex)
                {
                    LogRecords.LogRecord(ex);
                }
            }

        }

        [System.Web.Services.WebMethod]
        public static List<ParkingSpaceModel> GetParkingSpaceDetailsByZone(string zoneId)
        {
            try
            {
                if(CommonAuthClass.GetCurrentUserId()==0)
                {
                    throw new Exception("Trying to access data without login");
                }
                return ParkingSpaceBusiness.GetParkingSpaceDetailsByZone(int.Parse(zoneId));
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }
        
        [System.Web.Services.WebMethod]
        public static string InitializeApplication()
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                
                if (IsUserBookingAgent() == "false")
                {
                    throw new Exception("other user is trying to use Release Slot feature");
                }
                InitializeApplicationBusiness.InitializeAllDetails();
                return "Done";
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }
        
        [System.Web.Services.WebMethod]
        public static int SetNewVehicleBookingDetails(VehicleParkingModel vehicleParkingModel)
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                if (IsUserBookingAgent() == "false")
                {
                    throw new Exception("other user is trying to use Release Slot feature");
                }
                if (vehicleParkingModel.Registration_No.Length == 0 || vehicleParkingModel.Release_Date_Time.Length == 0)
                {
                    return 0;
                }
                return VehicleParkingBusiness.SetNewVehicleBookingDetails(vehicleParkingModel);
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return 0;
            }
        }
        
        [System.Web.Services.WebMethod]
        public static VehicleParkingModel GetVehicleDetailsById(string vehicleParkingId)
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                return VehicleParkingBusiness.GetVehicleDetailsById(int.Parse(vehicleParkingId));
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }

        [System.Web.Services.WebMethod]
        public static VehicleParkingModel GetDetailsOfVechileInSpaceId(string parkingSpaceId)
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                return VehicleParkingBusiness.GetDetailsOfVechileInSpaceId(int.Parse(parkingSpaceId));
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }

        [System.Web.Services.WebMethod]
        public static string ReleaseParkingSpace(string vehicleParkingId)
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                if (IsUserBookingAgent() == "false")
                {
                    throw new Exception("other user is trying to use Release Slot feature");
                }
                VehicleParkingBusiness.ReleaseParkingSpace(int.Parse(vehicleParkingId));
                return "Done";
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }

        [System.Web.Services.WebMethod]
        public static List<ParkingZoneModel> GetDetailsOfAllZones()
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                return ParkingZoneBusiness.GetDetailsOfAllZones();
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }
        
        [System.Web.Services.WebMethod]
        public static List<ReportParkingZoneModel> GenerateReports(string date)
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                return VehicleParkingBusiness.GenerateReports(date);
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }
        
        [System.Web.Services.WebMethod]
        public static string CheckIfUserIsBookingAgent()
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                return IsUserBookingAgent();
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetUserName()
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                return UserBusiness.GetUserName(CommonAuthClass.GetCurrentUserId());
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }

        /// <summary>
        /// It used create a fake url for js and css files
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RenameStaticFile(string path)
        {
            try
            {
                if (HttpRuntime.Cache[path] == null)
                {
                    string absolute = HostingEnvironment.MapPath("~" + path);
                    DateTime date = File.GetLastWriteTime(absolute);
                    FileInfo fileInfo = new FileInfo(absolute);
                    string extn = fileInfo.Extension;
                    path=path.Substring(0,path.Length-extn.Length);
                    string result = path+"/v-" + date.Ticks+"/aniket"+ extn;
                    HttpRuntime.Cache.Insert(path, result, new CacheDependency(absolute));
                }

                return HttpRuntime.Cache[path] as string;
            }catch(Exception ex)
            {
                LogRecords.LogRecord(ex);
                return path;
            }
        }
    }
}
