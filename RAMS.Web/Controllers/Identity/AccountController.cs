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
    // TODO - Uncomment [Authorize]
    // [Authorize] 
    public class AccountController : BaseController
    {
        private ApplicationSignInManager SignInManager;

        private ApplicationUserManager UserManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            this.UserManager = userManager;

            this.SignInManager = signInManager;
        }

        public ApplicationSignInManager GetSignInManager
        {
            get { return this.SignInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }

            private set { this.SignInManager = value; }
        }

        public ApplicationUserManager GetUserManager
        {
            get { return this.UserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }

            private set { this.UserManager = value; }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await this.GetSignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:

                    return View("Lockout");

                case SignInStatus.RequiresVerification:

                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:

                default:

                    ModelState.AddModelError("", "Invalid login attempt.");

                    return View(model);
            }
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public async Task<PartialViewResult> EditUserProfile()
        {
            var identity = User.Identity as ClaimsIdentity;

            var response = new HttpResponseMessage();

            var editUserProfileViewModel = new EditUserProfileViewModel();

            if (identity.HasClaim("UserType", "Agent"))
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Agent?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    editUserProfileViewModel = Mapper.Map<Agent, EditUserProfileViewModel>(await response.Content.ReadAsAsync<Agent>());


                    if (editUserProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    editUserProfileViewModel = Mapper.Map<Client, EditUserProfileViewModel>(await response.Content.ReadAsAsync<Client>());


                    if (editUserProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Admin?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    editUserProfileViewModel = Mapper.Map<Admin, EditUserProfileViewModel>(await response.Content.ReadAsAsync<Admin>());


                    if (editUserProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }

            return PartialView("_EditUserProfile", editUserProfileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditUserProfile(EditUserProfileViewModel model)
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

                            Mapper.Map<EditUserProfileViewModel, Agent>(model, agent);

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

                                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                                    return PartialView("_Success", confirmationViewModel);
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

                            Mapper.Map<EditUserProfileViewModel, Client>(model, client);

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

                                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                                    return PartialView("_Success", confirmationViewModel);
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

                            Mapper.Map<EditUserProfileViewModel, Admin>(model, admin);

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

                                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                                    return PartialView("_Success", confirmationViewModel);
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

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
                catch (ClaimsAssignmentException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Claim could NOT be assigned to the user.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to assign a user claim. Please review an exception log for more details about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
                catch (EmployeeUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Employee could NOT be updated.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update an employee profile. Please review an exception log for more details about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            return PartialView("_EditUserProfile", model);
        }

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