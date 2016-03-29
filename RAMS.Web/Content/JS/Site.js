/***************** GENERAL FUNCTIONS ****************/
// Remove empty options from dropdowns
$("select option").filter(function () { return !this.value || $.trim(this.value).length == 0; }).prop("disabled", "disabled");

// Remove empty options from dropdowns for partial views (Partial views (Loaded with Ajax after triggering some event) will not pickup already loaded JavaScript)
function DisableEmptyOptions()
{
    $("select option").filter(function () { return !this.value || $.trim(this.value).length == 0; }).prop("disabled", "disabled");
}

// Functions for Ajax callback functions
function OnAjaxBegin()
{
    
}

function OnAjaxSuccess(data) // Data represents response to ajax call (Partial view etc.)
{
    
}

function OnAjaxFailure(request, error) // Error represents the error that could have been encountered during the request (Exceptions such as "parsererror" etc)
{
    
}

function OnAjaxComplete(request, status) // Status represents the status of the request when request is completed (Success etc.)
{
    
}


$(document).ajaxStart(function () { $.isLoading({ text: "Loading" }); }).ajaxStop(function () { $.isLoading("hide"); });

// Generate random strings with provided string length and allowed characters (If allowed characters are not provided default characters are used)
function GenerateRandomString(stringLength, allowedCharacters, regexString)
{
    allowedCharacters = (typeof allowedCharacters === "undefined") ? "1234567890!#$&*()+abcdefghijklmnoprstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ" : allowedCharacters;

    var result = "";

    for (var i = 0; i < stringLength; i++)
    {
        var randomNumber = Math.floor(Math.random() * allowedCharacters.length);

        result += allowedCharacters.substring(randomNumber, randomNumber + 1)
    }

    if (typeof regexString !== "undefined")
    {
        var regex = new RegExp(regexString);
        
        if (regex.test(result))
        {
            return result;
        }

        return GenerateRandomString(stringLength, allowedCharacters, regexString);
    }
    else
    {
        return result;
    }
}

// LoadDataTable method loads DataTable for the table with provided id
function LoadDataTable(id, lengthMenuArray, order, sorting)
{
    lengthMenuArray = (typeof lengthMenuArray === "undefined") ? [[10, 15, 20, 25], [10, 15, 20, 25]] : lengthMenuArray;

    order = (typeof order === "undefined") ? [[0, "asc"]] : order;

    $("#" + id).DataTable({ "lengthMenu": lengthMenuArray, "order": order, "aoColumnDefs": [{ "bSortable": false, "aTargets": sorting }] });
}

// LoadAction method loads an action method into a div with provided id
function LoadAction(divId, actionUrl)
{
    $("#" + divId).load(actionUrl, function (response, status, xhr) { return status; });
}

// GetBaseUrl method returns a string representation of current application's base url
function GetBaseUrl()
{
    urlComponents = location.href.split('/');

    return urlComponents[0] + "//" + urlComponents[2];
}

// ShowHidePassword method toggles password visibility of password fields
function ShowHidePassword()
{
    $("#showHidePassword").click(function ()
    {
        if($(".password").attr("type") == "password")
        {
            $(".password").attr("type", "text");
        }
        else
        {
            $(".password").attr("type", "password");
        }
    });
}

// ProfilePictureUpload method allows to remove default "C:\\fakepath\\" path portion of the path to file and to preview the file that is being uploaded
function ProfilePictureUpload()
{
    $('#profile-picture-upload').change(function ()
    {
        $('#path-input-field').val($(this).val().replace("C:\\fakepath\\", ""));

        if (this.files && this.files[0])
        {
            var fileReader = new FileReader();

            fileReader.onload = function (e)
            {
                $('#profile-picture-preview').attr('src', e.target.result);
            }

            fileReader.readAsDataURL(this.files[0]);
        }
    });
}

function setDatepicker()
{
    $(".datepicker-field").datepicker();

    
}
function DisableInput(divId)
{
    $("#" + divId + " :input").attr("disabled", true);
}

