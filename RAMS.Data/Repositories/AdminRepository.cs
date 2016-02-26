using RAMS.Data.Infrastructure;
using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Repositories
{
    /// <summary>
    /// Admin repository implements Admin specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class AdminRepository : RepositoryBase<Admin>, IAdminRepository
    {
        public AdminRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get first admin with matching user name
        /// </summary>
        /// <param name="userName">User name to match with admins' data</param>
        /// <returns>First admin with matching user name</returns>
        public Admin GetOneByUserName(string userName)
        {
            return this.GetContext.Admins.Where(a => a.UserName == userName).FirstOrDefault();
        }

        /// <summary>
        /// Get multiple admins with matching user type
        /// </summary>
        /// <param name="userType">User type to match with admins' data</param>
        /// <returns>Multiple admins with matching user type</returns>
        /// 
        public IEnumerable<Admin> GetManyByUserType(UserType userType)
        {
            return this.GetContext.Admins.Where(a => a.UserType == userType).ToList();
        }

        /// <summary>
        /// Get multiple admins with matching user status
        /// </summary>
        /// <param name="userStatus">User status to match with admins' data</param>
        /// <returns>Multiple admins with matching user status</returns>
        public IEnumerable<Admin> GetManyByUserStatus(UserStatus userStatus)
        {
            return this.GetContext.Admins.Where(a => a.UserStatus == userStatus).ToList();
        }

        /// <summary>
        /// Get multiple admins with matching role
        /// </summary>
        /// <param name="role">Role to match with admins' data</param>
        /// <returns>Multiple admins with matching role</returns>
        public IEnumerable<Admin> GetManyByRole(Role role)
        {
            return this.GetContext.Admins.Where(a => a.Role == role).ToList();
        }

        /// <summary>
        /// Get multiple admins with matching first name
        /// </summary>
        /// <param name="firstName">First name to match with admins' data</param>
        /// <returns>Multiple admins with matching first name</returns>
        public IEnumerable<Admin> GetManyByFirstName(string firstName)
        {
            return this.GetContext.Admins.Where(a => a.FirstName == firstName).ToList();
        }

        /// <summary>
        /// Get multiple admins with matching last name
        /// </summary>
        /// <param name="lastName">Last name to match with admins' data</param>
        /// <returns>Multiple clients with matching last name</returns>
        public IEnumerable<Admin> GetManyByLastName(string lastName)
        {
            return this.GetContext.Admins.Where(a => a.LastName == lastName).ToList();
        }

        /// <summary>
        /// Get multiple admins with matching job title
        /// </summary>
        /// <param name="jobTitle">Job title to match with admins' data</param>
        /// <returns>Multiple admins with matching job title</returns>
        public IEnumerable<Admin> GetManyByJobTitle(string jobTitle)
        {
            return this.GetContext.Admins.Where(a => a.JobTitle == jobTitle).ToList();
        }

        /// <summary>
        /// Get multiple admins with matching company name
        /// </summary>
        /// <param name="companyName">Company name to match with amins' data</param>
        /// <returns>Multiple admins with matching company name</returns>
        public IEnumerable<Admin> GetManyByCompanyName(string companyName)
        {
            return this.GetContext.Admins.Where(a => a.Company == companyName).ToList();
        }

        /// <summary>
        /// Get multiple admins with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number to match with amins' data</param>
        /// <returns>Multiple admins with matching phone number</returns>
        public IEnumerable<Admin> GetManyByPhoneNumber(string phoneNumber)
        {
            return this.GetContext.Admins.Where(a => a.PhoneNumber == phoneNumber).ToList();
        }

        /// <summary>
        /// Get first admin with matching email
        /// </summary>
        /// <param name="email">Email to match with admins' data</param>
        /// <returns>First admin with matching email</returns>
        public Admin GetOneByEmail(string email)
        {
            return this.GetContext.Admins.Where(a => a.Email == email).FirstOrDefault();
        }

        /// <summary>
        /// Get multiple admins with matching partial email (e. g. email.com, john.doe)
        /// </summary>
        /// <param name="email">Partial email to match with admins' data</param>
        /// <returns>Multiple admins with matching partial email</returns>
        public IEnumerable<Admin> GetManyByEmail(string email)
        {
            return this.GetContext.Admins.Where(a => a.Email.ToLower().Trim().Contains(email.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get first admin with matching notification id
        /// </summary>
        /// <param name="id">Notification id to match with admins' data</param>
        /// <returns>First admin with matching notification id</returns>
        public Admin GetOneByNotificationId(int id)
        {
            var notification = this.GetContext.Notifications.Where(n => n.NotificationId == id).FirstOrDefault() ?? new Notification();

            return this.GetContext.Admins.Where(a => a.AdminId == notification.AdminId).FirstOrDefault();
        }

        /// <summary>
        /// Get first admin with matching notification
        /// </summary>
        /// <param name="notification">Notification to match with admins' data</param>
        /// <returns>First admin with matching notification</returns>
        public Admin GetOneByNotification(Notification notification)
        {
            return this.GetContext.Admins.Where(a => a.AdminId == notification.AdminId).FirstOrDefault();
        }
        #endregion
    }
}
