using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ParkingManagement.Utils
{
    public class CommonAuthClass
    {
        public static int GetCurrentUserId()
        {
            if (HttpContext.Current.Session==null || HttpContext.Current.Session["UserId"] == null)
            {
                return 0;
            }
            return int.Parse(HttpContext.Current.Session["UserId"].ToString());
        }
    }
}