function DisableDeleteButton()
{
    var selection = $(".delete-position-selector");

    selection.change(function () {
        $("#delete-position-button").prop("disabled", selection.filter(":checked").length < 1);
    });

    selection.change();
}

function ToggleCheckboxes()
{
    var selection = $(".delete-position-selector");

    if ($("#delete-position-select-all").val() == "Select All")
    {
        selection.filter(":visible").prop("checked", true);

        $("#delete-position-select-all").val("Unselect All");
    }
    else
    {
        selection.filter(":visible").prop("checked", false);

        $("#delete-position-select-all").val("Select All");
    }
    DisableDeleteButton();
}
/************* END OF GENERAL FUNCTIONS *************/

/*************** GENERAL MODAL CONTROLS *************/
function GeneralModalControls()
{
    $("#edit-profile-modal").on("show.bs.modal", function (e) { LoadAction("edit-user-profile-modal-body-div", "/RAMS/Account/EditUserProfile"); });

    $("#change-password-modal").on("show.bs.modal", function (e) { LoadAction("password-change-div", "/RAMS/Account/ChangePassword?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type")); });

    $("#edit-profile-modal").on("hidden.bs.modal", function (e) { $("#edit-user-profile-modal-body-div").empty(); $("#edit-user-profile-message-modal-body-div").empty(); });

    $("#forgot-password-modal").on("show.bs.modal", function (e) { LoadAction("forgot-password-div", "/RAMS/Account/ForgotPassword"); });
}
/*********** END OF GENERAL MODAL CONTROLS **********/

