<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="ParkingManagement.Web.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>
    <link rel="stylesheet" href="<%=RenameStaticFile("/StyleSheets/LoginPage.css")%>" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Roboto+Slab:wght@300;400&display=swap" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
</head>
<body>
    <div id="mainContainer" class="container-div">
        <form id="form1" runat="server" class="login-form">
            <div class="login-header">
                <h2>Agent Login</h2>
                <p class="heading-text">Hey,Enter your details to get sign in</p>
                <p>to your account</p>
            </div>
            <div class="login-body">
                <div class="inputs-field-div">
                    <input type="text" id="userNameInput" placeholder="Enter UserName" class="inputs-field" />
                </div>
                <div class="inputs-field-div">
                    <input type="password" id="passwordInput" placeholder="Password" class="inputs-field" />
                </div>
            </div>
            <div class="login-footer">
                <button value="Login" class="login-btn" id="loginBtn">Login</button><br />
                <div class="login-signup">
                    <%--<p>Don't Have an Account? <a href="loginpage"> Create One</a></p>--%>
                </div>
            </div>
        </form>
    </div>
    <div class="modal-container" id="modalContainer">
    </div>
    <div class="modal-container-2" id="modalContainer2">
    </div>
    <script src="<%=RenameStaticFile("/Scripts/LoginPage.js")%>"></script>
</body>
</html>
