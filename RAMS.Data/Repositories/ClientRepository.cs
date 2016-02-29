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
        /// Get first client with matching client id
        /// </summary>
        /// <param name="id">Id to match with clients' data</param>
        /// <returns>First client with matching id</returns>
        public Client GetOneByClientId(int id)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").FirstOrDefault(c => c.ClientId == id);
        }

        /// <summary>
        /// Get all clients
        /// </summary>
        /// <returns>All clients</returns>
        public IEnumerable<Client> GetAllClients()
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications");
        }

        /// <summary>
        /// Get first client with matching user name
        /// </summary>
        /// <param name="userName">User name to match with clients' data</param>
        /// <returns>First client with matching user name</returns>
        public Client GetOneByUserName(string userName)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").FirstOrDefault(c => c.UserName == userName);
        }

        /// <summary>
        /// Get multiple clients with matching user type
        /// </summary>
        /// <param name="userType">User type to match with clients' data</param>
        /// <returns>Multiple clients with matching user type</returns>
        public IEnumerable<Client> GetManyByUserType(UserType userType)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.UserType == userType).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching user status
        /// </summary>
        /// <param name="userStatus">User status to match with clients' data</param>
        /// <returns>Multiple clients with matching user status</returns>
        public IEnumerable<Client> GetManyByUserStatus(UserStatus userStatus)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.UserStatus == userStatus).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching role
        /// </summary>
        /// <param name="role">Role to match with clients' data</param>
        /// <returns>Multiple clients with matching role</returns>
        public IEnumerable<Client> GetManyByRole(Role role)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.Role == role).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching first name
        /// </summary>
        /// <param name="firstName">First name to match with clients' data</param>
        /// <returns>Multiple clients with matching first name</returns>
        public IEnumerable<Client> GetManyByFirstName(string firstName)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.FirstName == firstName).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching last name
        /// </summary>
        /// <param name="lastName">Last name to match with clients' data</param>
        /// <returns>Multiple clients with matching last name</returns>
        public IEnumerable<Client> GetManyByLastName(string lastName)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.LastName == lastName).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching job title
        /// </summary>
        /// <param name="jobTitle">Job title to match with clients' data</param>
        /// <returns>Multiple clients with matching job title</returns>
        public IEnumerable<Client> GetManyByJobTitle(string jobTitle)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.JobTitle == jobTitle).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching company name
        /// </summary>
        /// <param name="companyName">Company name to match with clients' data</param>
        /// <returns>Multiple clients with matching company name</returns>
        public IEnumerable<Client> GetManyByCompanyName(string companyName)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.Company == companyName).ToList();
        }

        /// <summary>
        /// Get multiple clients with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number to match with clients' data</param>
        /// <returns>Multiple clients with matching phone number</returns>
        public IEnumerable<Client> GetManyByPhoneNumber(string phoneNumber)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.PhoneNumber == phoneNumber).ToList();
        }

        /// <summary>
        /// Get first client with matching email
        /// </summary>
        /// <param name="email">Email to match with clients' data</param>
        /// <returns>First client with matching email</returns>
        public Client GetOneByEmail(string email)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").FirstOrDefault(c => c.Email == email);
        }

        /// <summary>
        /// Get multiple clients with matching partial email (e. g. email.com, john.doe)
        /// </summary>
        /// <param name="email">Partial email to match with clients' data</param>
        /// <returns>Multiple clients with matching partial email</returns>
        public IEnumerable<Client> GetManyByEmail(string email)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").Where(c => c.Email.ToLower().Trim().Contains(email.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get first client with matching position id
        /// </summary>
        /// <param name="id">Position id to match with clients' data</param>
        /// <returns>First client with matching position id</returns>
        public Client GetOneByPositionId(int id)
        {
            var position = this.GetContext.Positions.FirstOrDefault(p => p.PositionId == id) ?? new Position();

            return this.GetContext.Clients.Include("Positions").Include("Notifications").FirstOrDefault(c => c.ClientId == position.ClientId);
        }

        /// <summary>
        /// Get first client with matching position
        /// </summary>
        /// <param name="position">Position to match with clients' data</param>
        /// <returns>First client with matching position</returns>
        public Client GetOneByPosition(Position position)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").FirstOrDefault(c => c.ClientId == position.ClientId);
        }

        /// <summary>
        /// Get first client with matching notification id
        /// </summary>
        /// <param name="id">Notification id to match with clients' data</param>
        /// <returns>First client with matching notification id</returns>
        public Client GetOneByNotificationId(int id)
        {
            var notification = this.GetContext.Notifications.FirstOrDefault(n => n.NotificationId == id) ?? new Notification();

            return this.GetContext.Clients.Include("Positions").Include("Notifications").FirstOrDefault(c => c.ClientId == notification.ClientId);
        }

        /// <summary>
        /// Get first client with matching notification
        /// </summary>
        /// <param name="notification">Notification to match with clients' data</param>
        /// <returns>First client with matching notification</returns>
        public Client GetOneByNotification(Notification notification)
        {
            return this.GetContext.Clients.Include("Positions").Include("Notifications").FirstOrDefault(c => c.ClientId == notification.ClientId);
        }
        #endregion
    }
}