/************ SYSTEM ADMIN MODAL CONTROLS ***********/
function SystemAdminModalControls()
{
    //Users
    $("#new-user-modal").on("show.bs.modal", function (e) { LoadAction("new-user-modal-body-div", "/RAMS/SystemAdmin/User/NewUser"); });

    $("#new-user-modal").on("hidden.bs.modal", function (e) { $("#new-user-modal-body-div").empty(); });

    $("#edit-user-modal").on("show.bs.modal", function (e) { $("#edit-user-modal-title").text("Edit User - " + $(e.relatedTarget).data("user-name")); LoadAction("edit-user-modal-body-div", "/RAMS/SystemAdmin/User/EditUser?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type")); $("#edit-user-message-modal-body-div").empty(); });

    $("#edit-user-modal").on("hidden.bs.modal", function (e) { $("#edit-user-modal-title").text("Edit User"); $("#edit-user-modal-body-div").empty(); });

    $("#reset-password-modal").on("show.bs.modal", function (e) { LoadAction("password-reset-div", "/RAMS/SystemAdmin/User/ResetPassword?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type") + "&email=" + $(e.relatedTarget).data("email") + "&firstName=" + $(e.relatedTarget).data("firstName")); });

    $("#reset-password-modal").on("hidden.bs.modal", function (e) { $("#password-reset-div").empty(); });

    $("#block-delete-user-modal").on("show.bs.modal", function (e)
    {
        if ($(e.relatedTarget).data("action") == "block")
        {
            $("#block-delete-user-modal-title").text("Block User - " + $(e.relatedTarget).data("user-name"));

            LoadAction("block-delete-user-div", "/RAMS/SystemAdmin/User/BlockUser?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type"));
        }
        else if ($(e.relatedTarget).data("action") == "delete")
        {
            $("#block-delete-user-modal-title").text("Delete User - " + $(e.relatedTarget).data("user-name"));

            LoadAction("block-delete-user-div", "/RAMS/SystemAdmin/User/DeleteUser?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type"));
        }
        else if ($(e.relatedTarget).data("action") == "unblock") {
            $("#block-delete-user-modal-title").text("Unblock User - " + $(e.relatedTarget).data("user-name"));

            LoadAction("block-delete-user-div", "/RAMS/SystemAdmin/User/UnblockUser?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type"));
        }
    });

    $("#block-delete-user-modal").on("hidden.bs.modal", function (e) { $("#block-delete-user-div").empty(); });

    //Departments
    $("#new-department-modal").on("show.bs.modal", function (e) { LoadAction("new-department-modal-body-div", "/RAMS/SystemAdmin/Department/NewDepartment"); });

    $("#new-department-modal").on("hidden.bs.modal", function (e) { $("#new-department-modal-body-div").empty(); });

    $("#edit-department-modal").on("show.bs.modal", function (e) { $("#edit-department-modal-title").text("Edit Department - " + $(e.relatedTarget).data("department-name")); LoadAction("edit-department-modal-body-div", "/RAMS/SystemAdmin/Department/EditDepartment?id=" + $(e.relatedTarget).data("department-id")); });

    $("#edit-department-modal").on("hidden.bs.modal", function (e) { $("#edit-department-modal-title").text("Edit Department"); $("#edit-department-modal-body-div").empty(); });

    // Profile
    $("#upload-change-profile-picture-modal").on("show.bs.modal", function (e) {
        if ($(e.relatedTarget).data("action") == "upload") {
            $("#upload-change-profile-picture-modal-title").text("Upload Profile Picture");
        }
        else if ($(e.relatedTarget).data("action") == "change") {
            $("#upload-change-profile-picture-modal-title").text("Change Profile Picture");
        }

        LoadAction("upload-change-profile-picture-modal-body-div", "/RAMS/SystemAdmin/Profile/UploadProfilePicture");
    });

    $("#change-notification-status-modal").on("show.bs.modal", function (e) { LoadAction("change-notification-status-div", "/RAMS/SystemAdmin/Profile/ChangeNotificationStatus?notificationId=" + $(e.relatedTarget).data("notification-id") + "&notificationTitle=" + encodeURIComponent($(e.relatedTarget).data("notification-title")) + "&notificationStatus=" + $(e.relatedTarget).data("notification-status")); });

    $("#change-notification-status-modal").on("hidden.bs.modal", function (e) { $("#change-notification-status-div").empty(); });

    //Categories
    $("#new-category-modal").on("show.bs.modal", function (e) { LoadAction("new-category-modal-body-div", "/RAMS/SystemAdmin/Category/NewCategory"); });

    $("#new-category-modal").on("hidden.bs.modal", function (e) { $("#new-category-modal-body-div").empty(); });

    $("#edit-category-modal").on("show.bs.modal", function (e) { $("#edit-category-modal-title").text("Edit Category - " + $(e.relatedTarget).data("category-name")); LoadAction("edit-category-modal-body-div", "/RAMS/SystemAdmin/Category/EditCategory?id=" + $(e.relatedTarget).data("category-id")); });

    $("#edit-category-modal").on("hidden.bs.modal", function (e) { $("#edit-category-modal-title").text("Edit Category"); $("#edit-category-modal-body-div").empty(); });
    
    $("#admin-position-details-modal").on("show.bs.modal", function (e) { $("#admin-position-details-modal-title").text("Position Details - " + $(e.relatedTarget).data("position-title")); LoadAction("admin-position-details-modal-body-div", "/RAMS/SystemAdmin/Position/PositionDetails?positionId=" + $(e.relatedTarget).data("position-id")); });

    $("#admin-position-details-modal").on("hidden.bs.modal", function (e) { $("#admin-position-closure-confirmation-modal-body-div").empty(); $("#admin-position-details-message-modal-body-div").empty(); });

    $("#delete-position-modal").on("show.bs.modal", function (e) { LoadAction("delete-position-modal-body-div", "/RAMS/SystemAdmin/Position/PositionDeleteConfirmation"); });

    $("#delete-position-modal").on("hidden.bs.modal", function (e) { $("#delete-position-modal-body-div").empty(); });
}
/******** END OF SYSTEM ADMIN MODAL CONTROLS ********/

