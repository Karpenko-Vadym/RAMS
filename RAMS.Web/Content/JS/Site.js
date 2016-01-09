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


function LoadDataTable(id)
{
    $("#" + id).DataTable();
}


function LoadAction(divName, actionUrl)
{
    $("#" + divName).load(actionUrl, function (response, status, xhr) { return status; });
}


function GetBaseUrl()
{
    urlComponents = location.href.split('/');

    return urlComponents[0] + "//" + urlComponents[2];
}

function ShowHidePassword()
{
    $("#showHidePassword").click(function () {
        if($(".password").attr("type") == "password")
        {
            $(".password").attr("type", "text");
        }
        else {
            $(".password").attr("type", "password");
        }
    });
}


/************* END OF GENERAL FUNCTIONS *************/

/*************** GENERAL MODAL CONTROLS *************/
function GeneralModalControls() {
    $("#edit-profile-modal").on("show.bs.modal", function (e) { LoadAction("edit-user-profile-modal-body-div", "/RAMS/Account/EditUserProfile"); });

    $("#change-password-modal").on("show.bs.modal", function (e) { LoadAction("password-change-div", "/RAMS/Account/ChangePassword?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type")); });

    $("#edit-profile-modal").on("hidden.bs.modal", function (e) { $("#edit-user-profile-modal-body-div").empty(); $("#edit-user-profile-message-modal-body-div").empty(); });
}
/*********** END OF GENERAL MODAL CONTROLS **********/

/************ SYSTEM ADMIN MODAL CONTROLS ***********/
function SystemAdminModalControls() {
    $("#new-user-modal").on("show.bs.modal", function (e) { LoadAction("new-user-modal-body-div", "/RAMS/SystemAdmin/User/NewUser"); });

    $("#new-user-modal").on("hidden.bs.modal", function (e) { $("#new-user-modal-body-div").empty(); });

    $("#edit-user-modal").on("show.bs.modal", function (e) { $("#edit-user-modal-title").text("Edit User - " + $(e.relatedTarget).data("user-name")); LoadAction("edit-user-modal-body-div", "/RAMS/SystemAdmin/User/EditUser?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type")); $("#edit-user-message-modal-body-div").empty(); });

    $("#edit-user-modal").on("hidden.bs.modal", function (e) { $("#edit-user-modal-title").text("Edit User"); $("#edit-user-modal-body-div").empty(); });



    $("#reset-password-modal").on("show.bs.modal", function (e) { LoadAction("password-reset-div", "/RAMS/SystemAdmin/User/ResetPassword?userName=" + $(e.relatedTarget).data("user-name") + "&userType=" + $(e.relatedTarget).data("user-type") + "&email=" + $(e.relatedTarget).data("email") + "&firstName=" + $(e.relatedTarget).data("firstName")); });

    $("#reset-password-modal").on("hidden.bs.modal", function (e) { $("#password-reset-div").empty(); });

    $("#block-delete-user-modal").on("show.bs.modal", function (e) {
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
}
/******** END OF SYSTEM ADMIN MODAL CONTROLS ********/

/************** SYSTEM ADMIN FUNCTIONS **************/
function RefreshEditForm(userName, userType) {
    LoadAction("edit-user-modal-body-div", "/RAMS/SystemAdmin/User/EditUser?userName=" + userName + "&userType=" + userType);
}


function DisableEditForm()
{
    $("#edit-user-modal-body-div :input").attr("Disabled", true);

    $("#unblock-user-button").attr("Disabled", false);
}

/********** END OF SYSTEM ADMIN FUNCTIONS ***********/


/****************** MODAL UTILITIES *****************/

$(document).on("show.bs.modal", ".modal", function () {
    var index = ($('.modal:visible').length * 10) + 1040;

    $(this).css('z-index', index);

    setTimeout(function () { $(".modal-backdrop").not(".modal-stack").css("z-index", index - 1).addClass("modal-stack"); }, 0);
});


$(document).on("hidden.bs.modal", ".modal", function () { $(".modal:visible").length && $(document.body).addClass('modal-open'); });

/*************** END OF MODAL UTILITIES *************/