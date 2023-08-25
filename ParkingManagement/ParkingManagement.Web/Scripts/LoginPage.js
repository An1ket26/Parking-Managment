$(document).ready(function () {
    //validate
    $("#loginBtn").on('click', function (e) {
        e.preventDefault();
        showLoadingBar();
        $(this).prop('disabled', true);
        var userName = $("#userNameInput").val();
        var password = $("#passwordInput").val();
        //validate
        const regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        if (password.length == 0 || userName.length == 0 || regex.test(userName)==false) {
            //show error Modal
            $(this).prop('disabled', false);
            hideLoadingBar();
            showErrorModal("Invalid Email or Password");
            
            return;
        }
        //ajax call
        console.log("Login Clicked");
        const loginDetails = {}
        loginDetails.Password = password;
        loginDetails.Email = userName;
        $.ajax({
            type: "POST",
            url: 'LoginPage.aspx/ValidateLoginDetails',
            data: JSON.stringify({ userLoginDetails: loginDetails }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d ==null) {
                    //error modal
                    hideLoadingBar();
                    $("#LoginBtn").prop('disabled', false);
                    showErrorModal("Invalid Email or Password")
                    
                } else {
                    hideLoadingBar();
                    $("#LoginBtn").prop('disabled', false);
                    window.location.href = data.d;
                    
                    
                }
            },
            failure: function (response) {
                hideLoadingBar();
                showErrorModal("Something went wrong please try again!!");
                $(this).prop('disabled', false);
            }
        });
        
    })
    $("#modalContainer2").on('click', "#cancelErrorModalBtn", function () {
        $("#modalContainer2").css("display", "none");
    })

    function createErrorModal(message) {
        return `<div class="error-modal">
        <p class="error-modal-title">Error</p>
          <p class="error-modal-text">${message}</p>
          <button class="error-modal-btn" id="cancelErrorModalBtn">Ok</button>
                 </div >`;
    }
    function showErrorModal(message) {
        $("#modalContainer2").html("");
        $("#modalContainer2").html(createErrorModal(message));
        $("#modalContainer2").css({ "display": "flex", "justify-content": "center" });
    }

    function createLoadingModal() {
        return `<div class="loading-spinner-div"><img src="../Images/loading.gif" class="loading-spinner"></div>`;
    }

    function showLoadingBar() {
        $("#modalContainer").html("");
        $("#modalContainer").html(createLoadingModal());
        $("#modalContainer").css("display", "block");
    }
    function hideLoadingBar() {
        $("#modalContainer").css("display", "none");
    }


})