/************** CUSTOMER MODAL CONTROLS *************/
function CustomerModalControls()
{
    // Profile
    $("#upload-change-profile-picture-modal").on("show.bs.modal", function (e)
    {
        if ($(e.relatedTarget).data("action") == "upload")
        {
            $("#upload-change-profile-picture-modal-title").text("Upload Profile Picture");
        }
        else if ($(e.relatedTarget).data("action") == "change")
        {
            $("#upload-change-profile-picture-modal-title").text("Change Profile Picture");
        }

        LoadAction("upload-change-profile-picture-modal-body-div", "/RAMS/Customer/Profile/UploadProfilePicture");
    });

    $("#change-notification-status-modal").on("show.bs.modal", function (e) { LoadAction("change-notification-status-div", "/RAMS/Customer/Profile/ChangeNotificationStatus?notificationId=" + $(e.relatedTarget).data("notification-id") + "&notificationTitle=" + encodeURIComponent($(e.relatedTarget).data("notification-title")) + "&notificationStatus=" + $(e.relatedTarget).data("notification-status")); });

    $("#change-notification-status-modal").on("hidden.bs.modal", function (e) { $("#change-notification-status-div").empty(); });

    // Position
    $("#new-position-modal").on("show.bs.modal", function (e) { LoadAction("new-position-modal-body-div", "/RAMS/Customer/Position/NewPosition"); });

    $("#new-position-modal").on("hidden.bs.modal", function (e) { $("#new-position-modal-body-div").empty(); });

    $("#position-details-modal").on("show.bs.modal", function (e) { $("#position-details-modal-title").text("Position Details - " + $(e.relatedTarget).data("position-title")); LoadAction("position-details-modal-body-div", "/RAMS/Customer/Position/PositionDetails?positionId=" + $(e.relatedTarget).data("position-id")); });

    $("#position-details-modal").on("hidden.bs.modal", function (e) { $("#position-closure-confirmation-modal-body-div").empty(); $("#position-details-message-modal-body-div").empty(); });

    $("#position-closure-confirmation-modal").on("show.bs.modal", function (e) { LoadAction("position-closure-confirmation-modal-body-div", "/RAMS/Customer/Position/PositionClosure?agentId=" + $(e.relatedTarget).data("agent-id") + "&agentName=" + encodeURIComponent($(e.relatedTarget).data("agent-name")) + "&positionId=" + $(e.relatedTarget).data("position-id") + "&positionTitle=" + encodeURIComponent($(e.relatedTarget).data("position-title")) + "&clientUserName=" + $(e.relatedTarget).data("client-user-name") + "&clientFullName=" + encodeURIComponent($(e.relatedTarget).data("client-full-name"))); });

    $("#position-status-report-modal").on("show.bs.modal", function (e) { $("#position-status-report-modal-title").text("Status Report - " + $(e.relatedTarget).data("position-title")); LoadAction("position-status-report-modal-body-div", "/RAMS/Customer/Report/PositionStatusReport?positionId=" + $(e.relatedTarget).data("position-id")); });

    $("#position-status-report-modal").on("hidden.bs.modal", function (e) { $("#position-status-report-modal-body-div").empty(); });

    $("#position-final-report-modal").on("show.bs.modal", function (e) { $("#position-final-report-modal-title").text("Final Report - " + $(e.relatedTarget).data("position-title")); LoadAction("position-final-report-modal-body-div", "/RAMS/Customer/Report/PositionFinalReport?positionId=" + $(e.relatedTarget).data("position-id")); });

    $("#position-final-report-modal").on("hidden.bs.modal", function (e) { $("#position-final-report-modal-body-div").empty(); });

    $("#candidate-report-modal").on("show.bs.modal", function (e) { $("#candidate-report-modal-title").text("Candidate Report - " + $(e.relatedTarget).data("candidate-name")); LoadAction("candidate-report-modal-body-div", "/RAMS/Customer/Report/CandidateReport?candidateId=" + $(e.relatedTarget).data("candidate-id")); });

    $("#candidate-report-modal").on("hidden.bs.modal", function (e) { $("#candidate-report-modal-body-div").empty(); });
}

/********** END OF CUSTOMER MODAL CONTROLS **********/

