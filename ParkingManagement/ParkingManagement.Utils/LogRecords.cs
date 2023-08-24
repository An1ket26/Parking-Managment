using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Utils
{
    public class LogRecords
    {
        public static void LogRecord(Exception ex)
        {
            try
            {
                string errorPath = ex.ToString();
                string message = ex.Message.ToString() + Environment.NewLine;
                while (ex.InnerException != null)
                {
                    message += ex.InnerException.Message.ToString() + Environment.NewLine;
                    ex = ex.InnerException;
                }
                string ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7).ToString();
                string date = DateTime.Now.ToString();
                string UserDetail = CommonAuthClass.GetCurrentUserId().ToString();
                string design = "==============================================================";
                string pathName = Path.Combine(ConfigurationManager.AppSettings["LogUrl"] + DateTime.Now.ToString("dd-MM-yyyy") + ".txt");
                if (File.Exists(pathName))
                {
                    File.AppendAllText(pathName, design + Environment.NewLine + date + Environment.NewLine + "Current UserId = " + UserDetail
                        + Environment.NewLine + message + Environment.NewLine + ErrorlineNo + Environment.NewLine + errorPath + Environment.NewLine + design + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    File.WriteAllText(pathName, date + Environment.NewLine + design + Environment.NewLine + "Current UserId = " + UserDetail
                        + Environment.NewLine + message + Environment.NewLine + ErrorlineNo + errorPath + Environment.NewLine + Environment.NewLine + design + Environment.NewLine + Environment.NewLine);
                }
            }
            catch (Exception e)
            {

            }
        }
        public static void DebugLogs(string message)
        {
            string pathName = Path.Combine(ConfigurationManager.AppSettings["LogUrl"] + DateTime.Now.ToString("dd-MM-yyyy")+"debug" + ".txt");
            if (File.Exists(pathName))
            {
                File.AppendAllText(pathName, message+ Environment.NewLine);
            }
            else
            {
                File.WriteAllText(pathName,message+ Environment.NewLine);
            }
        }
    }
}
