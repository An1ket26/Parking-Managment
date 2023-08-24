using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ParkingManagement.Web
{
    /// <summary>
    /// Summary description for StaticFilePathDecrypter
    /// </summary>
    public class StaticFilePathDecrypter : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            FileInfo fileInfo = new FileInfo(path);
            string ext=fileInfo.Extension;
            string filePath = path.Substring(0,path.LastIndexOf('/'));
            filePath=filePath.Substring(0,filePath.LastIndexOf("/"));
            string contentType = "";
            if(ext.Substring(1)=="js")
            {
                contentType = "javascript";
            }
            else
            {
                contentType = ext.Substring(1);
            }
            context.Response.Clear();
            context.Response.ContentType = "text/"+ contentType;
            context.Response.AddHeader("content-disposition", "inline;filename=" + Path.GetFileName("~"+filePath+ext));
            context.Response.WriteFile("~"+filePath+ext);
            context.Response.End();
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