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
    /// DeleteUserException is a custom exception that provides additional controls over the application and error handling
    /// DeleteUserException is thrown when user deletion issue occurs
    /// </summary>
    public class DeleteUserException : Exception
    {
        public DeleteUserException() : base("User could not be deleted.") { }

        public DeleteUserException(string message) : base(message) { }

        public DeleteUserException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// DeleteEmployeeException is a custom exception that provides additional controls over the application and error handling
    /// DeleteEmployeeException is thrown when employee deletion issue occurs
    /// </summary>
    public class DeleteEmployeeException : Exception
    {
        public DeleteEmployeeException() : base("Employee could not be deleted.") { }

        public DeleteEmployeeException(string message) : base(message) { }

        public DeleteEmployeeException(string message, Exception innerException) : base(message, innerException) { }
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
}