/************** AGENCY MODAL CONTROLS *************/
function AgencyModalControls()
{
    // Profile
    $("#upload-change-profile-picture-modal").on("show.bs.modal", function (e) {
        if ($(e.relatedTarget).data("action") == "upload") {
            $("#upload-change-profile-picture-modal-title").text("Upload Profile Picture");
        }
        else if ($(e.relatedTarget).data("action") == "change") {
            $("#upload-change-profile-picture-modal-title").text("Change Profile Picture");
        }

        LoadAction("upload-change-profile-picture-modal-body-div", "/RAMS/Agency/Profile/UploadProfilePicture");
    });

    $("#change-notification-status-modal").on("show.bs.modal", function (e) { LoadAction("change-notification-status-div", "/RAMS/Agency/Profile/ChangeNotificationStatus?notificationId=" + $(e.relatedTarget).data("notification-id") + "&notificationTitle=" + encodeURIComponent($(e.relatedTarget).data("notification-title")) + "&notificationStatus=" + $(e.relatedTarget).data("notification-status")); });

    $("#change-notification-status-modal").on("hidden.bs.modal", function (e) { $("#change-notification-status-div").empty(); });

    // Position

    $("#edit-position-modal").on("show.bs.modal", function (e) { $("#edit-position-report-modal-title").text("Edit Position - " + $(e.relatedTarget).data("position-title")); LoadAction("edit-position-modal-body-div", "/RAMS/Agency/Position/EditPosition?positionId=" + $(e.relatedTarget).data("position-id")); });

    $("#edit-position-modal").on("hidden.bs.modal", function (e) { $("#edit-position-modal-body-div").empty(); $("#edit-position-message-modal-body-div").empty(); });

    $("#edit-candidate-modal").on("show.bs.modal", function (e) { $("#edit-candidate-report-modal-title").text("Candidate Details - " + $(e.relatedTarget).data("candidate-name")); LoadAction("edit-candidate-modal-body-div", "/RAMS/Agency/Position/EditCandidate?candidateId=" + $(e.relatedTarget).data("candidate-id") + "&positionStatus=" + $(e.relatedTarget).data("position-status")); });

    $("#edit-candidate-modal").on("hidden.bs.modal", function (e) { $("#edit-candidate-modal-body-div").empty(); });

    $("#approve-position-modal").on("show.bs.modal", function (e) { LoadAction("approve-position-modal-body-div", "/RAMS/Agency/Position/ApprovePosition?positionId=" + $(e.relatedTarget).data("position-id") + "&positionTitle=" + encodeURIComponent($(e.relatedTarget).data("position-title"))); });

    $("#approve-position-modal").on("hidden.bs.modal", function (e) { $("#approve-position-modal-body-div").empty(); });

    $("#close-position-modal").on("show.bs.modal", function (e) { LoadAction("close-position-modal-body-div", "/RAMS/Agency/Position/ClosePosition?positionId=" + $(e.relatedTarget).data("position-id") + "&positionTitle=" + encodeURIComponent($(e.relatedTarget).data("position-title"))); });

    $("#close-position-modal").on("hidden.bs.modal", function (e) { $("#close-position-modal-body-div").empty(); });

    $("#assign-position-modal").on("show.bs.modal", function (e) { LoadAction("assign-position-modal-body-div", "/RAMS/Agency/Position/AssignPosition?positionId=" + $(e.relatedTarget).data("position-id") + "&agentId=" + $(e.relatedTarget).data("agent-id") + "&positionTitle=" + encodeURIComponent($(e.relatedTarget).data("position-title"))); });

    $("#assign-position-modal").on("hidden.bs.modal", function (e) { $("#assign-position-modal-body-div").empty(); });

    $("#agent-details-modal").on("show.bs.modal", function (e) { $("#agent-details-modal-title").text("Agent Details - " + $(e.relatedTarget).data("agent-name")); LoadAction("agent-details-modal-body-div", "/RAMS/Agency/Agent/AgentDetails?agentId=" + $(e.relatedTarget).data("agent-id")); });

    $("#agent-details-modal").on("hidden.bs.modal", function (e) { $("#agent-details-modal-body-div").empty(); });

    $("#client-details-modal").on("show.bs.modal", function (e) { $("#client-details-modal-title").text("Client Details - " + $(e.relatedTarget).data("client-name")); LoadAction("client-details-modal-body-div", "/RAMS/Agency/Client/ClientDetails?clientId=" + $(e.relatedTarget).data("client-id")); });

    $("#client-details-modal").on("hidden.bs.modal", function (e) { $("#client-details-modal-body-div").empty(); });

    $("#position-status-report-modal").on("show.bs.modal", function (e) { $("#position-status-report-modal-title").text("Status Report - " + $(e.relatedTarget).data("position-title")); LoadAction("position-status-report-modal-body-div", "/RAMS/Agency/Report/PositionStatusReport?positionId=" + $(e.relatedTarget).data("position-id")); });

    $("#position-status-report-modal").on("hidden.bs.modal", function (e) { $("#position-status-report-modal-body-div").empty(); });

    $("#position-final-report-modal").on("show.bs.modal", function (e) { $("#position-final-report-modal-title").text("Final Report - " + $(e.relatedTarget).data("position-title")); LoadAction("position-final-report-modal-body-div", "/RAMS/Agency/Report/PositionFinalReport?positionId=" + $(e.relatedTarget).data("position-id")); });

    $("#position-final-report-modal").on("hidden.bs.modal", function (e) { $("#position-final-report-modal-body-div").empty(); });

    $("#candidate-report-modal").on("show.bs.modal", function (e) { $("#candidate-report-modal-title").text("Candidate Report - " + $(e.relatedTarget).data("candidate-name")); LoadAction("candidate-report-modal-body-div", "/RAMS/Agency/Report/CandidateReport?candidateId=" + $(e.relatedTarget).data("candidate-id")); });

    $("#candidate-report-modal").on("hidden.bs.modal", function (e) { $("#candidate-report-modal-body-div").empty(); });

    $("#schedule-interview-modal").on("show.bs.modal", function (e) { LoadAction("schedule-interview-modal-body-div", "/RAMS/Agency/Position/ScheduleInterview?candidateId=" + $(e.relatedTarget).data("candidate-id") + "&displayDate=" + encodeURIComponent($(e.relatedTarget).data("date-time")) + "&selected=" + $(e.relatedTarget).data("selected")); });

    $("#schedule-interview-modal").on("hidden.bs.modal", function (e) { $("#schedule-interview-modal-body-div").empty(); });

    $("#personal-schedule-modal").on("show.bs.modal", function (e) { LoadAction("personal-schedule-modal-body-div", "/RAMS/Agency/Profile/PersonalSchedule?displayDate=" + encodeURIComponent($(e.relatedTarget).data("date-time"))); });

    $("#personal-schedule-modal").on("hidden.bs.modal", function (e) { $("#personal-schedule-modal-body-div").empty(); });
}

