<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ParkingManagement.Web.Dashboard" %>

<!DOCTYPE html>
<%@ Register Src="~/ReportsUserControl.ascx" TagName="report" TagPrefix="uc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Dashboard</title>
    <link rel="stylesheet" href="<%=RenameStaticFile("/StyleSheets/Dashboard.css")%>"/>
    <link rel="stylesheet" href="<%=RenameStaticFile("/StyleSheets/Reports.css")%>"/>
    <link rel="preconnect" href="https://fonts.googleapis.com"/>
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto+Slab:wght@300;400&display=swap" rel="stylesheet"/>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
    
</head>
<body>
    <div>
        <div>
            <div id="topNavbar" class="top-navbar">
                <p class="welcome-text" id="welcomeText">Welcome Back, Aniket</p>
                <div class="navbar-btns">
                    <button id="initializeBtn" class="initialize-btn">Initialize</button>
                    <button id="logoutBtn" class="logout-btn">Logout</button>
                    
                </div>
            </div>
            <div id="bottomNavbar" class="bottom-navbar">
            </div>
            <div id="zonesDiv" class="main-body display-none">
                <div class="zone-dropdown-div">
                    
                     <select
                        id="zoneDrpDwn"
                        name="ZoneDrpDwn"
                         class="zone-drp-dwn"
                        >
                        <option value="0" selected="selected" disabled="disabled" hidden="hidden">Select Zone
                        </option>
                    </select>
                </div>
                <div id="parkingSpaceDiv" class="parking-space-div">
                   
                </div>
            </div>
            <div  id="reportsDiv" class="display-none" runat="server" >
                
             
                
            </div>
        </div>
    </div>
    <div class="modal-container" id="modalContainer">
    </div>
    <div class="modal-container-2" id="modalContainer2">
    </div>
     <div class="modal-container-3" id="modalContainer3">
    </div>
   
   
    
    <%--<script src="Scripts/Dashboard.js" asp-append-version="true"></script>--%>
    <script src="<%=RenameStaticFile("/Scripts/Dashboard.js")%>"></script>
</body>
</html>
