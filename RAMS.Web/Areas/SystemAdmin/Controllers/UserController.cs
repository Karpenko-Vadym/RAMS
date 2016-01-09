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
using AutoMapper;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using RAMS.Web.Identity;
using RAMS.Enums;
using RAMS.ViewModels;
using RAMS.Models;
using RAMS.Web.Controllers;
using RAMS.Helpers;

namespace RAMS.Web.Areas.SystemAdmin.Controllers
{
    /// <summary>
    /// UserController controller implements CRUD operations for application users and employees
    /// </summary>
    // [Authorize]
    // TODO - Uncomment [Authorize]
    // TODO - Replace my (atomix0x@gmail.com) email address with the email address of the user
    public class UserController : BaseController
    {
        private ApplicationSignInManager SignInManager;

        private ApplicationUserManager UserManager;

        /// <summary>
        /// Default UserController constructor
        /// </summary>
        public UserController() { }

        /// <summary>
        /// UserController constructor that sets GetUserManager and GetSignInManager
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            GetUserManager = userManager;

            GetSignInManager = signInManager;
        }

        /// <summary>
        /// PasswordRegex getter 
        /// </summary>
        public string PasswordRegex
        {
            get { return "^(?=.*[a-z])(?=.*[A-Z])(?=.*[1234567890])(?=.*[!@#$%^&*()_+=-]).+$"; }
        }

        /// <summary>
        /// ApplicationSignInManager getter and setter
        /// </summary>
        public ApplicationSignInManager GetSignInManager
        {
            get { return SignInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }

            private set { SignInManager = value; }
        }

        /// <summary>
        /// ApplicationUserManager getter and setter
        /// </summary>
        public ApplicationUserManager GetUserManager
        {
            get { return UserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }

            private set { UserManager = value; }
        }

        /// <summary>
        /// Default action method that returns main view of User controller
        /// </summary>
        /// <returns>Main view of User controller</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #region User List
        /// <summary>
        /// UserList action method displays all the employees in _UserList partial view
        /// </summary>
        /// <returns>_UserList partial view with all the employees</returns>
        [HttpGet]
        public async Task<PartialViewResult> UserList()
        {
            var users = new List<UserListViewModel>();

            var response = await this.GetHttpClient().GetAsync("Agent");

            if (response.IsSuccessStatusCode)
            {
                users.AddRange(Mapper.Map<List<Agent>, List<UserListViewModel>>(await response.Content.ReadAsAsync<List<Agent>>()));
            }

            response = await this.GetHttpClient().GetAsync("Client");

            if (response.IsSuccessStatusCode)
            {
                users.AddRange(Mapper.Map<List<Client>, List<UserListViewModel>>(await response.Content.ReadAsAsync<List<Client>>()));
            }

            response = await this.GetHttpClient().GetAsync("Admin");

            if (response.IsSuccessStatusCode)
            {
                users.AddRange(Mapper.Map<List<Admin>, List<UserListViewModel>>(await response.Content.ReadAsAsync<List<Admin>>()));
            }

            return PartialView("_UserList", users); // TODO - Do not display deleted users (logical) in the list
        }
        #endregion

        #region Registration
        /// <summary>
        /// NewUser method returns _NewUser partial view
        /// </summary>
        /// <returns>_NewUser partial view</returns>
        [HttpGet]
        public PartialViewResult NewUser()
        {
            return PartialView("_NewUser");
        }

        #region UserType Select
        /// <summary>
        /// UserTypeSelect method returns _UserTypeSelect partial view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult UserTypeSelect()
        {
            return PartialView("_UserTypeSelect");
        }