/********** END OF AGENCY MODAL CONTROLS **********/

/************** SYSTEM ADMIN FUNCTIONS **************/
function RefreshUserEditForm(userName, userType)
{
    LoadAction("edit-user-modal-body-div", "/RAMS/SystemAdmin/User/EditUser?userName=" + userName + "&userType=" + userType);
}


function DisableEditForm()
{
    $("#edit-user-modal-body-div :input").attr("Disabled", true);

    $("#unblock-user-button").attr("Disabled", false);
}

/********** END OF SYSTEM ADMIN FUNCTIONS ***********/

/************** AGENCY FUNCTIONS **************/
function RefreshPositionEditForm(positionId) {
    LoadAction("edit-position-modal-body-div", "/RAMS/Agency/Position/EditPosition?positionId=" + positionId);
}

function CheckTableRadioButton(obj)
{
    $(obj).find("td input:radio").prop("checked", true);
}

/********** END OF AGENCY FUNCTIONS ***********/


/****************** MODAL UTILITIES *****************/
// Fixes the issue with modal overflow in cases with multiple modals on one screen
$(document).on("show.bs.modal", ".modal", function ()
{
    var index = ($('.modal:visible').length * 10) + 1040;

    $(this).css('z-index', index);

    setTimeout(function () { $(".modal-backdrop").not(".modal-stack").css("z-index", index - 1).addClass("modal-stack"); }, 0);
});

// Fixes the issue with page scroll on pages with the modals
$(document).on("hidden.bs.modal", ".modal", function () { $(".modal:visible").length && $(document.body).addClass('modal-open'); });

/*************** END OF MODAL UTILITIES *************/