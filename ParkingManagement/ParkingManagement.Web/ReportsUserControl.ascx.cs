using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ParkingManagement.Business;
using ParkingManagement.Utils.Models;
using ParkingManagement.Utils;
using System.Web.UI.HtmlControls;

namespace ParkingManagement.Web
{
    public partial class ReportsUserControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// It generates the pdf of the report of selected Date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GeneratePdf(object sender, EventArgs e)
        {
            try
            {
                if(Auth.IsUserBookingAgent()== "true")
                {
                    throw new Exception("Not Authorized to extract pdf");
                }
                var inputDate = Page.Request.Form["reportDateInput"];
                if(inputDate.Length==0)
                {
                    throw new Exception("No Date Selected");
                }
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    BaseFont headingFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font titleFont = new Font(headingFont, 18, Font.BOLD);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12);
                    BaseColor titleColor = new BaseColor(41, 128, 185);
                    BaseColor tableHeaderColor = new BaseColor(189, 195, 199);

                    Paragraph title = new Paragraph("Reports", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20f;
                    
                    document.Add(title);

                    Paragraph date = new Paragraph("Date: " + inputDate, normalFont);
                    date.Alignment = Element.ALIGN_LEFT;
                    date.SpacingAfter = 20f;
                    document.Add(date);
                    PdfPTable table = new PdfPTable(4);
                    
                    //table.DefaultCell.BackgroundColor = tableHeaderColor;
                    table.DefaultCell.BorderWidth = 1;
                    table.DefaultCell.Padding = 5;
                    table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    
                    var cell1 = new PdfPCell(new Phrase("Parking Zone", normalFont));
                    var cell2 = new PdfPCell(new Phrase("Parking Space", normalFont));
                    var cell3 = new PdfPCell(new Phrase("No of Booking", normalFont));
                    var cell4 = new PdfPCell(new Phrase("Is Vehicle Currently Parked", normalFont));
                    cell1.BackgroundColor = tableHeaderColor;
                    cell2.BackgroundColor = tableHeaderColor;
                    cell3.BackgroundColor = tableHeaderColor;
                    cell4.BackgroundColor = tableHeaderColor;
                    table.AddCell(cell1);
                    table.AddCell(cell2);
                    table.AddCell(cell3);
                    table.AddCell(cell4);
                    




                    List<ReportParkingZoneModel> temp = VehicleParkingBusiness.GenerateReports(inputDate);
                    foreach (var report in temp)
                    {
                        int i = 0;
                        foreach (var item in report.ParkingSpaceList)
                        {
                            if (i == 0)
                            {
                                table.AddCell(new Phrase(report.ZoneName, normalFont));
                            }
                            else
                            {
                                table.AddCell(new Phrase(" ", normalFont));

                            }


                            table.AddCell(new Phrase(item.ParkingSpaceName, normalFont));
                            table.AddCell(new Phrase(item.NoOfBooking.ToString(), normalFont));
                            table.AddCell(new Phrase(item.IsCurrentlyVehicleparked, normalFont));
                            i++;
                        }
                    }



                    document.Add(table);

                    document.Close();
                    Response.Clear();
                    Response.BufferOutput = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=Reports.pdf");
                    Response.BinaryWrite(memoryStream.ToArray());
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                LogRecords.LogRecord(ex);
                string message = "Something Went Wrong,Please Try Again!!";
                string script2 = $@"document.addEventListener(""DOMContentLoaded"", function(event) {{alert(`{message}`);}})";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(),script2 , true);
            }
        }
    }
}