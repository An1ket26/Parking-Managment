<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportsUserControl.ascx.cs" Inherits="ParkingManagement.Web.ReportsUserControl" %>

<div>
  
    <form runat="server">
        <label for="reportDateInput" class="report-date-label">Date :</label>
        <input type="date" id="reportDateInput"  runat="server" ClientIDMode="Static" class="report-date-input"/>
        <asp:Button ID="extractToPdfBtn" runat="server"  Text="Extract to Pdf" OnClick="GeneratePdf" ClientIDMode="Static" CssClass="display-none extract-pdf-btn"/>
    </form>
    <div id="reportTable">
        <div id="reportTableCaption">
        Reports
        </div>
        <div id="reportTableHeader">
            <div class="table-header-cell">
            Parking Zone
            </div>
            <div class="table-header-cell">
            Parking Space
            </div>
            <div class="table-header-cell">
            No of Booking
            </div>
            <div class="table-header-cell">
            No Vehicle Parked
            </div>
            
        </div>
        <div id="reportTableBody">
            
        </div>
    </div>
    <h2 class="no-data-table-text" id="tableErrorText">Select Date To See Report</h2> 
</div>