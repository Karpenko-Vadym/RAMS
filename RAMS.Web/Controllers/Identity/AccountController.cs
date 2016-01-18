using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RAMS.ViewModels;
using RAMS.Web.Identity;
using System.Net.Http;
using AutoMapper;
using RAMS.Models;
using RAMS.Helpers;
using System.Text;
using RAMS.Enums;

namespace RAMS.Web.Controllers
{
    /// <summary>
    /// AccountController controller implements login and registration related action methods
    /// </summary>
    // [Authorize]
    // TODO - Uncomment [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager SignInManager;

        private ApplicationUserManager UserManager;

        /// <summary>
        /// Default AccountController constructor
        /// </summary>
        public AccountController() { }

        /// <summary>
        /// AccountController constructor that sets UserManager and SignInManager
        /// </summary>
        /// <param name="userManager">UserManager setter</param>
        /// <param name="signInManager">SignInManager setter</param>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            this.UserManager = userManager;

            this.SignInManager = signInManager;
        }

        /// <summary>
        /// PasswordRegex getter 
        /// </summary>
        public string PasswordRegex
        {
            get { return "^(?=.*[a-z])(?=.*[A-Z])(?=.*[1234567890])(?=.*[!@#$%^&*()_+=-]).+$"; }
        }

        /// <summary>
        /// Getter and setter for SignInManager property
        /// </summary>
        public ApplicationSignInManager GetSignInManager
        {
            get { return this.SignInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }

            private set { this.SignInManager = value; }
        }

        /// <summary>
        /// Getter and setter for UserManager property
        /// </summary>
        public ApplicationUserManager GetUserManager
        {
            get { return this.UserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }

            private set { this.UserManager = value; }
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>Login form</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        /// <summary>
        /// Login method authenticates the user if provided credentials are valid, otherwise re-displays login form with an error message
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <param name="returnUrl">URL of a page from where user was redirected to login form (In cases when cookie expires)</param>
        /// <returns>Login form if there has been an error while logging in, otherwise redirects to returnUrl or Home view if returnUrl is null</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.GetSignInManager.PasswordSignInAsync(model.UserName, model.Password, false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(returnUrl);

                case SignInStatus.Failure:

                default:

                    ModelState.AddModelError("", "Invalid login attempt.");

                    return View(model);
            }
        }

        /// <summary>
        /// LogOff method signs out the user and redirects to home view
        /// </summary>
        /// <returns>Home</returns>
        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        #region Edit User Profile
        /// <summary>
        /// EditUserProfile method gets user details and returns _EditUserProfile partial view with the model containing user details
        /// </summary>
        /// <returns>_EditUserProfile partial view with the model containing user details</returns>
        [HttpGet]
        public async Task<PartialViewResult> EditUserProfile()
        {
            var identity = User.Identity as ClaimsIdentity; // Get current user

            var response = new HttpResponseMessage();

            var UserEditProfileViewModel = new UserEditProfileViewModel();

            if (identity.HasClaim("UserType", "Agent")) // Current user is Agent
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Agent?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    UserEditProfileViewModel = Mapper.Map<Agent, UserEditProfileViewModel>(await response.Content.ReadAsAsync<Agent>());


                    if (UserEditProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }
            else if (identity.HasClaim("UserType", "Client")) // Current user is Client
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    UserEditProfileViewModel = Mapper.Map<Client, UserEditProfileViewModel>(await response.Content.ReadAsAsync<Client>());


                    if (UserEditProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }
            else if (identity.HasClaim("UserType", "Admin")) // Current user is Admin
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Admin?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    UserEditProfileViewModel = Mapper.Map<Admin, UserEditProfileViewModel>(await response.Content.ReadAsAsync<Admin>());


                    if (UserEditProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }

            return PartialView("_EditUserProfile", UserEditProfileViewModel);
        }

        /// <summary>
        /// EditUserProfile method persists updated user details and returns success message if update was successful, error message otherwise
        /// </summary>
        /// <param name="model">ViewModel with updated user details</param>
        /// <returns>Success message if update was successful (_Success partial view), error message otherwise (_Error partial view)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditUserProfile(UserEditProfileViewModel model)
        {
            var response = new HttpResponseMessage();

            if (!String.IsNullOrEmpty(model.Email))
            {
                if (model.CurrentEmail != model.Email)
                {
                    // If email is taken, add model error with following error message "The Email is unavalilable."
                    if (this.GetUserManager.FindByEmail(model.Email) != null)
                    {
                        ModelState.AddModelError("Email", "The Email is unavalilable.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Attempt to update an email and a user name
                    var user = await this.GetUserManager.FindByNameAsync(model.UserName);

                    user.Email = model.Email;

                    var result = await this.GetUserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // If user name and email successfully updated, attempt to update FullName user claim
                        result = await this.GetUserManager.RemoveClaimAsync(user.Id, new Claim("FullName", model.CurrentFullName));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("FullName claim could not be removed from user {0}.", model.UserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("FullName", model.FirstName + " " + model.LastName));

                        if (!result.Succeeded)
                        {

                            var message = "FullName claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        if(model.UserType == UserType.Agent)
                        {
                            // If FullName user claim successfully updated, attempt to update employee profile
                            var agent = new Agent();

                            response = await this.GetHttpClient().GetAsync(String.Format("Agent?userName={0}", model.UserName));

                            if (response.IsSuccessStatusCode) // Ensure that data that is not being changed, remain in the database
                            {
                                agent = await response.Content.ReadAsAsync<Agent>();
                            }

                            Mapper.Map<UserEditProfileViewModel, Agent>(model, agent);

                            response = await this.GetHttpClient().PutAsJsonAsync("Agent", agent);

                            if (response.IsSuccessStatusCode)
                            {
                                agent = await response.Content.ReadAsAsync<Agent>();

                                if (agent != null)
                                {
                                    var stringBuilder = new StringBuilder();

                                    stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully updated!</strong></h4></div>", agent.UserName);

                                    stringBuilder.AppendFormat("<div class='row'><div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.FirstName);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.LastName);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.JobTitle);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Company);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Email);

                                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'>Please remember NOT to share your login credentials with anyone.</div></div>");

                                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, false, true);

                                    return PartialView("_Success", userConfirmationViewModel);
                                }
                                else
                                {
                                    throw new EmployeeUpdateException("Null is returned after updating an employee. Status Code: " + response.StatusCode);
                                }
                            }
                            else
                            {
                                throw new EmployeeUpdateException("Employee profile could not be updated. Status Code: " + response.StatusCode);
                            }
                        }
                        else if (model.UserType == UserType.Client)
                        {
                            // If FullName user claim successfully updated, attempt to update employee profile
                            var client = new Client();

                            response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", model.UserName));

                            if (response.IsSuccessStatusCode) // Ensure that data that is not being changed, remain in the database
                            {
                                client = await response.Content.ReadAsAsync<Client>();
                            }

                            Mapper.Map<UserEditProfileViewModel, Client>(model, client);

                            response = await this.GetHttpClient().PutAsJsonAsync("Client", client);

                            if (response.IsSuccessStatusCode)
                            {
                                client = await response.Content.ReadAsAsync<Client>();

                                if (client != null)
                                {
                                    var stringBuilder = new StringBuilder();

                                    stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully updated!</strong></h4></div>", client.UserName);

                                    stringBuilder.AppendFormat("<div class='row'><div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.FirstName);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.LastName);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", client.JobTitle);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.Company);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", client.Email);

                                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'>Please remember NOT to share your login credentials with anyone.</div></div>");

                                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, false, true);

                                    return PartialView("_Success", userConfirmationViewModel);
                                }
                                else
                                {
                                    throw new EmployeeUpdateException("Null is returned after updating an employee. Status Code: " + response.StatusCode);
                                }
                            }
                            else
                            {
                                throw new EmployeeUpdateException("Employee profile could not be updated. Status Code: " + response.StatusCode);
                            }
                        }
                        else if(model.UserType == UserType.Admin)
                        {
                            // If FullName user claim successfully updated, attempt to update employee profile
                            var admin = new Admin();

                            response = await this.GetHttpClient().GetAsync(String.Format("Admin?userName={0}", model.UserName));

                            if (response.IsSuccessStatusCode) // Ensure that data that is not being changed, remain in the database
                            {
                                admin = await response.Content.ReadAsAsync<Admin>();
                            }

                            Mapper.Map<UserEditProfileViewModel, Admin>(model, admin);

                            response = await this.GetHttpClient().PutAsJsonAsync("Admin", admin);

                            if (response.IsSuccessStatusCode)
                            {
                                admin = await response.Content.ReadAsAsync<Admin>();

                                if (admin != null)
                                {
                                    var stringBuilder = new StringBuilder();

                                    stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully updated!</strong></h4></div>", admin.UserName);

                                    stringBuilder.AppendFormat("<div class='row'><div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.FirstName);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.LastName);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.JobTitle);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Company);

                                    stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Email);

                                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'>Please remember NOT to share your login credentials with anyone.</div></div>");

                                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, false, true);

                                    return PartialView("_Success", userConfirmationViewModel);
                                }
                                else
                                {
                                    throw new EmployeeUpdateException("Null is returned after updating an employee. Status Code: " + response.StatusCode);
                                }
                            }
                            else
                            {
                                throw new EmployeeUpdateException("Employee profile could not be updated. Status Code: " + response.StatusCode);
                            }
                        }
                    }
                    else
                    {
                        var message = "User could not be updated.";

                        if (!Utilities.IsEmpty(result.Errors))
                        {
                            foreach (var error in result.Errors)
                            {
                                message += " " + error;
                            }
                        }

                        throw new UserUpdateException(message);
                    }
                }
                catch (UserUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be updated.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update a user profile. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel);
                }
                catch (ClaimsAssignmentException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Claim could NOT be assigned to the user.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to assign a user claim. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel);
                }
                catch (EmployeeUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Employee could NOT be updated.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update an employee profile. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel);
                }
            }

            return PartialView("_EditUserProfile", model);
        }
        #endregion

        #region Change Password
        /// <summary>
        /// ChangePassword method populates the view model with properties received from _EditUserProfile partial view and passes it to _ChangePassword partial view
        /// </summary>
        /// <param name="userName">Setter for UserName property</param>
        /// <param name="userType">Setter for UserType property</param>
        /// <returns>_ChangePassword partial view with populated view model</returns>
        [HttpGet]
        public PartialViewResult ChangePassword(string userName, string userType)
        {
            var changePasswordViewModel = new ChangePasswordViewModel(userName, userType);

            return PartialView("_ChangePassword", changePasswordViewModel);
        }

        /// <summary>
        /// ChangePassword method validates current and new password and displays an error message if new password does not match password criteria or old password does not match database records, otherwise replaces old password with a new password
        /// </summary>
        /// <param name="model">User information required to reset password</param>
        /// <returns>_Success partial view with success message, or _Error partial view with error message depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> ChangePassword(ChangePasswordViewModel model)
        {
            var stringBuilder = new StringBuilder();

            var userConfirmationViewModel = new UserConfirmationViewModel();

            var identity = new ApplicationUser();

            // If any of password fields is empty, display _Error partial view with following error message "Not all required fields were entered."
            if (String.IsNullOrEmpty(model.CurrentPassword) || String.IsNullOrEmpty(model.Password) || String.IsNullOrEmpty(model.ConfirmPassword))
            { 
                stringBuilder.Clear();

                stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be changed.</strong></h4></div>");

                stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Not all required fields were entered.</div>");

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Please ensure that all required fields are entered.</div></div>");

                userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                return PartialView("_Confirmation", userConfirmationViewModel);
                
            }

            if (!String.IsNullOrEmpty(model.CurrentPassword))
            {
                // If current password does not match database records, display _Error partial view with following error message "Current Password does NOT match our records."
                if ((identity = this.GetUserManager.Find(model.UserName, model.CurrentPassword)) == null)
                {
                    stringBuilder.Clear();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be changed.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Current Password does NOT match our records.</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Please try again using valid password.</div></div>");

                    userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Confirmation", userConfirmationViewModel);
                }
            }

            if (!String.IsNullOrEmpty(model.Password))
            {
                // If password has less than 6 characters, display _Error partial view with following error message "Passwords must be at least 6 character long."
                if (model.Password.Length < 6)
                {
                    stringBuilder.Clear();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be changed.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Passwords must be at least 6 character long.</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Please try again using valid password pattern.</div></div>");

                    userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Confirmation", userConfirmationViewModel);
                }
                // If password is invalid format, display _Error partial view with following error message "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z')."
                else if (!Utilities.RegexMatch(this.PasswordRegex, model.Password))
                {
                    stringBuilder.Clear();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be changed.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>New Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z').</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Please try again using valid password pattern.</div></div>");

                    userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Confirmation", userConfirmationViewModel);
                }
            }

            if (ModelState.IsValid)
            {
                if (identity != null)
                {
                    // Attempt to change password
                    try
                    {
                        var result = await this.GetUserManager.ChangePasswordAsync(identity.Id, model.CurrentPassword, model.Password);

                        if (!result.Succeeded)
                        {
                            var message = String.Format("Password could not be changed from user {0}.", identity.UserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new PasswordChangeException(message);
                        }                     

                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>Success!</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>User password has been successfully changed.</div>");

                        stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Please remember NOT to share your login credentials with anyone.</div></div>");

                        userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                        return PartialView("_Confirmation", userConfirmationViewModel);
                    }
                    catch (PasswordChangeException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be changed.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to change a user password. Please review an exception log for more details about the exception.</div></div>");

                        userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                        return PartialView("_Confirmation", userConfirmationViewModel);
                    }
                }
            }

            stringBuilder.Clear();

            stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be changed.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>ModelState is not valid for current instance of the request. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

            userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

            return PartialView("_Confirmation", userConfirmationViewModel);
        }
        #endregion

        #region Forgot Password
        /// <summary>
        /// ForgotPassword method returns _ForgotPassword partial view
        /// </summary>
        /// <returns>_ForgotPassword partial view</returns>
        [HttpGet]
        public PartialViewResult ForgotPassword()
        {
            return PartialView("_ForgotPassword");
        }

        /// <summary>
        /// ForgotPassword method checks if an email address and user name provided by user match to email address and user name stored in data context, and if it matches, an email will be sent to administration with request to change password. Otherwise form will be redisplayed with an error message
        /// </summary>
        /// <param name="model">Email address and user name provided by user</param>
        /// <returns>_Confirmation partial view with success message if outcome of this method is success, _ForgotPassword partial view with model errors otherwise</returns>
        [HttpPost]
        public async Task<PartialViewResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!String.IsNullOrEmpty(model.UserName) && !String.IsNullOrEmpty(model.Email))
            {
                var user = await this.GetUserManager.FindByNameAsync(model.UserName);

                if (user != null) // Check if user exists
                {
                    if (user.Email != model.Email) // Check if user name matches with email
                    {
                        ModelState.AddModelError("", "User name or email address does not match our records. Please try again.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User name or email address does not match our records. Please try again.");
                }
            }

            if(ModelState.IsValid)
            {
                try
                {
                    var template = HttpContext.Server.MapPath("~/App_Data/UserPasswordResetEmailTemplate.txt");

                    var message = System.IO.File.ReadAllText(template);

                    message = message.Replace("%username%", model.UserName).Replace("%email%", model.Email);

                    // TODO - Change "atomix0x@gmail.com" to the email address of admin group
                    Email.EmailService.SendEmail("atomix0x@gmail.com", "Request to reset user password.", message); // Send a request to reset user password to admin group email address
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                }
                catch (System.IO.IOException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                }

                var stringBuilder = new StringBuilder();

                stringBuilder.Append("<div class='text-center'><h4><strong>Success!</strong></h4></div>");

                stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Request to reset user password has been sent to the RAMS Administration.</div>");

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>You will receive your new login credentials by an email once request is complete.</div>");

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE: </strong> Please remember NOT to share your login credentials with anyone.</div></div>");

                var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                return PartialView("_ForgotPasswordConfirmation", userConfirmationViewModel);
            }

            return PartialView("_ForgotPassword", model);
        }
        #endregion

        // From Microsoft Developer Network at https://msdn.microsoft.com/en-us/library/dd492699(v=vs.118).aspx
        /// <summary>
        /// Dispose method releases unmanaged resources and optionally releases managed resources
        /// </summary>
        /// <param name="disposing">If disposing is set to true releases both managed and unmanaged resources, otherwise only releases unmanaged resources</param> 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.GetUserManager != null)
                {
                    this.GetUserManager.Dispose();

                    this.GetUserManager = null;
                }

                if (this.GetSignInManager != null)
                {
                    this.GetSignInManager.Dispose();

                    this.GetSignInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null) {}

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;

                RedirectUri = redirectUri;

                UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}