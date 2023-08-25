using ParkingManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ParkingManagement.Web
{
    /// <summary>
    /// Convert the recieved fake path into original path and return the contents.
    /// </summary>
    public class StaticFilePathDecrypter : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string path = context.Request.Url.AbsolutePath;
                FileInfo fileInfo = new FileInfo(path);
                string ext = fileInfo.Extension;
                string filePath = path.Substring(0, path.LastIndexOf('/'));
                filePath = filePath.Substring(0, filePath.LastIndexOf("/"));
                string contentType = "";
                if (ext.Substring(1) == "js")
                {
                    contentType = "javascript";
                }
                else
                {
                    contentType = ext.Substring(1);
                }
                if (string.IsNullOrEmpty(filePath) && !File.Exists(filePath + ext))
                {

                    context.Response.ContentType = "text/plain";
                    context.Response.Write("File not be found!");
                    context.ApplicationInstance.CompleteRequest();
                }
                context.Response.Clear();
                context.Response.ContentType = "text/" + contentType;
                context.Response.AddHeader("content-disposition", "inline;filename=" + Path.GetFileName("~" + filePath + ext));
                context.Response.WriteFile("~" + filePath + ext);
                
                context.ApplicationInstance.CompleteRequest();
            }
            catch(Exception ex)
            {
                LogRecords.LogRecord(ex);
                
                context.Response.ContentType = "text/plain";
                context.Response.Write("Something Went Wrong !!!");
                context.ApplicationInstance.CompleteRequest();
            }
        }
            

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}