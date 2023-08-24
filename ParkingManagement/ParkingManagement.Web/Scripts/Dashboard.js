$(document).ready(function () {

    $("#reportDateInput").attr("name", "reportDateInput");

    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    var tab = "";
    /*It Checks the query string of the page and finds whether it is reports tab or dashboard tab */
    if (urlParams.get('tab') !== null) {
        tab = urlParams.get('tab');
        if (tab === "dashboard") {
            $("#reportsDiv").addClass("display-none")
            $("#zonesDiv").removeClass("display-none")
            document.title = "DashBoard";


        } else if (tab === "reports") {
            $("#reportsDiv").removeClass("display-none")
            $("#zonesDiv").addClass("display-none")
            document.title = "Reports"

        }
    }
    else {
        $("#reportsDiv").addClass("display-none")
        $("#zonesDiv").removeClass("display-none")
        document.title = "DashBoard";
    }


    $("#bottomNavbar").on('click', "#navbarDashboardBtn", function () {
        history.pushState("userdetails", 'Title', location.href.split('?')[0] + `?tab=dashboard`);
        $("#reportsDiv").addClass("display-none")
        $("#zonesDiv").removeClass("display-none")
        $(this).addClass("navbarActiveBtns");
        $("#navbarReportBtn").removeClass("navbarActiveBtns");

    })

    $("#bottomNavbar").on('click', "#navbarReportBtn", function () {
        history.pushState("userdetails", 'Title', location.href.split('?')[0] + `?tab=reports`);
        $("#reportsDiv").removeClass("display-none")
        $("#zonesDiv").addClass("display-none")
        $(this).addClass("navbarActiveBtns");
        $("#navbarDashboardBtn").removeClass("navbarActiveBtns");

    })


    function getAllZones() {
        showLoadingBar();
        $.ajax({
            type: "POST",
            url: "dashboard.aspx/GetDetailsOfAllZones",
            data: JSON.stringify({ "message": "send Zone details" }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {
                    hideLoadingBar();
                    showErrorModal("something went wrong please try again")
                } else {

                    populateAllZoneNamesinDropDown("#zoneDrpDwn", data.d);
                    hideLoadingBar();
                }

            },
            failure: function (response) {
                alert(response.d);
                hideLoadingBar();
            }
        })
    }

    var isBookingAgent = "false";
    // Ajax Call To Get Whether user is booking agent or not.
    function checkUserRole() {
        $.ajax({
            type: "POST",
            url: "dashboard.aspx/CheckIfUserIsBookingAgent",
            data: JSON.stringify({ "message": "Check User" }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {
                    //confirm modal
                    showErrorModal("something went wrong please try again")
                } else {
                    isBookingAgent = data.d;
                    ShowBottomNavbar();
                }
            },
            failure: function (response) {
                alert(response.d);
            }
        })
    }
    // Ajax Call To Get UserName
    function getUserName() {
        $.ajax({
            type: "POST",
            url: "dashboard.aspx/GetUserName",
            data: JSON.stringify({ "message": "Give Username" }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {
                    //confirm modal
                    showErrorModal("something went wrong please try again")
                } else {
                    var name = data.d.toUpperCase();
                    $("#welcomeText").html(`Welcome Back, ${name}`)
                }
            },
            failure: function (response) {
                alert(response.d);
            }
        })
    }

    function ShowBottomNavbar() {
        if (isBookingAgent == "false") {
            $("#bottomNavbar").append(`<button id="navbarDashboardBtn" class="nav-links ${tab === `dashboard` ? `navbarActiveBtns` : ``}">DashBoard</button>
            <button id="navbarReportBtn" class="nav-links ${tab === `reports` ? `navbarActiveBtns` : ``}">Reports</button>`);
            $("#initializeBtn").addClass("display-none");
        }
    }

    function createOption(value, text) {
        var newOption = $("<option>");
        newOption.val(value);
        newOption.text("Zone " + text);
        newOption.attr("name", text);
        return newOption;
    }

    function populateAllZoneNamesinDropDown(selectId, zoneDetails) {
        $(selectId)
            .children()
            .remove()
            .end()
            .append(
                '<option value="none" selected disabled hidden>Select Zone</option>'
            );
        if (zoneDetails == null || zoneDetails.length === 0) {
            $(selectId).append(
                createOption(
                    $($(selectId).attr("parent-id")).val(),
                    $($(selectId).attr("parent-id")).val()
                )
            );
        } else {
            for (const zone of zoneDetails) {
                $(selectId).append(createOption(zone.Parking_Zone_Id, zone.Parking_Zone_Title));
            }
        }

    }

    function createFreeParkingSpaceTemplate(parkingSlot) {
        var template = `<div id="parkingSlotDiv" class="parking-slot-div free-slot">
                            <div class="parking-slot-div-heading">
                                <p><u>${parkingSlot.Parking_Space_Title}</u></p>
                           </div>
                            ${isBookingAgent == `true` ? `<button id="${parkingSlot.Parking_Space_Id}btn" slotId="${parkingSlot.Parking_Space_Id}" 
                            zoneId="${parkingSlot.Parking_Zone_Id}" slotTitle="${parkingSlot.Parking_Space_Title}" class="book-slot-btn">Book</button>` : ``}
                        </div>`;
        return template;
    }
    function createBookedParkingSpaceTemplate(parkingSlot) {
        var date = parkingSlot.ReleaseTime.split(' ')[0];
        var time = parkingSlot.ReleaseTime.split(' ')[1];
        var hours = time.split('-')[0];
        var AmOrPm = hours >= 12 ? 'pm' : 'am';
        hours = (hours % 12) || 12;
        var minutes = time.split('-')[1];
        var finalTime = hours + ":" + minutes + " " + AmOrPm;


        var template = `<div id="parkingSlotDiv" class="parking-slot-div booked-slot">
                         <div class="parking-slot-div-heading">
                            <p><u>${parkingSlot.Parking_Space_Title}</u></p>  </div><div class="parking-slot-div-body">
                            <p class="parking-slot-div-register-no"> Register No : ${parkingSlot.BookedRegistrationNumber}</p>
                            <p class="parking-slot-div-register-no">Release Time : ${date} ${finalTime}</p>
                            ${isBookingAgent == `true` ? `<button id="${parkingSlot.Parking_Space_Id}btn" slotId="${parkingSlot.Parking_Space_Id}" 
                            zoneId="${parkingSlot.Parking_Zone_Id}" slotTitle="${parkingSlot.Parking_Space_Title}" bookedId=${parkingSlot.BookedId} registrationNo="${parkingSlot.BookedRegistrationNumber}" class="release-slot-btn">Release</button>` : ``}
                        </div>
                       </div>`;
        return template;
    }
    function createBookModalTemplate(zoneId, slotId, btnId) {
        var minDate = new Date().toISOString().slice(0, new Date().toISOString().lastIndexOf(":"));

        return `<div class="modal">
                    <div class="book_modal_title"><u>Book Slot</u></div>
           
                    <div class="book_modal_content">  
                        <div class="book-modal-label-container"><label for="RegistrationNoInp">Registration No :</label></div>
                        <input type="text" id="RegistrationNoInp${slotId}" class="modal-register-input"/><br />
                        <div class="book-modal-label-container"><label for="">Duration :</label></div>
                        <input  id="releaseTime${slotId}" class="modal-datetime-input" type="datetime-local" min="${minDate}"/>
                    </div><br />
                    <div class="book-modal-buttons-container">
                        <button id="bookModalSubmitBtn" zoneId=${zoneId} slotId=${slotId} registerNoId="RegistrationNoInp${slotId}"
                        releaseTimeId="releaseTime${slotId}" btnId="${btnId}" class="book-slot-modal-btn">Book Slot</button>
                        <button id="bookModalCancelBtn" class="cancel-book-slot-modal-btn">Cancel</button>
                     </div>
                </div>`;
    }

    function createReleaseModalTemplate(data) {

        return `<div class="modal">
                    <div class="release_modal_title">Are you sure you want to Release Slot?</div>
                    <div class="release-modal-buttons-container">
                        <button id="releaseModalYesBtn" bookedId="${data.vehicleParkingId}" class="book-slot-modal-btn">Yes</button>
                        <button id="bookModalCancelBtn" class="cancel-book-slot-modal-btn">No</button>
                    </div>
                </div>`;
    }
    function createLogoutModalTemplate() {
        return `<div class="modal">
                    <div class="release_modal_title">Are you sure you want to Log Out?</div>
                    <div class="release-modal-buttons-container">
                        <button id="logoutYesBtn" class="book-slot-modal-btn">Yes</button>
                        <button id="bookModalCancelBtn" class="cancel-book-slot-modal-btn">No</button>
                    </div>
                </div>`;
    }

    function createInitializeModalTemplate() {
        return `<div class="error-modal">
                    <p class="initialize-modal-title">Confirm</p>
                    <p class="initialize-modal_text">Are You Sure Want To Clear All Data and Initialize ?</p>
                    <button class="initialize-modal-yes-btn " id="modalInitializeYesBtn">Yes</button>
                    <button class="initialize-modal-no-btn" id="modalNoBtn">No</button>
                </div >`;
    }

    function populateParkingSpaceDiv(parkingSlots) {
        $("#parkingSpaceDiv").html("");
        for (var parkingSlot of parkingSlots) {
            if (parkingSlot.IsBooked === "YES") {
                $("#parkingSpaceDiv").append(createBookedParkingSpaceTemplate(parkingSlot));
            }
            else {
                $("#parkingSpaceDiv").append(createFreeParkingSpaceTemplate(parkingSlot));
            }

        }
    }

    function getParkingSpaceDetailsByZone(zoneId) {
        showLoadingBar();
        $.ajax({
            type: "POST",
            url: 'Dashboard.aspx/GetParkingSpaceDetailsByZone',
            data: JSON.stringify({ zoneId: zoneId }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {
                    hideLoadingBar();
                    showErrorModal("something went wrong please try again")
                } else {

                    populateParkingSpaceDiv(data.d);
                    hideLoadingBar();
                }

            },
            failure: function (response) {
                hideLoadingBar();
                showErrorModal("something went wrong please try again")
            }
        });
    }
    function getParkingSpaceDetailsByZoneWithoutReload(zoneId) {

        $.ajax({
            type: "POST",
            url: 'Dashboard.aspx/GetParkingSpaceDetailsByZone',
            data: JSON.stringify({ zoneId: zoneId }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {

                    showErrorModal("something went wrong please try again")
                } else {

                    populateParkingSpaceDiv(data.d);

                }

            },
            failure: function (response) {

                showErrorModal("something went wrong please try again")
            }
        });
    }

    function createErrorModal(message) {
        return `<div class="error-modal">
                  <p class="error-modal-title">Error</p>
                  <p class="error-modal-text">${message}</p>
                  <button class="error-modal-btn" id="cancelErrorModalBtn">Ok</button>
                </div >`;
    }
    function createSuccessModal(message) {
        return `<div class="error-modal">
                  <p class="success-modal-title">Success</p>
                  <p class="error-modal-text">${message}</p>
                  <button class="error-modal-btn" id="cancelErrorModalBtn">Ok</button>
                </div >`;
    }

    function createLoadingModal() {
        return `<div class="loading-spinner-div"><img src="../Images/loading.gif" class="loading-spinner"></div>`;
    }

    $("#zoneDrpDwn").on('change', function () {
        var zoneId = $(this).val();
        if (zoneId != 0)
            getParkingSpaceDetailsByZone(zoneId);
    })
    $("#initializeBtn").on('click', function () {
        $("#modalContainer").html("");
        $("#modalContainer").html(createInitializeModalTemplate());
        $("#modalContainer").css({ "display": "flex", "justify-content": "center" });
    })

    $("#modalContainer").on('click', "#modalInitializeYesBtn", function () {
        $("#modalContainer").css("display", "none");
        showLoadingBar();
        $.ajax({
            type: "POST",
            url: 'Dashboard.aspx/InitializeApplication',
            data: JSON.stringify({ "message": "clear all Data" }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {
                    //confirm modal
                    hideLoadingBar();
                    showErrorModal("something went wrong please try again")
                } else {
                    hideLoadingBar();
                    getAllZones();
                    showSuccessModal("Data initialized successfully!!");
                    $("#parkingSpaceDiv").html("");


                }

            },
            failure: function (response) {
                hideLoadingBar();
                showErrorModal("something went wrong please try again")
            }
        });
    })

    $("#modalContainer").on('click', "#modalNoBtn", function () {
        $("#modalContainer").css("display", "none");
    })

    $("#modalContainer2").on('click', "#cancelErrorModalBtn", function () {
        $("#modalContainer2").css("display", "none");
    })

    $("#modalContainer3").on('click', "#cancelErrorModalBtn", function () {
        $("#modalContainer3").css("display", "none");
    })

    //Book or Release
    $("#parkingSpaceDiv").on('click', '#parkingSlotDiv button', function () {
        $("#modalContainer").html("");
        if ($(this).attr("bookedId") != null) {
            //ajax call
            var vehicleParkingId = $(this).attr("bookedId")
            var registrationNumber = $(this).attr("registrationNo");
            var slotName = $(this).attr("slotTitle");
            $("#modalContainer").html(createReleaseModalTemplate({ vehicleParkingId, registrationNumber, slotName }));
        } else {
            var zoneId = $(this).attr("zoneId");
            var slotId = $(this).attr("slotId");
            var btnId = $(this).attr("id");
            var slotName = $(this).attr("slotTitle");
            $("#modalContainer").html(createBookModalTemplate(zoneId, slotId, btnId, slotName));
        }
        $("#modalContainer").css({ "display": "flex", "justify-content": "center" });
    })

    $("#modalContainer").on('click', "#bookModalCancelBtn", function () {
        $("#modalContainer").css("display", "none");
    })

    function checkBookingDate(value) {
        let todayDate = new Date();
        let inputDate = new Date(value);

        const differenceinDate =
            (inputDate - todayDate) / (1000 * 60 * 60 * 24 * 365);

        if (differenceinDate < 0) {
            return false;
        }
        return true;
    }

    $("#modalContainer").on('click', "#bookModalSubmitBtn", function () {

        //validations
        var registrationNo = $(`#${$(this).attr("registerNoId")}`).val();
        var releaseTime = $(`#${$(this).attr("releaseTimeId")}`).val();
        if (registrationNo.length == 0) {
            showErrorModal("Please Enter Valid Registration Number")
            return;
        }
        if (releaseTime.length == 0 || checkBookingDate(releaseTime) == false) {
            showErrorModal("Please choose valid release Time")
            return;
        }
        //making object

        formData = {};
        formData.Parking_Zone_Id = $(this).attr("zoneId");;
        formData.Parking_Space_Id = $(this).attr("slotId");;
        formData.Release_Date_Time = releaseTime.replace('T', ' ').replace(':', '-');;
        formData.Registration_No = registrationNo;
        console.log(formData);
        //ajax call for booking slot
        showLoadingBar();
        $.ajax({
            type: "POST",
            url: 'Dashboard.aspx/SetNewVehicleBookingDetails',
            data: JSON.stringify({ "vehicleParkingModel": formData }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == 0) {
                    //confirm modal
                    hideLoadingBar();
                    showErrorModal("something went wrong please try again")
                } else {
                    var zoneId = $("#zoneDrpDwn").val()
                    getParkingSpaceDetailsByZone(zoneId);
                    hideLoadingBar();
                    showSuccessModal("Slot Booked SuccessFully");
                }
            },
            failure: function (response) {
                hideLoadingBar();
                showErrorModal("something went wrong please try again")
            }
        });
        $("#modalContainer").css("display", "none");
    })

    $("#modalContainer").on('click', "#releaseModalYesBtn", function () {
        //ajax to release slot
        showLoadingBar();
        var vehicleParkingId = $(this).attr("bookedId");
        $.ajax({

            type: "POST",
            url: "dashboard.aspx/ReleaseParkingSpace",
            data: JSON.stringify({ "vehicleParkingId": vehicleParkingId }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d === null) {
                    //confirm modal
                    hideLoadingBar();
                    showErrorModal("Something went wrong please try again")
                } else {
                    //change
                    var zoneId = $("#zoneDrpDwn").val();
                    getParkingSpaceDetailsByZone(zoneId);
                    hideLoadingBar();
                    showSuccessModal("Slot Released SuccessFully");
                }
            },
            failure: function (response) {
                hideLoadingBar();
                showErrorModal("something went wrong please try again")
            }
        })
        $("#modalContainer").css("display", "none");
    })


    $("#modalContainer").on('click', "#logoutYesBtn", function () {
        //ajax to release slot
        showLoadingBar();
        $.ajax({
            type: "POST",
            url: 'loginpage.aspx/LogOut',
            data: JSON.stringify({ "message": "logout" }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {
                    //confirm modal
                    hideLoadingBar();
                    showErrorModal("something went wrong please try again");
                    //alert("something went wrong please try again")
                } else {
                    window.location.href = "loginpage";
                    hideLoadingBar();

                }
            },
            failure: function (response) {
                hideLoadingBar();
                showErrorModal("something went wrong please try again");
            }
        })
        $("#modalContainer").css("display", "none");
    })


    $("#logoutBtn").on('click', function () {

        $("#modalContainer").html(createLogoutModalTemplate());
        $("#modalContainer").css({ "display": "flex", "justify-content": "center" });
    })


    function createReportsTemplate(zoneName, parkingSpaceData) {
        return `<div class="report-table-row">
            <div class="table-body-cell zone-column">
                ${zoneName}
            </div>
            <div class="table-body-cell">
                ${parkingSpaceData.ParkingSpaceName}
            </div>
            <div class="table-body-cell">
                ${parkingSpaceData.NoOfBooking}
            </div>
            <div class="table-body-cell">
                ${parkingSpaceData.IsCurrentlyVehicleparked === "YES" ? `1` : `0`}
            </div>
        </div>`;
    }

    function populateReportTable(reportData) {
        $("#reportTableBody").html("");
        if (reportData.length == 0) {
            $("#tableErrorText").html("No Vehicles Parked on this day");
            return;
        }
        $("#tableErrorText").html("");
        for (var item of reportData) {
            var i = 0;
            for (var item2 of item.ParkingSpaceList) {
                if (i === 0)
                    $("#reportTableBody").append(createReportsTemplate("Zone " + item.ZoneName, item2));
                else
                    $("#reportTableBody").append(createReportsTemplate("", item2));
                i++;
            }
        }
    }

    function GenerateReport(date) {
        showLoadingBar();
        $.ajax({
            type: "POST",
            url: "dashboard.aspx/GenerateReports",
            data: JSON.stringify({ "date": date }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == null) {
                    //confirm modal
                    hideLoadingBar();
                    showErrorModal("something went wrong please try again")

                } else {

                    populateReportTable(data.d);

                    hideLoadingBar();
                }
            },
            failure: function (response) {
                alert(response.d);
                hideLoadingBar();
            }
        })
    }

    if ($("#reportDateInput").val() != "") {
        $("#reportDateInput").val("");
    }

    $("#reportDateInput").on('change', function () {
        $("#extractToPdfBtn").removeClass("display-none");
        $("#extractToPdfBtn").attr("inputDate", $(this).val());
        GenerateReport($(this).val());
    })



    function showErrorModal(message) {
        $("#modalContainer2").html("");
        $("#modalContainer2").html(createErrorModal(message));
        $("#modalContainer2").css({ "display": "flex", "justify-content": "center" });
    }
    function showSuccessModal(message) {
        $("#modalContainer3").html("");
        $("#modalContainer3").html(createSuccessModal(message));
        $("#modalContainer3").css({ "display": "flex", "justify-content": "center" });
    }

    function hideErrorModal() {
        $("#modalContainer2").css("display", "none");
    }
    function showLoadingBar() {
        $("#modalContainer2").html("");
        $("#modalContainer2").html(createLoadingModal());
        $("#modalContainer2").css("display", "block");
    }
    function hideLoadingBar() {
        $("#modalContainer2").css("display", "none");
    }


    // to continuosly update data
    setInterval(() => {
        var zoneId = $("#zoneDrpDwn").val();
        if (zoneId != 0 && zoneId != "" && zoneId != null) {
            getParkingSpaceDetailsByZoneWithoutReload(zoneId);
        }

    }, 10000);



    // Initial calls
    checkUserRole();
    getUserName();
    getAllZones();
})