        /// <summary>
        /// UserTypeSelect method displays different registration form depending on user selection from _UserTypeSelect partial view
        /// </summary>
        /// <param name="model">Model that contains information about user selection</param>
        /// <returns>Different registration form depending on user selection from _UserTypeSelect partial view, or _Error partial view with error message if selection did not match to UserType enum values</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> UserTypeSelect(UserTypeViewModel model)
        {
            if (!String.IsNullOrEmpty(model.SelectedValue))
            {
                if (Enum.GetName(typeof(Enums.UserType), UserType.Agent) == model.SelectedValue) // Agent is selected
                {
                    var agentAddViewModel = new AgentAddViewModel();

                    var departments = new List<Department>();

                    HttpResponseMessage response = await this.GetHttpClient().GetAsync("Department");

                    if (response.IsSuccessStatusCode)
                    {
                        departments = await response.Content.ReadAsAsync<List<Department>>();
                    }

                    agentAddViewModel.Departments = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(departments.Select(d => new SelectListItem { Text = d.Name, Value = d.DepartmentId.ToString() }).ToList()).ToList();

                    return PartialView("_RegisterAgent", agentAddViewModel);
                }
                else if (Enum.GetName(typeof(Enums.UserType), UserType.Client) == model.SelectedValue) // Cleint is selected
                {
                    var clientAddViewModel = new ClientAddViewModel();

                    return PartialView("_RegisterClient", clientAddViewModel);
                }
                else if (Enum.GetName(typeof(Enums.UserType), UserType.Admin) == model.SelectedValue) // Admin is selected
                {
                    var adminAddViewModel = new AdminAddViewModel();

                    return PartialView("_RegisterAdmin", adminAddViewModel);
                }
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class='text-center'><h4><strong>User type has NOT been selected.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Please select the user type from the dropdown and click on 'Select' button in order to create new user.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Different input form will be displayed depending on user type selection.</div></div>");

            var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

            return PartialView("_Error", userConfirmationViewModel);
        }
        #endregion

        #region Agent Registration
        /// <summary>
        /// RegisterAgent method creates new user and new employee 
        /// </summary>
        /// <param name="model">User and employee details required in order to create new user and employee</param>
        /// <returns>_Success partial view with success message if user and employee were successfully created, _Error partial view with error message otherwise. If mode is not valid, returns _RegisterAgent partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> RegisterAgent(AgentAddViewModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            // If department id is 0, add model error with following error message "The Department field is required."
            if(model.DepartmentId == 0)
            {
                ModelState.AddModelError("DepartmentId", "The Department field is required.");
            }

            // If user name is taken, add model error with following error message "The User Name is unavalilable."
            if (!String.IsNullOrEmpty(model.UserName))
            {
                if (this.GetUserManager.FindByName(model.UserName) != null)
                {
                    ModelState.AddModelError("UserName", "The User Name is unavalilable.");
                }
            }

            if (!String.IsNullOrEmpty(model.Email))
            {
                // If email is taken, add model error with following error message "The Email is unavalilable."
                if (this.GetUserManager.FindByEmail(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "The Email is unavalilable.");
                }
            }

            if (!String.IsNullOrEmpty(model.Password))
            {
                // If password is invalid format, add model error with following error message "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z')."
                if (!Utilities.RegexMatch(this.PasswordRegex, model.Password))
                {
                    ModelState.AddModelError("Password", "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z').");
                }
            }

            if (ModelState.IsValid)
            {
                var success = true;

                // If model is valid (All mandatory fields are entered), attempt to create a new user with information from the model
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = true, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true };

                try
                {  
                    var result = await this.GetUserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // If IdentityResult (Create new user) is Succeded, attempt to add FullName claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("FullName", model.FirstName + " " + model.LastName));

                        if (!result.Succeeded)
                        {
                            // If adding FullName claim failed, throw ClaimsAssignmentException with detailed error message
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

                        // If IdentityResult (Assign FullName claim) is Succeded, attempt to add Role claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("Role", model.SelectedRole));

                        if(!result.Succeeded)
                        {
                            // If adding Role claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "Role claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign Role claim) is Succeded, attempt to add UserType claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("UserType", model.UserType.ToString()));

                        if (!result.Succeeded)
                        {
                            // If adding UserType claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "UserType claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign UserType claim) is Succeded, attempt to add UserStatus claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("UserStatus", model.UserStatus.ToString()));

                        if (!result.Succeeded)
                        {
                            // If adding UserStatus claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "UserStatus claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign UserType claim) is Succeded, attempt to create new agent
                        var agent = Mapper.Map<AgentAddViewModel, Agent>(model);

                        response = await this.GetHttpClient().PostAsJsonAsync("Agent", agent);

                        if (response.IsSuccessStatusCode)
                        {
                            agent = await response.Content.ReadAsAsync<Agent>();

                            try
                            {
                                var template = HttpContext.Server.MapPath("~/App_Data/UserRegistrationEmailTemplate.txt");

                                var message = System.IO.File.ReadAllText(template);  
                   
                                message = message.Replace("%name%", agent.FirstName).Replace("%username%", agent.UserName).Replace("%password%", model.Password);

                                Email.EmailService.SendEmail("atomix0x@gmail.com", "Your account has been created.", message); // Send login credentials to newly created user via email
                            }
                            catch(System.IO.FileNotFoundException ex)
                            {
                                // Log exception
                                ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                            }
                            catch(System.IO.IOException ex)
                            {
                                // Log exception
                                ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                            }

                            var stringBuilder = new StringBuilder();

                            stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully created!</strong></h4></div>", agent.UserName);

                            stringBuilder.Append("<div class='row'><div class='col-md-offset-1'>Please review user details and ensure that all the information is valid.</div>");

                            stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-2 col-md-3'>User Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.UserName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>User Type: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.UserType);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.FirstName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.LastName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.JobTitle);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Company);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Department Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Department.Name);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Email);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Phone Number: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.PhoneNumber);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Role: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Role.ToString());

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Agent Status: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.AgentStatus.ToString());

                            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> Login credentials have been sent to the user via an email.</div></div>");

                            var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, true);

                            return PartialView("_Success", userConfirmationViewModel);
                        }
                        else
                        {
                            // If agent could not be created, throw EmployeeRegistrationException with detailed error message
                            throw new EmployeeRegistrationException("Agent " + agent.UserName + " could not be created. Response: " + response.StatusCode);
                        }
                    }
                    else
                    {
                        // If user could not be create, throw UserRegistrationException with detailed error message
                        var message = "User " + user.UserName + " could not be created.";

                        if (!Utilities.IsEmpty(result.Errors))
                        {
                            foreach (var error in result.Errors)
                            {
                                message += " " + error;
                            }
                        }

                        throw new UserRegistrationException(message);
                    }
                }
                catch (UserRegistrationException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be registered.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to register a user. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel);  
                }
                catch (ClaimsAssignmentException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    success = false;

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Claim could NOT be assigned to the user.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to assign a user claim. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel); 
                }
                catch (EmployeeRegistrationException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    success = false;

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Employee could NOT be registered.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to register an employee. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel);  
                }
                finally
                {
                    if (success == false)
                    {
                        // If any of operations failed, delete user and agent from database
                        var result = this.GetUserManager.Delete(user);

                        if (!result.Succeeded)
                        {
                            // If user could not be deleted, log UserDeleteException with detailed error message
                            var message = "User " + user.UserName + " could not be deleted.";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            // Log exception
                            ErrorHandlingUtilities.LogException(new UserDeleteException(message).ToString(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
                        }

                        response = this.GetHttpClient().DeleteAsync("Agent?userName=" + model.UserName).Result;

                        // If agent could not be deleted, log EmployeeDeleteException with detailed error message
                        if (!response.IsSuccessStatusCode)
                        {
                            // Log exception
                            ErrorHandlingUtilities.LogException(new EmployeeDeleteException("Agent " + model.UserName + " could not be deleted. Response: " + response.StatusCode).ToString(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
                        }
                    }
                }
            }

            var departments = new List<Department>();

            response = await this.GetHttpClient().GetAsync("Department");

            if (response.IsSuccessStatusCode)
            {
                departments = await response.Content.ReadAsAsync<List<Department>>();
            }

            model.Departments = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(departments.Select(d => new SelectListItem { Text = d.Name, Value = d.DepartmentId.ToString(), Selected = ( d.DepartmentId == model.DepartmentId ) }).ToList()).ToList();

            return PartialView("_RegisterAgent", model);
        }
        #endregion

        #region Client Registration
        /// <summary>
        /// RegisterClient method creates new user and new employee
        /// </summary>
        /// <param name="model">User and employee details required in order to create new user and employee</param>
        /// <returns>_Success partial view with success message if user and employee were successfully created, _Error partial view with error message otherwise. If mode is not valid, returns _RegisterClient partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> RegisterClient(ClientAddViewModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            // If user name is taken, add model error with following error message "The User Name is unavalilable."
            if (!String.IsNullOrEmpty(model.UserName))
            {
                if (this.GetUserManager.FindByName(model.UserName) != null)
                {
                    ModelState.AddModelError("UserName", "The User Name is unavalilable.");
                }
            }

            // If email is taken, add model error with following error message "The Email is unavalilable."
            if (!String.IsNullOrEmpty(model.Email))
            {
                if (this.GetUserManager.FindByEmail(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "The Email is unavalilable.");
                }
            }

            if (!String.IsNullOrEmpty(model.Password))
            {
                // If password is invalid format, add model error with following error message "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z')."
                if (!Utilities.RegexMatch(this.PasswordRegex, model.Password))
                {
                    ModelState.AddModelError("Password", "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z').");
                }
            }

            if (ModelState.IsValid)
            {
                var success = true;

                // If model is valid (All mandatory fields are entered), attempt to create a new user with information from the model
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = true, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true };

                try
                {
                    var result = await this.GetUserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // If IdentityResult (Create new user) is Succeded, attempt to add FullName claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("FullName", model.FirstName + " " + model.LastName));

                        if (!result.Succeeded)
                        {
                            // If adding FullName claim failed, throw ClaimsAssignmentException with detailed error message
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

                        // If IdentityResult (Assign FullName claim) is Succeded, attempt to add Role claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("Role", model.Role.ToString()));

                        if (!result.Succeeded)
                        {
                            // If adding Role claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "Role claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign Role claim) is Succeded, attempt to add UserType claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("UserType", model.UserType.ToString()));

                        if (!result.Succeeded)
                        {
                            // If adding UserType claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "UserType claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign UserType claim) is Succeded, attempt to add UserStatus claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("UserStatus", model.UserStatus.ToString()));

                        if (!result.Succeeded)
                        {
                            // If adding UserStatus claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "UserStatus claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }


                        // If IdentityResult (Assign UserType claim) is Succeded, attempt to create new client
                        var client = Mapper.Map<ClientAddViewModel, Client>(model);

                        response = await this.GetHttpClient().PostAsJsonAsync("Client", client);

                        if (response.IsSuccessStatusCode)
                        {
                            client = await response.Content.ReadAsAsync<Client>();

                            try
                            {
                                var template = HttpContext.Server.MapPath("~/App_Data/UserRegistrationEmailTemplate.txt");

                                var message = System.IO.File.ReadAllText(template);

                                message = message.Replace("%name%", client.FirstName).Replace("%username%", client.UserName).Replace("%password%", model.Password);

                                Email.EmailService.SendEmail("atomix0x@gmail.com", "Your account has been created.", message); // Send login credentials to newly created user via email
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

                            stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully created!</strong></h4></div>", client.UserName);

                            stringBuilder.Append("<div class='row'><div class='col-md-offset-1'>Please review user details and ensure that all the information is valid.</div>");

                            stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-2 col-md-3'>User Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.UserName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>User Type: </div><div class='col-md-7'><strong>{0}</strong></div>", client.UserType);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.FirstName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.LastName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", client.JobTitle);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.Company);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", client.Email);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Phone Number: </div><div class='col-md-7'><strong>{0}</strong></div>", client.PhoneNumber);

                            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> Login credentials have been sent to the user via an email.</div></div>");

                            var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, true);

                            return PartialView("_Success", userConfirmationViewModel);
                        }
                        else
                        {
                            // If client could not be created, throw EmployeeRegistrationException with detailed error message
                            throw new EmployeeRegistrationException("Client " + client.UserName + " could not be created. Response: " + response.StatusCode);
                        }
                    }
                    else
                    {
                        // If user could not be create, throw UserRegistrationException with detailed error message
                        var message = "User " + user.UserName + " could not be created.";

                        if (!Utilities.IsEmpty(result.Errors))
                        {
                            foreach (var error in result.Errors)
                            {
                                message += " " + error;
                            }
                        }

                        throw new UserRegistrationException(message);
                    }
                }
                catch (UserRegistrationException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be registered.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to register a user. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel);  
                }
                catch (ClaimsAssignmentException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    success = false;

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Claim could NOT be assigned to the user.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to assign a user claim. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel); 
                }
                catch (EmployeeRegistrationException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    success = false;

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Employee could NOT be registered.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to register an employee. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel); 
                }
                finally
                {
                    if (success == false)
                    {
                        // If any of operations failed, delete user and client from database
                        var result = this.GetUserManager.Delete(user);

                        if (!result.Succeeded)
                        {
                            // If user could not be deleted, log UserDeleteException with detailed error message
                            var message = "User " + user.UserName + " could not be deleted.";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            // Log exception
                            ErrorHandlingUtilities.LogException(new UserDeleteException(message).ToString(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
                        }

                        response = this.GetHttpClient().DeleteAsync("Client?userName=" + model.UserName).Result;

                        // If client could not be deleted, log EmployeeDeleteException with detailed error message
                        if (!response.IsSuccessStatusCode)
                        {
                            // Log exception
                            ErrorHandlingUtilities.LogException(new EmployeeDeleteException("Client " + model.UserName + " could not be deleted. Response: " + response.StatusCode).ToString(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
                        }
                    }
                }
            }

            return PartialView("_RegisterClient", model);
        }
        #endregion
        
        #region Admin Registration
        /// <summary>
        /// RegisterAdmin method creates new user and new employee
        /// </summary>
        /// <param name="model">User and employee details required in order to create new user and employee</param>
        /// <returns>_Success partial view with success message if user and employee were successfully created, _Error partial view with error message otherwise. If mode is not valid, returns _RegisterAdmin partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> RegisterAdmin(AdminAddViewModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            // If user name is taken, add model error with following error message "The User Name is unavalilable."
            if (!String.IsNullOrEmpty(model.UserName))
            {
                if (this.GetUserManager.FindByName(model.UserName) != null)
                {
                    ModelState.AddModelError("UserName", "The User Name is unavalilable.");
                }
            }

            if (!String.IsNullOrEmpty(model.Email))
            {
                // If email is taken, add model error with following error message "The Email is unavalilable."
                if (this.GetUserManager.FindByEmail(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "The Email is unavalilable.");
                }
            }

            if (!String.IsNullOrEmpty(model.Password))
            {
                // If password is invalid format, add model error with following error message "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z')."
                if (!Utilities.RegexMatch(this.PasswordRegex, model.Password))
                {
                    ModelState.AddModelError("Password", "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z').");
                }
            }

            if (ModelState.IsValid)
            {
                var success = true;

                // If model is valid (All mandatory fields are entered), attempt to create a new user with information from the model
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = true, PhoneNumber = model.PhoneNumber, PhoneNumberConfirmed = true };

                try
                {
                    var result = await this.GetUserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // If IdentityResult (Create new user) is Succeded, attempt to add FullName claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("FullName", model.FirstName + " " + model.LastName));

                        if (!result.Succeeded)
                        {
                            // If adding FullName claim failed, throw ClaimsAssignmentException with detailed error message
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

                        // If IdentityResult (Assign FullName claim) is Succeded, attempt to add Role claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("Role", model.SelectedRole));

                        if (!result.Succeeded)
                        {
                            // If adding Role claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "Role claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign Role claim) is Succeded, attempt to add UserType claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("UserType", model.UserType.ToString()));

                        if (!result.Succeeded)
                        {
                            // If adding UserType claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "UserType claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign UserType claim) is Succeded, attempt to add UserStatus claim
                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("UserStatus", model.UserStatus.ToString()));

                        if (!result.Succeeded)
                        {
                            // If adding UserStatus claim failed, throw ClaimsAssignmentException with detailed error message
                            var message = "UserStatus claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If IdentityResult (Assign UserType claim) is Succeded, attempt to create new admin
                        var admin = Mapper.Map<AdminAddViewModel, Admin>(model);

                        response = await this.GetHttpClient().PostAsJsonAsync("Admin", admin);

                        if (response.IsSuccessStatusCode)
                        {
                            admin = await response.Content.ReadAsAsync<Admin>();

                            try
                            {
                                var template = HttpContext.Server.MapPath("~/App_Data/UserRegistrationEmailTemplate.txt");

                                var message = System.IO.File.ReadAllText(template);

                                message = message.Replace("%name%", admin.FirstName).Replace("%username%", admin.UserName).Replace("%password%", model.Password);

                                Email.EmailService.SendEmail("atomix0x@gmail.com", "Your account has been created.", message); // Send login credentials to newly created user via email
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

                            stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully created!</strong></h4></div>", admin.UserName);

                            stringBuilder.Append("<div class='row'><div class='col-md-offset-1'>Please review user details and ensure that all the information is valid.</div>");

                            stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-2 col-md-3'>User Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.UserName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>User Type: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.UserType);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.FirstName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.LastName);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.JobTitle);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Company);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Email);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Phone Number: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.PhoneNumber);

                            stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Role: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Role.ToString());

                            stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> Login credentials have been sent to the user via an email.</div></div>");

                            var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, true);

                            return PartialView("_Success", userConfirmationViewModel);
                        }
                        else
                        {
                            // If admin could not be created, throw EmployeeRegistrationException with detailed error message
                            throw new EmployeeRegistrationException("Admin " + admin.UserName + " could not be created. Response: " + response.StatusCode);
                        }
                    }
                    else
                    {
                        // If user could not be create, throw UserRegistrationException with detailed error message
                        var message = "User " + user.UserName + " could not be created.";

                        if (!Utilities.IsEmpty(result.Errors))
                        {
                            foreach (var error in result.Errors)
                            {
                                message += " " + error;
                            }
                        }

                        throw new UserRegistrationException(message);
                    }
                }
                catch (UserRegistrationException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be registered.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to register a user. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel); 
                }
                catch (ClaimsAssignmentException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    success = false;

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Claim could NOT be assigned to the user.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to assign a user claim. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel); 
                }
                catch (EmployeeRegistrationException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    success = false;

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Employee could NOT be registered.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to register an employee. Please review an exception log for more details about the exception.</div></div>");

                    var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", userConfirmationViewModel); 
                }
                finally
                {
                    if (success == false)
                    {
                        // If any of operations failed, delete user and admin from database
                        var result = this.GetUserManager.Delete(user);

                        if (!result.Succeeded)
                        {
                            // If user could not be deleted, log UserDeleteException with detailed error message
                            var message = "User " + user.UserName + " could not be deleted.";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            // Log exception
                            ErrorHandlingUtilities.LogException(new UserDeleteException(message).ToString(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
                        }

                        response = this.GetHttpClient().DeleteAsync("Admin?userName=" + model.UserName).Result;

                        // If admin could not be deleted, log EmployeeDeleteException with detailed error message
                        if (!response.IsSuccessStatusCode)
                        {
                            // Log exception
                            ErrorHandlingUtilities.LogException(new EmployeeDeleteException("Admin " + model.UserName + " could not be deleted. Response: " + response.StatusCode).ToString(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
                        }
                    }
                }
            }

            return PartialView("_RegisterAdmin", model);
        }
        #endregion

        #endregion

        #region Edit Users
        /// <summary>
        /// EditUser method displays different edit form depending on user selection from _UserList partial view
        /// </summary>
        /// <param name="userName">User name of selected user</param>
        /// <param name="userType">User type of selected user</param>
        /// <returns>Different edit form depending on user selection from _UserList partial view, or _Error partial view with error message if userType parameter value did not match to UserType enum values</returns>
        [HttpGet]
        public async Task<PartialViewResult> EditUser(string userName, string userType)
        {
            var response = new HttpResponseMessage();

            if (Enum.GetName(typeof(Enums.UserType), UserType.Agent) == userType)
            {
                var agentEditViewModel = new AgentEditViewModel();

                // Retrieve an employee profile
                response = await this.GetHttpClient().GetAsync(String.Format("Agent?userName={0}", userName));

                if (response.IsSuccessStatusCode)
                {
                    var departments = new List<Department>();

                    agentEditViewModel = Mapper.Map<Agent, AgentEditViewModel>(await response.Content.ReadAsAsync<Agent>());

                    response = await this.GetHttpClient().GetAsync("Department");

                    if (response.IsSuccessStatusCode)
                    {
                        departments = await response.Content.ReadAsAsync<List<Department>>();
                    }

                    agentEditViewModel.Departments = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(departments.Select(d => new SelectListItem { Text = d.Name, Value = d.DepartmentId.ToString(), Selected = (d.DepartmentId == agentEditViewModel.DepartmentId) }).ToList()).ToList();

                    if (agentEditViewModel.UserStatus == UserStatus.Deleted)
                    {
                        return PartialView("_EditAgent");
                    }

                    return PartialView("_EditAgent", agentEditViewModel);

                }
            }
            else if (Enum.GetName(typeof(Enums.UserType), UserType.Client) == userType)
            {
                var clientEditViewModel = new ClientEditViewModel();

                // Retrieve an employee profile
                response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", userName));

                if (response.IsSuccessStatusCode)
                {
                    var client = await response.Content.ReadAsAsync<Client>();

                    clientEditViewModel = Mapper.Map<Client, ClientEditViewModel>(client);


                    if (clientEditViewModel.UserStatus == UserStatus.Deleted)
                    {
                        return PartialView("_EditClient");
                    }

                    return PartialView("_EditClient", clientEditViewModel);
                }
            }
            else if (Enum.GetName(typeof(Enums.UserType), UserType.Admin) == userType)
            {
                var adminEditViewModel = new AdminEditViewModel();

                // Retrieve an employee profile
                response = await this.GetHttpClient().GetAsync(String.Format("Admin?userName={0}", userName));

                if (response.IsSuccessStatusCode)
                {
                    adminEditViewModel = Mapper.Map<Admin, AdminEditViewModel>(await response.Content.ReadAsAsync<Admin>());


                    if (adminEditViewModel.UserStatus == UserStatus.Deleted)
                    {
                        return PartialView("_EditAdmin");
                    }

                    return PartialView("_EditAdmin", adminEditViewModel);
                }
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class='text-center'><h4><strong>User details are NOT available at this time.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>User could have been deleted from the system, or recently updated causing user name mismatch.</div>");

            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'>Please refresh the list and try again in a moment.</div></div>");

            var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, true);

            return PartialView("_Error", userConfirmationViewModel);
        }

        #region Edit Agent
        /// <summary>
        /// EditAgent method updates user and employee profiles
        /// </summary>
        /// <param name="model">Updated user and employee details</param>
        /// <returns>_Success partial view with success message if user and employee were successfully updated, _Error partial view with error message otherwise. If mode is not valid, returns _EditAgent partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditAgent(AgentEditViewModel model)
        {
            var response = new HttpResponseMessage();

            var departments = new List<Department>();

            // If department id is 0, add model error with following error message "The Department field is required."
            if (model.DepartmentId == 0)
            {
                ModelState.AddModelError("DepartmentId", "The Department field is required.");
            }

            // If user name is taken, add model error with following error message "The User Name is unavalilable."
            if (!String.IsNullOrEmpty(model.UserName))
            {
                if (model.CurrentUserName != model.UserName)
                {
                    if (this.GetUserManager.FindByName(model.UserName) != null)
                    {
                        ModelState.AddModelError("UserName", "The User Name is unavalilable.");
                    }
                }
            }

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

            response = await this.GetHttpClient().GetAsync("Department");

            if (response.IsSuccessStatusCode)
            {
                departments = await response.Content.ReadAsAsync<List<Department>>();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Attempt to update an email and a user name
                    var user = await this.GetUserManager.FindByNameAsync(model.CurrentUserName);

                    user.Email = model.Email;

                    user.UserName = model.UserName;

                    var result = await this.GetUserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // If user name and email successfully updated, attempt to update FullName user claim
                        result = await this.GetUserManager.RemoveClaimAsync(user.Id, new Claim("FullName", model.CurrentFullName));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("FullName claim could not be removed from user {0}.", model.CurrentUserName);

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

                        // If FullName user claim successfully updated, attempt to update Role user claim
                        result = await this.GetUserManager.RemoveClaimAsync(user.Id, new Claim("Role", model.CurrentRole));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("Role claim could not be removed from user {0}.", model.CurrentUserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("Role", model.SelectedRole));

                        if (!result.Succeeded)
                        {
                            var message = "Role claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If Role user claim successfully updated, attempt to update employee profile
                        var agent = new Agent();

                        response = await this.GetHttpClient().GetAsync(String.Format("Agent?userName={0}", model.CurrentUserName));

                        if (response.IsSuccessStatusCode) // Ensure that data that is not being changed, remain in the database
                        {
                            agent = await response.Content.ReadAsAsync<Agent>();
                        }

                        Mapper.Map<AgentEditViewModel, Agent>(model, agent);

                        response = await this.GetHttpClient().PutAsJsonAsync("Agent", agent);

                        if (response.IsSuccessStatusCode)
                        {
                            agent = await response.Content.ReadAsAsync<Agent>();

                            if (agent.UserStatus == UserStatus.Deleted)
                            {
                                return PartialView("_EditAgent");
                            }

                            if (agent != null)
                            {
                                // If employee profile was successfully updated, send an email notification to the user and display _Success partial view with success message
                                try
                                {
                                    var template = HttpContext.Server.MapPath("~/App_Data/UserUpdateEmailTemplate.txt");

                                    var message = System.IO.File.ReadAllText(template);

                                    message = message.Replace("%name%", agent.FirstName).Replace("%username%", agent.UserName);

                                    Email.EmailService.SendEmail("atomix0x@gmail.com", "Your user account has been successfully updated.", message); // Send notification about account changes to the user via email
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

                                stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully updated!</strong></h4></div>", agent.UserName);

                                stringBuilder.Append("<div class='row'><div class='col-md-offset-1'>Please review user details and ensure that all the information is valid.</div>");

                                stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-2 col-md-3'>User Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.UserName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>User Type: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.UserType);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.FirstName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.LastName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.JobTitle);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Company);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Department Name: </div><div class='col-md-7'><strong>{0}</strong></div>", departments.FirstOrDefault(d => d.DepartmentId == agent.DepartmentId).Name);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Email);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Phone Number: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.PhoneNumber);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Role: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.Role.ToString());

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Agent Status: </div><div class='col-md-7'><strong>{0}</strong></div>", agent.AgentStatus.ToString());

                                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> The user has been notified about the profile changes via an email.</div></div>");

                                var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, true, true);

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
                catch(UserUpdateException ex)
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
                catch(EmployeeUpdateException ex)
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

            model.Departments = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(departments.Select(d => new SelectListItem { Text = d.Name, Value = d.DepartmentId.ToString(), Selected = (d.DepartmentId == model.DepartmentId) }).ToList()).ToList();

            return PartialView("_EditAgent", model);
        }
        #endregion

        #region Edit Client
        /// <summary>
        /// EditClient method updates user and employee profiles
        /// </summary>
        /// <param name="model">Updated user and employee details</param>
        /// <returns>_Success partial view with success message if user and employee were successfully updated, _Error partial view with error message otherwise. If mode is not valid, returns _EditClient partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditClient(ClientEditViewModel model)
        {
            var response = new HttpResponseMessage();

            // If user name is taken, add model error with following error message "The User Name is unavalilable."
            if (!String.IsNullOrEmpty(model.UserName))
            {
                if (model.CurrentUserName != model.UserName)
                {
                    if (this.GetUserManager.FindByName(model.UserName) != null)
                    {
                        ModelState.AddModelError("UserName", "The User Name is unavalilable.");
                    }
                }
            }

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
                    var user = await this.GetUserManager.FindByNameAsync(model.CurrentUserName);

                    user.Email = model.Email;

                    user.UserName = model.UserName;

                    var result = await this.GetUserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // If user name and email successfully updated, attempt to update FullName user claim
                        result = await this.GetUserManager.RemoveClaimAsync(user.Id, new Claim("FullName", model.CurrentFullName));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("FullName claim could not be removed from user {0}.", model.CurrentUserName);

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

                        // If FullName user claim successfully updated, attempt to update employee profile
                        var client = new Client();

                        response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", model.CurrentUserName));

                        if (response.IsSuccessStatusCode) // Ensure that data that is not being changed, remain in the database
                        {
                            client = await response.Content.ReadAsAsync<Client>();
                        }

                        Mapper.Map<ClientEditViewModel, Client>(model, client);

                        response = await this.GetHttpClient().PutAsJsonAsync("Client", client);

                        if (response.IsSuccessStatusCode)
                        {
                            client = await response.Content.ReadAsAsync<Client>();

                            if (client.UserStatus == UserStatus.Deleted)
                            {
                                return PartialView("_EditClient");
                            }

                            if (client != null)
                            {
                                // If employee profile was successfully updated, send an email notification to the user and display _Success partial view with success message
                                try
                                {
                                    var template = HttpContext.Server.MapPath("~/App_Data/UserUpdateEmailTemplate.txt");

                                    var message = System.IO.File.ReadAllText(template);

                                    message = message.Replace("%name%", client.FirstName).Replace("%username%", client.UserName);

                                    Email.EmailService.SendEmail("atomix0x@gmail.com", "Your user account has been successfully updated.", message); // Send notification about account changes to the user via email
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

                                stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully updated!</strong></h4></div>", client.UserName);

                                stringBuilder.Append("<div class='row'><div class='col-md-offset-1'>Please review user details and ensure that all the information is valid.</div>");

                                stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-2 col-md-3'>User Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.UserName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>User Type: </div><div class='col-md-7'><strong>{0}</strong></div>", client.UserType);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.FirstName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.LastName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", client.JobTitle);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", client.Company);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", client.Email);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Phone Number: </div><div class='col-md-7'><strong>{0}</strong></div>", client.PhoneNumber);

                                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> The user has been notified about the profile changes via an email.</div></div>");

                                var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, true, true);

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

            return PartialView("_EditClient", model);
        }
        #endregion

        #region Edit Admin
        /// <summary>
        /// EditAdmin method updates user and employee profiles
        /// </summary>
        /// <param name="model">Updated user and employee details</param>
        /// <returns>_Success partial view with success message if user and employee were successfully updated, _Error partial view with error message otherwise. If mode is not valid, returns _EditAdmin partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditAdmin(AdminEditViewModel model)
        {
            var response = new HttpResponseMessage();

            var departments = new List<Department>();

            // If user name is taken, add model error with following error message "The User Name is unavalilable."
            if (!String.IsNullOrEmpty(model.UserName))
            {
                if (model.CurrentUserName != model.UserName)
                {
                    if (this.GetUserManager.FindByName(model.UserName) != null)
                    {
                        ModelState.AddModelError("UserName", "The User Name is unavalilable.");
                    }
                }
            }

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
                    var user = await this.GetUserManager.FindByNameAsync(model.CurrentUserName);

                    user.Email = model.Email;

                    user.UserName = model.UserName;

                    var result = await this.GetUserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // If user name and email successfully updated, attempt to update FullName user claim
                        result = await this.GetUserManager.RemoveClaimAsync(user.Id, new Claim("FullName", model.CurrentFullName));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("FullName claim could not be removed from user {0}.", model.CurrentUserName);

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

                        // If FullName user claim successfully updated, attempt to update Role user claim
                        result = await this.GetUserManager.RemoveClaimAsync(user.Id, new Claim("Role", model.CurrentRole));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("Role claim could not be removed from user {0}.", model.CurrentUserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        result = await this.GetUserManager.AddClaimAsync(user.Id, new Claim("Role", model.SelectedRole));

                        if (!result.Succeeded)
                        {
                            var message = "Role claim could not be assigned to user " + user.UserName + ".";

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        // If Role user claim successfully updated, attempt to update employee profile
                        var admin = new Admin();

                        response = await this.GetHttpClient().GetAsync(String.Format("Admin?userName={0}", model.CurrentUserName));

                        if (response.IsSuccessStatusCode) // Ensure that data that is not being changed, remain in the database
                        {
                            admin = await response.Content.ReadAsAsync<Admin>(); 
                        }

                        Mapper.Map<AdminEditViewModel, Admin>(model, admin);
   
                        response = await this.GetHttpClient().PutAsJsonAsync("Admin", admin);

                        if (response.IsSuccessStatusCode)
                        {
                            admin = await response.Content.ReadAsAsync<Admin>();

                            if (admin.UserStatus == UserStatus.Deleted)
                            {
                                return PartialView("_EditAdmin");
                            }

                            if (admin != null)
                            {
                                // If employee profile was successfully updated, send an email notification to the user and display _Success partial view with success message
                                try
                                {
                                    var template = HttpContext.Server.MapPath("~/App_Data/UserUpdateEmailTemplate.txt");

                                    var message = System.IO.File.ReadAllText(template);

                                    message = message.Replace("%name%", admin.FirstName).Replace("%username%", admin.UserName);

                                    Email.EmailService.SendEmail("atomix0x@gmail.com", "Your user account has been successfully updated.", message); // Send notification about account changes to the user via email
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

                                stringBuilder.AppendFormat("<div class='text-center'><h4><strong>User {0} has been successfully updated!</strong></h4></div>", admin.UserName);

                                stringBuilder.Append("<div class='row'><div class='col-md-offset-1'>Please review user details and ensure that all the information is valid.</div>");

                                stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-2 col-md-3'>User Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.UserName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>User Type: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.UserType);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>First Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.FirstName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Last Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.LastName);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Job Title: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.JobTitle);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Company Name: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Company);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Email: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Email);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Phone Number: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.PhoneNumber);

                                stringBuilder.AppendFormat("<div class='col-md-offset-2 col-md-3'>Role: </div><div class='col-md-7'><strong>{0}</strong></div>", admin.Role.ToString());

                                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> The user has been notified about the profile changes via an email.</div></div>");

                                var userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), false, true, true);

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

            return PartialView("_EditAdmin", model);
        }
        #endregion

        #region Reset Password
        /// <summary>
        /// ResetPassword method populates the view model with properties received from _EditAgent, _EditClient, and _EditAdmin partial views and passes it to _ResetPassword partial view
        /// </summary>
        /// <param name="userName">Setter for UserName property</param>
        /// <param name="userType">Setter for UserType property</param>
        /// <param name="email">Setter for Email property</param>
        /// <param name="firstName">Setter for FirstName property</param>
        /// <returns>_ResetPassword partial view with populated view model</returns>
        [HttpGet]
        public PartialViewResult ResetPassword(string userName, string userType, string email, string firstName)
        {
            var resetPasswordViewModel = new ResetPasswordViewModel(userName, userType, email, firstName);

            return PartialView("_ResetPassword", resetPasswordViewModel);
        }

        /// <summary>
        /// ResetPassword method validates new password and displays an error message if password does not match password criteria, otherwise replaces old password with a new password
        /// </summary>
        /// <param name="model">User information required to reset password</param>
        /// <returns>_Confirmation partial view with confirmation message of success or failure depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> ResetPassword(ResetPasswordViewModel model)
        {
            var stringBuilder = new StringBuilder();

            var userConfirmationViewModel = new UserConfirmationViewModel();

            if (!String.IsNullOrEmpty(model.Password))
            {
                // If password is invalid format, display _Confirmation partial view with following error message "Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z')."
                if (!Utilities.RegexMatch(this.PasswordRegex, model.Password))
                {
                    stringBuilder.Clear();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be reset.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Passwords must have at least one non letter or digit character, least one lowercase ('a'-'z'), least one uppercase ('A'-'Z').</div>");

                    stringBuilder.Append("<div class='col-md-offset-1 col-md-11'>Please try again using valid password pattern.</div></div>");

                    userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), model.UserName, model.UserType);

                    return PartialView("_Confirmation", userConfirmationViewModel);
                }
            }

            if(ModelState.IsValid)
            {
                // Find the user by user name
                var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                if(identity != null)
                {
                    // Attempt to update password
                    try
                    {
                        var result = await this.GetUserManager.RemovePasswordAsync(identity.Id);

                        if (!result.Succeeded)
                        {
                            var message = String.Format("Password could not be removed from user {0}.", identity.UserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new PasswordResetException(message);
                        }

                        result = await this.GetUserManager.AddPasswordAsync(identity.Id, model.Password);

                        if (!result.Succeeded)
                        {
                            var message = String.Format("Password could not be removed from user {0}.", identity.UserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new PasswordResetException(message);
                        }

                        // Send updated password to the user via email
                        try
                        {
                            var template = HttpContext.Server.MapPath("~/App_Data/PasswordResetEmailTemplate.txt");

                            var message = System.IO.File.ReadAllText(template);

                            message = message.Replace("%name%", model.FirstName).Replace("%username%", model.UserName).Replace("%password%", model.Password);

                            Email.EmailService.SendEmail("atomix0x@gmail.com", "Your password has been successfully reset.", message); 
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

                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>Success!</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>User password has been successfully reset.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> New password has been sent to the user via an email.</div></div>");

                        userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), model.UserName, model.UserType);

                        return PartialView("_Confirmation", userConfirmationViewModel);
                    }
                    catch(PasswordResetException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be reset.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to reset a user password. Please review an exception log for more details about the exception.</div></div>");

                        userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), model.UserName, model.UserType);

                        return PartialView("_Confirmation", userConfirmationViewModel); 
                    }
                }
            }

            stringBuilder.Clear();

            stringBuilder.Append("<div class='text-center'><h4><strong>Password could NOT be reset.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>ModelState is not valid for current instance of the request. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

            userConfirmationViewModel = new UserConfirmationViewModel(stringBuilder.ToString(), model.UserName, model.UserType);

            return PartialView("_Confirmation", userConfirmationViewModel); 
        }
        #endregion

        #region Block User
        /// <summary>
        /// BlockUser method populates the view model with properties received from _EditAgent, _EditClient, and _EditAdmin partial views and passes it to _BlockUser partial view
        /// </summary>
        /// <param name="userName">Setter for UserName property</param>
        /// <param name="userType">Setter for UserType property</param>
        /// <returns>_BlockUser partial view with populated view model</returns>
        [HttpGet]
        public PartialViewResult BlockUser(string userName, string userType)
        {
            var userConfirmationViewModel = new UserConfirmationViewModel() { UserName = userName, UserType = userType };

            return PartialView("_BlockUser", userConfirmationViewModel);
        }

        /// <summary>
        /// BlockUser method sets employee UserStatus property and user UserStatus claim to BLOCKED and returns _Confirmation partial view with the result (Success or failure) of this method
        /// </summary>
        /// <param name="model">User details required to block the user</param>
        /// <returns>_Confirmation partial view with confirmation message of success or failure depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> BlockUser(UserConfirmationViewModel model)
        {
            var stringBuilder = new StringBuilder();

            try
            {
                if (model.UserType == UserType.Agent.ToString())
                {
                    // Attempt to block an employee
                    var response = await this.GetHttpClient().PutAsync(String.Format("Agent?userName={0}&block={1}", model.UserName, true), null);

                    if (response.IsSuccessStatusCode)
                    {
                        var agent = await response.Content.ReadAsAsync<Agent>();

                        if (agent.UserStatus != UserStatus.Blocked)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be blocked at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to BLOCKED. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            // If employee status was successfully changed to BLOCKED, attempt to change UserStatus claim from ACTIVE to BLOCKED
                            var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                            if (identity != null)
                            {
                                
                                var result = await this.GetUserManager.RemoveClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Active.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be removed from user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }

                                result = await this.GetUserManager.AddClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Blocked.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be assigned to user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be blocked at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
                else if (model.UserType == UserType.Client.ToString())
                {
                    // Attempt to block an employee
                    var response = await this.GetHttpClient().PutAsync(String.Format("Client?userName={0}&block={1}", model.UserName, true), null);

                    if (response.IsSuccessStatusCode)
                    {
                        var client = await response.Content.ReadAsAsync<Agent>();

                        if (client.UserStatus != UserStatus.Blocked)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be blocked at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to BLOCKED. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            // If employee status was successfully changed to BLOCKED, attempt to change UserStatus claim from ACTIVE to BLOCKED
                            var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                            if (identity != null)
                            {
                                var result = await this.GetUserManager.RemoveClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Active.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be removed from user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }

                                result = await this.GetUserManager.AddClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Blocked.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be assigned to user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be blocked at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
                else if (model.UserType == UserType.Admin.ToString())
                {
                    // Attempt to block an employee
                    var response = await this.GetHttpClient().PutAsync(String.Format("Admin?userName={0}&block={1}", model.UserName, true), null);

                    if (response.IsSuccessStatusCode)
                    {
                        var admin = await response.Content.ReadAsAsync<Agent>();

                        if (admin.UserStatus != UserStatus.Blocked)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be blocked at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to BLOCKED. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            // If employee status was successfully changed to BLOCKED, attempt to change UserStatus claim from ACTIVE to BLOCKED
                            var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                            if (identity != null)
                            {
                                var result = await this.GetUserManager.RemoveClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Active.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be removed from user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }

                                result = await this.GetUserManager.AddClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Blocked.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be assigned to user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be blocked at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
            }
            catch (ClaimsAssignmentException ex)
            {
                // Log exception
                ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                stringBuilder.Clear();

                stringBuilder.Append("<div class='text-center'><h4><strong>Claim could NOT be assigned to the user.</strong></h4></div>");

                stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to assign a user claim. User is still ACTIVE! Please review an exception log for more details about the exception.</div></div>");

                model.Message = stringBuilder.ToString();

                return PartialView("_Confirmation", model);
            }

            stringBuilder.Clear();

            stringBuilder.Append("<div class='text-center'><h4><strong>User has been blocked successfully.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>This user no longer has access to the application areas that require authorization.</div>");
            
            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> This action can be undone from Edit User page.</div></div>");

            model.Message = stringBuilder.ToString();

            model.RefreshEditForm = true;

            model.RefreshList = true;

            return PartialView("_Confirmation", model);
        }
        #endregion

        #region Unblock User
        /// <summary>
        /// UnblockUser method populates the view model with properties received from _EditAgent, _EditClient, and _EditAdmin partial views and passes it to _UnblockUser partial view
        /// </summary>
        /// <param name="userName">Setter for UserName property</param>
        /// <param name="userType">Setter for UserType property</param>
        /// <returns>_UnblockUser partial view with populated view model</returns>
        [HttpGet]
        public PartialViewResult UnblockUser(string userName, string userType)
        {
            var userConfirmationViewModel = new UserConfirmationViewModel(userName, userType);

            return PartialView("_UnblockUser", userConfirmationViewModel);
        }

        /// <summary>
        /// UnblockUser method sets employee UserStatus property and user UserStatus claim to ACTIVE and returns _Confirmation partial view with the result (Success or failure) of this method
        /// </summary>
        /// <param name="model">User details required to unblock the user</param>
        /// <returns>_Confirmation partial view with confirmation message of success or failure depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> UnblockUser(UserConfirmationViewModel model)
        {
            var stringBuilder = new StringBuilder();

            try 
            {
                if (model.UserType == UserType.Agent.ToString())
                {
                    // Attempt to unblock an employee
                    var response = await this.GetHttpClient().PutAsync(String.Format("Agent?userName={0}&block={1}", model.UserName, false), null);

                    if (response.IsSuccessStatusCode)
                    {
                        var agent = await response.Content.ReadAsAsync<Agent>();

                        if (agent.UserStatus != UserStatus.Active)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be unblocked at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to ACTIVE. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            // If employee status was successfully changed to ACTIVE, attempt to change UserStatus claim from BLOCKED to ACTIVE
                            var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                            if (identity != null)
                            {
                                var result = await this.GetUserManager.RemoveClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Blocked.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be removed from user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }

                                result = await this.GetUserManager.AddClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Active.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be assigned to user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be unblocked at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
                else if (model.UserType == UserType.Client.ToString())
                {
                    // Attempt to unblock an employee
                    var response = await this.GetHttpClient().PutAsync(String.Format("Client?userName={0}&block={1}", model.UserName, false), null);

                    if (response.IsSuccessStatusCode)
                    {
                        var client = await response.Content.ReadAsAsync<Agent>();

                        if (client.UserStatus != UserStatus.Active)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be unblocked at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to ACTIVE. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            // If employee status was successfully changed to ACTIVE, attempt to change UserStatus claim from BLOCKED to ACTIVE
                            var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                            if (identity != null)
                            {
                                var result = await this.GetUserManager.RemoveClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Blocked.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be removed from user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }

                                result = await this.GetUserManager.AddClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Active.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be assigned to user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be unblocked at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
                else if (model.UserType == UserType.Admin.ToString())
                {
                    // Attempt to unblock an employee
                    var response = await this.GetHttpClient().PutAsync(String.Format("Admin?userName={0}&block={1}", model.UserName, false), null);

                    if (response.IsSuccessStatusCode)
                    {
                        var admin = await response.Content.ReadAsAsync<Agent>();

                        if (admin.UserStatus != UserStatus.Active)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be unblocked at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to ACTIVE. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            // If employee status was successfully changed to ACTIVE, attempt to change UserStatus claim from BLOCKED to ACTIVE
                            var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                            if (identity != null)
                            {
                                var result = await this.GetUserManager.RemoveClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Blocked.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be removed from user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }

                                result = await this.GetUserManager.AddClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Active.ToString()));

                                if (!result.Succeeded)
                                {
                                    var message = String.Format("UserStatus claim could not be assigned to user {0}.", identity.UserName);

                                    if (!Utilities.IsEmpty(result.Errors))
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            message += " " + error;
                                        }
                                    }

                                    throw new ClaimsAssignmentException(message);
                                }
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be unblocked at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
            }
            catch (ClaimsAssignmentException ex)
            {
                // Log exception
                ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                ModelState.AddModelError("ConfirmationMessage", String.Format("UserStatus claim could not be updated for user {0}. User is still BLOCKED! Please see exception logs for more details.", model.UserName));

                return PartialView("_Confirmation", model);
            }

            stringBuilder.Clear();

            stringBuilder.Append("<div class='text-center'><h4><strong>User has been unblocked successfully.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>This user can now access the application.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> This action can be undone from Edit User page.</div></div>");

            model.Message = stringBuilder.ToString();

            model.RefreshEditForm = true;

            model.RefreshList = true;

            return PartialView("_Confirmation", model);
        }
        #endregion

        #region Delete User
        /// <summary>
        /// DeleteUser method populates the view model with employee information retrieved from database and passes it to _UnblockUser partial view
        /// </summary>
        /// <param name="userName">Property required to retrieve the employee profile from database</param>
        /// <param name="userType">Property required to retrieve the employee profile from database</param>
        /// <returns>_DeleteUser partial view with populated view model</returns>
        [HttpGet]
        public async Task<PartialViewResult> DeleteUser(string userName, string userType)
        {
            var response = new HttpResponseMessage();

            var userConfirmationViewModel = new UserConfirmationViewModel();

            if(userType == UserType.Agent.ToString())
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Agent?userName={0}", userName));

                if(response.IsSuccessStatusCode)
                {
                    userConfirmationViewModel = Mapper.Map<Agent, UserConfirmationViewModel>(await response.Content.ReadAsAsync<Agent>());
                }
            }
            else if (userType == UserType.Client.ToString())
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", userName));

                if (response.IsSuccessStatusCode)
                {
                    userConfirmationViewModel = Mapper.Map<Client, UserConfirmationViewModel>(await response.Content.ReadAsAsync<Client>());
                }
            }
            else if (userType == UserType.Admin.ToString())
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Admin?userName={0}", userName));

                if (response.IsSuccessStatusCode)
                {
                    userConfirmationViewModel = Mapper.Map<Admin, UserConfirmationViewModel>(await response.Content.ReadAsAsync<Admin>());
                }
            }

            return PartialView("_DeleteUser", userConfirmationViewModel);
        }

        /// <summary>
        /// DeleteUser method either physically deletes the user profile and employee profile, or flags both the user profile (By setting UserStatus claim to DELETED) and the employee profile (By setting UserStatus property to DELETED) as DELETED
        /// </summary>
        /// <param name="model">User details required to delete the user</param>
        /// <returns>_Confirmation partial view with confirmation message of success or failure depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> DeleteUser(UserConfirmationViewModel model)
        {
            var stringBuilder = new StringBuilder();

            var response = new HttpResponseMessage();

            try
            {
                var identity = await this.GetUserManager.FindByNameAsync(model.UserName);

                if (identity != null)
                {
                    var result = new IdentityResult();

                    if (model.PhysicalDelete) // Physical delete
                    {
                        // Attempt to delete user
                        result = await this.GetUserManager.DeleteAsync(identity);

                        if (!result.Succeeded)
                        {
                            var message = String.Format("User {0} could not be deleted.", identity.UserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new UserDeleteException(message);
                        }
                    }
                    else
                    {
                        // Attempt to change UserStatus claim to DELETED
                        result = await this.GetUserManager.RemoveClaimAsync(identity.Id, new Claim("UserStatus", model.UserStatus));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("UserStatus claim could not be removed from user {0}.", identity.UserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }

                        result = await this.GetUserManager.AddClaimAsync(identity.Id, new Claim("UserStatus", UserStatus.Deleted.ToString()));

                        if (!result.Succeeded)
                        {
                            var message = String.Format("UserStatus claim could not be assigned to user {0}.", identity.UserName);

                            if (!Utilities.IsEmpty(result.Errors))
                            {
                                foreach (var error in result.Errors)
                                {
                                    message += " " + error;
                                }
                            }

                            throw new ClaimsAssignmentException(message);
                        }
                    }
                }

                if (model.UserType == UserType.Agent.ToString())
                {
                    if (model.PhysicalDelete)
                    {
                        // Attempt to delete an employee
                        response = await this.GetHttpClient().DeleteAsync(String.Format("Agent?userName={0}&physicalDelete=true", model.UserName));
                    }
                    else
                    {
                        // Attempt to change UserStatus to DELETED
                        response = await this.GetHttpClient().DeleteAsync(String.Format("Agent?userName={0}", model.UserName));
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var agent = await response.Content.ReadAsAsync<Agent>();

                        if (agent.UserStatus != UserStatus.Deleted)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be feleted at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to DELETED. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User has been deleted successfully.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>This is logical delete, which means that user is still be present in the database, but flagged as deleted.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> User name and email address of deleted user will not be available to new users, unless physical delete is performed.</div></div>");

                            model.Message = stringBuilder.ToString();

                            model.RefreshEditForm = true;

                            model.RefreshList = true;

                            return PartialView("_Confirmation", model);
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be deleted at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
                else if (model.UserType == UserType.Client.ToString())
                {
                    if (model.PhysicalDelete)
                    {
                        // Attempt to delete an employee
                        response = await this.GetHttpClient().DeleteAsync(String.Format("Client?userName={0}&physicalDelete=true", model.UserName));
                    }
                    else
                    {
                        // Attempt to change UserStatus to DELETED
                        response = await this.GetHttpClient().DeleteAsync(String.Format("Client?userName={0}", model.UserName));
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var client = await response.Content.ReadAsAsync<Agent>();

                        if (client.UserStatus != UserStatus.Deleted)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be feleted at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to DELETED. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User has been deleted successfully.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>This is logical delete, which means that user is still be present in the database, but flagged as deleted.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> User name and email address of deleted user will not be available to new users, unless physical delete is performed.</div></div>");

                            model.Message = stringBuilder.ToString();

                            model.RefreshEditForm = true;

                            model.RefreshList = true;

                            return PartialView("_Confirmation", model);
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be deleted at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
                else if (model.UserType == UserType.Admin.ToString())
                {
                    if (model.PhysicalDelete)
                    {
                        // Attempt to delete an employee
                        response = await this.GetHttpClient().DeleteAsync(String.Format("Admin?userName={0}&physicalDelete=true", model.UserName));
                    }
                    else
                    {
                        // Attempt to change UserStatus to DELETED
                        response = await this.GetHttpClient().DeleteAsync(String.Format("Admin?userName={0}", model.UserName));
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var admin = await response.Content.ReadAsAsync<Agent>();

                        if (admin.UserStatus != UserStatus.Deleted)
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be feleted at this time.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is successfull, but user status of returned model is not equal to DELETED. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            model.Message = stringBuilder.ToString();

                            return PartialView("_Confirmation", model);
                        }
                        else
                        {
                            stringBuilder.Clear();

                            stringBuilder.Append("<div class='text-center'><h4><strong>User has been deleted successfully.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>This is logical delete, which means that user is still be present in the database, but flagged as deleted.</div>");

                            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> User name and email address of deleted user will not be available to new users, unless physical delete is performed.</div></div>");

                            model.Message = stringBuilder.ToString();

                            model.RefreshEditForm = true;

                            model.RefreshList = true;

                            return PartialView("_Confirmation", model);
                        }
                    }
                    else
                    {
                        stringBuilder.Clear();

                        stringBuilder.Append("<div class='text-center'><h4><strong>User could NOT be deleted at this time.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Response is UNSUCCESSFUL. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        model.Message = stringBuilder.ToString();

                        return PartialView("_Confirmation", model);
                    }
                }
            }
            catch (ClaimsAssignmentException ex)
            {
                // Log exception
                ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                ModelState.AddModelError("ConfirmationMessage", String.Format("There has been a problem with assigning user claims. Please see exception logs for more details.", model.UserName));

                return PartialView("_Confirmation", model);
            }
            catch (UserDeleteException ex)
            {
                // Log exception
                ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                ModelState.AddModelError("ConfirmationMessage", String.Format("User could not be deleted at this time. Please see exception logs for more details.", model.UserName));

                return PartialView("_Confirmation", model);
            }

            stringBuilder.Clear();

            stringBuilder.Append("<div class='text-center'><h4><strong>Could NOT determine the user.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>User type could not be recognized. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

            model.Message = stringBuilder.ToString();

            return PartialView("_Confirmation", model);
        }
        #endregion

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
        #endregion
    }
}