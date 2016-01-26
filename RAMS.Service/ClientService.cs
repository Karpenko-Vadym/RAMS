using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// ClientService implements Client specific services
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IClientRepository ClientRepository;

        private readonly IPositionRepository PositionRepository;

        private readonly INotificationRepository NotificationRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="clientRepository">Parameter for setting ClientRepository</param>
        /// <param name="positionRepository">Parameter for setting PositionRepository</param>
        /// <param name="notificationRepository">Parameter for setting NotificationRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public ClientService(IClientRepository clientRepository, IPositionRepository positionRepository, INotificationRepository notificationRepository, IUnitOfWork unitOfWork) 
        {
            this.ClientRepository = clientRepository;

            this.PositionRepository = positionRepository;

            this.NotificationRepository = notificationRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the clients
        /// </summary>
        /// <returns>All the clients</returns>
        public IEnumerable<Client> GetAllClients()
        {
            return this.ClientRepository.GetAll();
        }

        /// <summary>
        /// Get a client with matching id
        /// </summary>
        /// <param name="id">Id of the client to be compared with the context clients' data</param>
        /// <returns>A client with matching id</returns>
        public Client GetOneClientById(int id)
        {
            return this.ClientRepository.GetById(id);
        }

        /// <summary>
        /// Get a client with matching user name
        /// </summary>
        /// <param name="userName">User name of the client to be compared with the context clients' data</param>
        /// <returns>A client with matching user name</returns>
        public Client GetOneClientByUserName(string userName)
        {
            return this.ClientRepository.GetOneByUserName(userName);
        }

        /// <summary>
        /// Get multiple clients with matching user type
        /// </summary>
        /// <param name="userType">User type of the client to be compared with the context clients' data</param>
        /// <returns>Multiple clients with matching user type</returns>
        public IEnumerable<Client> GetManyClientsByUserType(UserType userType)
        {
            return this.ClientRepository.GetManyByUserType(userType);
        }

        /// <summary>
        /// Get multiple clients with matching phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number of the client to be compared with the context clients' data</param>
        /// <returns>Multiple clients with matching phone number</returns>
        public IEnumerable<Client> GetManyClientsByPhoneNumber(string phoneNumber)
        {
            return this.ClientRepository.GetManyByPhoneNumber(phoneNumber);
        }

        /// <summary>
        /// Get multiple clients with matching user status
        /// </summary>
        /// <param name="userStatus">User status of the client to be compared with the context clients' data</param>
        /// <returns>Multiple clients with matching user status</returns>
        public IEnumerable<Client> GetManyClientsByUserStatus(UserStatus userStatus)
        {
            return this.ClientRepository.GetManyByUserStatus(userStatus);
        }

        /// <summary>
        /// Get multiple clients with matching role
        /// </summary>
        /// <param name="role">Client's role to be compared with the context clients' data</param>
        /// <returns>Multiple clients with matching role</returns>
        public IEnumerable<Client> GetManyClientsByRole(Role role)
        {
            return this.ClientRepository.GetManyByRole(role);
        }

        /// <summary>
        /// Get a client with specific position
        /// </summary>
        /// <param name="id">Id of the position for which a client is being retrieved</param>
        /// <returns>A client with specific position</returns>
        public Client GetOneClientByPositionId(int id)
        {
            var position = this.PositionRepository.GetById(id);

            return position.Client;
        }

        /// <summary>
        /// Get a client with specific notification
        /// </summary>
        /// <param name="id">Id of the notification for which a client is being retrieved</param>
        /// <returns>A client with specific notification</returns>
        public Client GetOneClientByNotificationId(int id)
        {
            var notification = this.NotificationRepository.GetById(id);

            return notification.Client;
        }
        #endregion

        /// <summary>
        /// Create new position
        /// </summary>
        /// <param name="client">Client to be created</param>
        public void CreateClient(Client client)
        {
            this.ClientRepository.Add(client);
        }

        /// <summary>
        /// Update existing client
        /// </summary>
        /// <param name="client">Client to be updated</param>
        public void UpdateClient(Client client)
        {
            this.ClientRepository.Update(client);
        }

        /// <summary>
        /// Delete existing client
        /// </summary>
        /// <param name="client">Client to be deleted</param>
        public void DeleteClient(Client client)
        {
            this.ClientRepository.Delete(client);
        }

        /// <summary>
        /// Save changes by using UnitOfWork's Commit method
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                this.UnitOfWork.Commit();
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateConcurrencyException ex)
            #pragma warning restore 0168 
            {
                throw;
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateException ex)
            #pragma warning restore 0168
            {
                throw;
            }
        }
    }
}
