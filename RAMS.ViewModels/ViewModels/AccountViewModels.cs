using RAMS.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RAMS.Web.Identity
{
    /// <summary>
    /// LoginViewModel view model declares properties for Login view
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}

namespace RAMS.ViewModels 
{
    /// <summary>
    /// EmployeeViewModel view model is a base view model for RegisterAgentViewModel, RegisterClientViewModel, RegisterAdminViewModel and declares common properties of those view models
    /// </summary>
    public class EmployeeViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } // User name is the connection between Employee class and User class (Must be unique)

        [Required]
        [Display(Name = "User Type")]
        public UserType UserType { get; set; } // User type defines to whom employee belongs to (Client, Agent/Manager, or SystemAdmin)

        [Required]
        [Display(Name = "Role")]
        public Role Role { get; set; } // Role determines what options are available for this employee 

        [Required]
        [Display(Name = "User Status")]
        public UserStatus UserStatus { get; set; } // User can be Blocked, Active, Deleted

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }        
    }

    /// <summary>
    /// RegisterAgentViewModel view model declares properties for _RegisterAgent partial view
    /// </summary>
    public class RegisterAgentViewModel : EmployeeViewModel
    {
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public List<System.Web.Mvc.SelectListItem> Departments { get; set; } // Select list for dropdowns

        [Required]
        [Display(Name = "Agent Status")]
        public AgentStatus AgentStatus { get; set; }

        [Required]
        [Display(Name = "Agent Status")]
        public string SelectedAgentStatus { get; set; } // Selected Agent Status

        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; } // Selected Role

        /// <summary>
        /// RegisterAgentViewModel constructor sets UserType to Agent, and UserStatus to Active
        /// </summary>
        public RegisterAgentViewModel()
        {
            this.UserType = Enums.UserType.Agent;
            this.UserStatus = Enums.UserStatus.Active;
        }
    }

    /// <summary>
    /// RegisterClientViewModel view model declares properties for _RegisterClient partial view
    /// </summary>
    public class RegisterClientViewModel : EmployeeViewModel 
    {
        /// <summary>
        /// RegisterClientViewModel constructor sets UserType to Client, UserStatus to Active, and Role to Employee
        /// </summary>
        public RegisterClientViewModel()
        {
            this.UserType = Enums.UserType.Client;
            this.UserStatus = Enums.UserStatus.Active;
            this.Role = Enums.Role.Employee;
        }
    }

    /// <summary>
    /// RegisterAdminViewModel view model declares properties for _RegisterAdmin partial view
    /// </summary>
    public class RegisterAdminViewModel : EmployeeViewModel 
    {
        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; } // Selected Role

        /// <summary>
        /// RegisterAdminViewModel constructor sets UserType to Admin, and UserStatus to Active
        /// </summary>
        public RegisterAdminViewModel()
        {
            this.UserType = Enums.UserType.Admin;
            this.UserStatus = Enums.UserStatus.Active;
        }
    }

    /// <summary>
    /// UserTypeViewModel view model declates properties for _UserTypeSelect partial view
    /// </summary>
    public class UserTypeViewModel
    {
        public UserType UserType { get; set; }

        [Display(Name = "User Type")]
        public string SelectedValue { get; set; } // Selected User Type
    }

    /// <summary>
    /// UserListViewModel view model declares properties for _UserList partial view
    /// </summary>
    public class UserListViewModel
    {
        [Display(Name = "Id")]
        public string UserId { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public Role Role { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        [Display(Name = "User Status")]
        public UserStatus UserStatus { get; set; }
    }

    /// <summary>
    /// EditEmployeeViewModel view model is a base view model for EditAgentViewModel, EditClientViewModel, EditAdminViewModel and declares common properties of those view models
    /// </summary>
    public class EditEmployeeViewModel
    {
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Current User Name")]
        public string CurrentUserName { get; set; } // CurrentUserName is used for tracking user name before it has been updated

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } // User name is the connection between Employee class and User class (Must be unique)

        [Required]
        [Display(Name = "User Type")]
        public UserType UserType { get; set; } // User type defines to whom employee belongs to (Client, Agent/Manager, or SystemAdmin)

        [Required]
        [Display(Name = "Current Role")]
        public string CurrentRole { get; set; } // CurrentRole is used for tracking role before it has been updated

        [Required]
        [Display(Name = "Role")]
        public Role Role { get; set; } // Role determines what options are available for this employee 

        [Required]
        [Display(Name = "User Status")]
        public UserStatus UserStatus { get; set; } // User can be Blocked, Active, Deleted

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Current Full Name")]
        public string CurrentFullName { get; set; } // CurrentFullName is used for tracking full name before it has been updated

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Required]
        [Display(Name = "Current Email")]
        public string CurrentEmail { get; set; } // CurrentEmail is used for tracking email before it has been updated

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public byte[] Timestamp { get; set; }
    }

    /// <summary>
    /// EditAgentViewModel view model declares properties for _EditAgent partial view
    /// </summary>
    public class EditAgentViewModel : EditEmployeeViewModel
    {
        public int AgentId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public List<System.Web.Mvc.SelectListItem> Departments { get; set; } // Select list for dropdowns

        [Required]
        [Display(Name = "Agent Status")]
        public AgentStatus AgentStatus { get; set; }

        [Required]
        [Display(Name = "Agent Status")]
        public string SelectedAgentStatus { get; set; } // Selected Agent Status

        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; } // Selected Role
    }

    /// <summary>
    /// EditClientViewModel view model declares properties for _EditClient partial view
    /// </summary>
    public class EditClientViewModel : EditEmployeeViewModel
    {
        public int ClientId { get; set; }
    }

    /// <summary>
    /// EditAdminViewModel view model declares properties for _EditAdmin partial view
    /// </summary>
    public class EditAdminViewModel : EditEmployeeViewModel
    {
        public int AdminId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; } // Selected Role
    }

    /// <summary>
    /// ResetPasswordViewModel view model declares properties for _ResetPassword partial view
    /// </summary>
    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserType { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[1234567890])(?=.*[!@#$%^&*()_+=-]).+$", ErrorMessage = "Invalid Password format.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// ConfirmationViewModel view model declares properties for _Confirmation, _Success, and _Error partial views
    /// </summary>
    public class ConfirmationViewModel
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "User Type")]
        public string UserType { get; set; }

        [Display(Name = "User Status")]
        public string UserStatus { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Refresh Edit Form?")]
        public bool RefreshEditForm { get; set; }

        [Display(Name = "Refresh User List?")]
        public bool RefreshUserList { get; set; }

        [Display(Name = "Clear Messages?")]
        public bool ClearMessages { get; set; }

        [Display(Name = "Physical Delete?")]
        public bool PhysicalDelete { get; set; }

        /// <summary>
        /// Default ConfirmationViewModel constructor sets all the properties default values
        /// </summary>
        public ConfirmationViewModel()
        {
            this.Message = "";

            this.RefreshEditForm = false;

            this.RefreshUserList = false;

            this.ClearMessages = false;

            this.PhysicalDelete = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructor which sets the Message property
        /// </summary>
        /// <param name="message">Setter for message property</param>
        public ConfirmationViewModel(string message)
        {
            this.Message = message;

            this.RefreshEditForm = false;

            this.RefreshUserList = false;

            this.ClearMessages = false;

            this.PhysicalDelete = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructor which sets UserName and UserType properties
        /// </summary>
        /// <param name="userName">Setter for user name property</param>
        /// <param name="userType">Setter for user type property</param>
        public ConfirmationViewModel(string userName, string userType)
        {
            this.Message = "";

            this.UserName = userName;

            this.UserType = userType;

            this.RefreshEditForm = false;

            this.RefreshUserList = false;

            this.ClearMessages = false;

            this.PhysicalDelete = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructors which sets Message, UserName, and UserType properties
        /// </summary>
        /// <param name="message">Setter for Message property</param>
        /// <param name="userName">Setter for UserName property</param>
        /// <param name="userType">Setter for UserType property</param>
        public ConfirmationViewModel(string message, string userName, string userType)
        {
            this.Message = message;

            this.UserName = userName;

            this.UserType = userType;

            this.RefreshEditForm = false;

            this.RefreshUserList = false;

            this.ClearMessages = false;

            this.PhysicalDelete = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructor which sets Message, RefreshEditForm, and RefreshUserList properties
        /// </summary>
        /// <param name="message">Setter for Message property</param>
        /// <param name="refreshEditForm">Setter for RefreshEditForm property</param>
        /// <param name="refreshUserList">Setter for RefreshUserList property</param>
        public ConfirmationViewModel(string message, bool refreshEditForm, bool refreshUserList)
        {
            this.Message = message;

            this.RefreshEditForm = refreshEditForm;

            this.RefreshUserList = refreshUserList;

            this.PhysicalDelete = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructor which sets Message, RefreshEditForm, RefreshUserList, and ClearMessages properties
        /// </summary>
        /// <param name="message">Setter for Message property</param>
        /// <param name="refreshEditForm">Setter for RefreshEditForm property</param>
        /// <param name="refreshUserList">Setter for RefreshUserList property</param>
        /// <param name="clearMessages">Setter for ClearMessages property</param>
        public ConfirmationViewModel(string message, bool refreshEditForm, bool refreshUserList, bool clearMessages)
        {
            this.Message = message;

            this.RefreshEditForm = refreshEditForm;

            this.RefreshUserList = refreshUserList;

            this.ClearMessages = clearMessages;

            this.PhysicalDelete = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructor which sets UserName, UserType, RefreshEditForm, and RefreshUserList properties
        /// </summary>
        /// <param name="userName">Setter for UserName property</param>
        /// <param name="userType">Setter for UserType property</param>
        /// <param name="refreshEditForm">Setter for RefreshEditForm property</param>
        /// <param name="refreshUserList">Setter for RefreshUserList property</param>
        public ConfirmationViewModel(string userName, string userType, bool refreshEditForm, bool refreshUserList)
        {
            this.Message = "";

            this.UserName = userName;

            this.UserType = userType;

            this.RefreshEditForm = refreshEditForm;

            this.RefreshUserList = refreshUserList;

            this.ClearMessages = false;

            this.PhysicalDelete = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructor which sets Message, UserName, UserType, RefreshEditForm, RefreshUserList, and PhysicalDelete properties
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userName"></param>
        /// <param name="userType"></param>
        /// <param name="refreshEditForm"></param>
        /// <param name="refreshUserList"></param>
        /// <param name="physicalDelete"></param>
        public ConfirmationViewModel(string message, string userName, string userType, bool refreshEditForm, bool refreshUserList, bool physicalDelete)
        {
            this.Message = message;

            this.UserName = userName;

            this.UserType = userType;

            this.RefreshEditForm = refreshEditForm;

            this.RefreshUserList = refreshUserList;

            this.ClearMessages = false;

            this.PhysicalDelete = physicalDelete;
        }
    }

    public class EditUserProfileViewModel
    {
        // TODO - Add GetAsync for EditAgent, EditClient, and EditAdmin for keeping the image.

        // TODO - Code from Automapper profiles and AccountController

        [Required]
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        [Required]
        [Display(Name = "Role")]
        public Role Role { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Current Full Name")]
        public string CurrentFullName { get; set; } // CurrentFullName is used for tracking full name before it has been updated

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Current Email")]
        public string CurrentEmail { get; set; } // CurrentEmail is used for tracking email before it has been updated

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string Company { get; set; }

        public byte[] Timestamp { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}