using ParkingManagement.Business;
using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkingManagement.Web
{
    public class Auth:System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (CommonAuthClass.GetCurrentUserId()==0)
            {
                ClearSession();
                Response.Redirect("loginpage");
            }
            if (Request.QueryString["tab"]!=null && Request.QueryString["tab"]=="reports" && IsUserBookingAgent()=="true")
            {
                Response.Redirect("dashboard?tab=dashboard");
            }
            if (Request.QueryString["tab"] != null && Request.QueryString["tab"] != "dashboard" && IsUserBookingAgent() == "true")
            {
                Response.Redirect("dashboard?tab=dashboard");
            }
            if(Request.QueryString["tab"] == null)
            {
                Response.Redirect("dashboard?tab=dashboard");
            }
        }

        /// <summary>
        /// It is used to store the current users id into session
        /// </summary>
        /// <param name="userId"></param>
        public static void StoreUserIdInSession(int userId)
        {
            HttpContext.Current.Session["UserId"] = userId;
            HttpContext.Current.Session.Timeout = 60;
        }

        /// <summary>
        /// It is used to check whether user is booking agent or not
        /// </summary>
        /// <returns></returns>
        public static string IsUserBookingAgent()
        {
            return UserBusiness.IsUserBookingAgent(CommonAuthClass.GetCurrentUserId())==true?"true":"false";
        }
        /// <summary>
        /// It is used to clear all session data for logout
        /// </summary>
        public static void ClearSession()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
        }
    }
}
