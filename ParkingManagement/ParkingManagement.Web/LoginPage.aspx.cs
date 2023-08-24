using ParkingManagement.Business;
using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ParkingManagement.Web
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [System.Web.Services.WebMethod]
        public static string ValidateLoginDetails(UserModel userLoginDetails)
        {
            try
            {
                int userId = UserBusiness.ValidateUser(userLoginDetails);
                if (userId == 0)
                {
                    return null;
                }
                else
                {
                    //store in session;
                    Auth.StoreUserIdInSession(userId);
                    return "DashBoard?tab=dashboard";
                }
            }catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return null;
            }
        }
        [System.Web.Services.WebMethod]
        public static string LogOut()
        {
            try
            {
                if (CommonAuthClass.GetCurrentUserId() == 0)
                {
                    throw new Exception("Trying to access data without login");
                }
                Auth.ClearSession();
                return "Done";
            }catch(Exception ex) 
            { 
                LogRecords.LogRecord(ex);
                return null;
            }
        }

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
                    path = path.Substring(0, path.Length - extn.Length);
                    string result = path + "/v-" + date.Ticks + "/aniket" + extn;
                    HttpRuntime.Cache.Insert(path, result, new CacheDependency(absolute));
                }

                return HttpRuntime.Cache[path] as string;
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                return "";
            }
        }
    }
}