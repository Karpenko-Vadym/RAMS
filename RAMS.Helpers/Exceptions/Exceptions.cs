using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Helpers
{
    /// <summary>
    /// UserRegistrationException is a custom exception that provides additional controls over the application and error handling
    /// UserRegistrationException is thrown when user registration issue occurs
    /// </summary>
    public class UserRegistrationException : Exception
    {
        public UserRegistrationException() : base("User could not be created.") { }

        public UserRegistrationException(string message) : base(message) { }

        public UserRegistrationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// ClaimsAssignmentException is a custom exception that provides additional controls over the application and error handling
    /// ClaimsAssignmentException is thrown when claim (Assignment or removal) issue occurs
    /// </summary>
    public class ClaimsAssignmentException : Exception
    {
        public ClaimsAssignmentException() : base("Claim could not be assigned.") { }

        public ClaimsAssignmentException(string message) : base(message) { }

        public ClaimsAssignmentException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// EmployeeRegistrationException is a custom exception that provides additional controls over the application and error handling
    /// EmployeeRegistrationException is thrown when employee registration issue occurs
    /// </summary>
    public class EmployeeRegistrationException : Exception
    {
        public EmployeeRegistrationException() : base("Employee could not be created.") { }

        public EmployeeRegistrationException(string message) : base(message) { }

        public EmployeeRegistrationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// UserDeleteException is a custom exception that provides additional controls over the application and error handling
    /// UserDeleteException is thrown when user deletion issue occurs
    /// </summary>
    public class UserDeleteException : Exception
    {
        public UserDeleteException() : base("User could not be deleted.") { }

        public UserDeleteException(string message) : base(message) { }

        public UserDeleteException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// EmployeeDeleteException is a custom exception that provides additional controls over the application and error handling
    /// EmployeeDeleteException is thrown when employee deletion issue occurs
    /// </summary>
    public class EmployeeDeleteException : Exception
    {
        public EmployeeDeleteException() : base("Employee could not be deleted.") { }

        public EmployeeDeleteException(string message) : base(message) { }

        public EmployeeDeleteException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// PasswordResetException is a custom exception that provides additional controls over the application and error handling
    /// PasswordResetException is thrown when password reset issue occurs
    /// </summary>
    public class PasswordResetException : Exception
    {
        public PasswordResetException() : base("Password could not be reset.") { }

        public PasswordResetException(string message) : base(message) { }

        public PasswordResetException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// PasswordChangeException is a custom exception that provides additional controls over the application and error handling
    /// PasswordChangeException is thrown when password change issue occurs
    /// </summary>
    public class PasswordChangeException : Exception
    {
        public PasswordChangeException() : base("Password could not be reset.") { }

        public PasswordChangeException(string message) : base(message) { }

        public PasswordChangeException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// UserUpdateException is a custom exception that provides additional controls over the application and error handling
    /// UserUpdateException is thrown when user update issue occurs
    /// </summary>
    public class UserUpdateException : Exception
    {
        public UserUpdateException() : base("User could not be updated.") { }

        public UserUpdateException(string message) : base(message) { }

        public UserUpdateException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// EmployeeUpdateException is a custom exception that provides additional controls over the application and error handling
    /// EmployeeUpdateException is thrown when employee update issue occurs
    /// </summary>
    public class EmployeeUpdateException : Exception
    {
        public EmployeeUpdateException() : base("Employee could not be updated.") { }

        public EmployeeUpdateException(string message) : base(message) { }

        public EmployeeUpdateException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// DepartmentAddException is a custom exception that provides additional controls over the application and error handling
    /// DepartmentAddException is thrown when department creation issue occurs
    /// </summary>
    public class DepartmentAddException : Exception
    {
        public DepartmentAddException() : base("Department could not be added.") { }

        public DepartmentAddException(string message) : base(message) { }

        public DepartmentAddException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// DepartmentUpdateException is a custom exception that provides additional controls over the application and error handling
    /// DepartmentUpdateException is thrown when department update issue occurs
    /// </summary>
    public class DepartmentUpdateException : Exception
    {
        public DepartmentUpdateException() : base("Department could not be updated.") { }

        public DepartmentUpdateException(string message) : base(message) { }

        public DepartmentUpdateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
