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
    /// Client repository implements Client specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get first client with matching user name
        /// </summary>
        /// <param name="userName">User name to match with clients' data</param>
        /// <returns>First client with matching user name</returns>
        public Client GetOneByUserName(string userName)
        {
            return this.GetContext.Clients.Where(a => a.UserName == userName).FirstOrDefault();
        }

        /// <summary>
        /// Get multiple clients with matching user type
        /// </summary>
        /// <param name="userType">User type to match with clients' data</param>
        /// <returns>Multiple clients with matching user type</returns>
        public IEnumerable<Client> GetManyByUserType(UserType userType)
        {
            return this.GetContext.Clients.Where(a => a.UserType == userType).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching user status
        /// </summary>
        /// <param name="userStatus">User status to match with clients' data</param>
        /// <returns>Multiple clients with matching user status</returns>
        public IEnumerable<Client> GetManyByUserStatus(UserStatus userStatus)
        {
            return this.GetContext.Clients.Where(a => a.UserStatus == userStatus).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching role
        /// </summary>
        /// <param name="role">Role to match with clients' data</param>
        /// <returns>Multiple clients with matching role</returns>
        public IEnumerable<Client> GetManyByRole(Role role)
        {
            return this.GetContext.Clients.Where(a => a.Role == role).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching first name
        /// </summary>
        /// <param name="firstName">First name to match with clients' data</param>
        /// <returns>Multiple clients with matching first name</returns>
        public IEnumerable<Client> GetManyByFirstName(string firstName)
        {
            return this.GetContext.Clients.Where(a => a.FirstName == firstName).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching last name
        /// </summary>
        /// <param name="lastName">Last name to match with clients' data</param>
        /// <returns>Multiple clients with matching last name</returns>
        public IEnumerable<Client> GetManyByLastName(string lastName)
        {
            return this.GetContext.Clients.Where(a => a.LastName == lastName).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching job title
        /// </summary>
        /// <param name="jobTitle">Job title to match with clients' data</param>
        /// <returns>Multiple clients with matching job title</returns>
        public IEnumerable<Client> GetManyByJobTitle(string jobTitle)
        {
            return this.GetContext.Clients.Where(a => a.JobTitle == jobTitle).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching company name
        /// </summary>
        /// <param name="companyName">Company name to match with clients' data</param>
        /// <returns>Multiple clients with matching company name</returns>
        public IEnumerable<Client> GetManyByCompanyName(string companyName)
        {
            return this.GetContext.Clients.Where(a => a.Company == companyName).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number to match with clients' data</param>
        /// <returns>Multiple clients with matching phone number</returns>
        public IEnumerable<Client> GetManyByPhoneNumber(string phoneNumber)
        {
            return this.GetContext.Clients.Where(a => a.PhoneNumber == phoneNumber).ToList();
        }

        /// <summary>
        /// Get first client with matching email
        /// </summary>
        /// <param name="email">Email to match with clients' data</param>
        /// <returns>First client with matching email</returns>
        public Client GetOneByEmail(string email)
        {
            return this.GetContext.Clients.Where(a => a.Email == email).FirstOrDefault();
        }

        /// <summary>
        /// Get multiple clients with matching partial email (e. g. email.com, john.doe)
        /// </summary>
        /// <param name="email">Partial email to match with clients' data</param>
        /// <returns>Multiple clients with matching partial email</returns>
        public IEnumerable<Client> GetManyByEmail(string email)
        {
            return this.GetContext.Clients.Where(a => a.Email.ToLower().Trim().Contains(email.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get first client with matching position id
        /// </summary>
        /// <param name="id">Position id to match with clients' data</param>
        /// <returns>First client with matching position id</returns>
        public Client GetOneByPositionId(int id)
        {
            var position = this.GetContext.Positions.Where(p => p.PositionId == id).FirstOrDefault() ?? new Position();

            return this.GetContext.Clients.Where(a => a.ClientId == position.ClientId).FirstOrDefault();
        }

        /// <summary>
        /// Get first client with matching position
        /// </summary>
        /// <param name="position">Position to match with clients' data</param>
        /// <returns>First client with matching position</returns>
        public Client GetOneByPosition(Position position)
        {
            return this.GetContext.Clients.Where(a => a.ClientId == position.ClientId).FirstOrDefault();
        }

        /// <summary>
        /// Get first client with matching notification id
        /// </summary>
        /// <param name="id">Notification id to match with clients' data</param>
        /// <returns>First client with matching notification id</returns>
        public Client GetOneByNotificationId(int id)
        {
            var notification = this.GetContext.Notifications.Where(n => n.NotificationId == id).FirstOrDefault() ?? new Notification();

            return this.GetContext.Clients.Where(a => a.ClientId == notification.ClientId).FirstOrDefault();
        }

        /// <summary>
        /// Get first client with matching notification
        /// </summary>
        /// <param name="notification">Notification to match with clients' data</param>
        /// <returns>First client with matching notification</returns>
        public Client GetOneByNotification(Notification notification)
        {
            return this.GetContext.Clients.Where(a => a.ClientId == notification.ClientId).FirstOrDefault();
        }
        #endregion
    }
}
