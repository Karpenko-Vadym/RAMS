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

namespace RAMS.Service.Interfaces
{
    /// <summary>
    /// NotificationService implements Notification specific services
    /// </summary>
    public class NotificationService : INotificationService
    {
        INotificationRepository NotificationRepository;

        IClientRepository ClientRepository;

        IAgentRepository AgentRepository;

        IAdminRepository AdminRepository;

        IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="notificationRepository">Parameter for setting NotificationRepository</param>
        /// <param name="clinetRepository">Parameter for setting ClientRepository</param>
        /// <param name="agentRepository">Parameter for setting AgentRepository</param>
        /// <param name="adminRepository">Parameter for setting AdminRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public NotificationService(INotificationRepository notificationRepository, IClientRepository clinetRepository, IAgentRepository agentRepository, IAdminRepository adminRepository, IUnitOfWork unitOfWork)
        {
            this.NotificationRepository = notificationRepository;

            this.ClientRepository = clinetRepository;

            this.AgentRepository = agentRepository;

            this.AdminRepository = adminRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the notifications
        /// </summary>
        /// <returns>All the notifications</returns>
        public IEnumerable<Notification> GetAllNotifications()
        {
            return this.NotificationRepository.GetAll();
        }

        /// <summary>
        /// Get notification with matching id
        /// </summary>
        /// <param name="id">Id of the notification to be compared with the context notifications' data</param>
        /// <returns>Notification with matching id</returns>
        public Notification GetOneNotificationById(int id)
        {
            return this.NotificationRepository.GetById(id);
        }

        /// <summary>
        /// Get multiple notifications that belong to specific client
        /// </summary>
        /// <param name="id">Id of the client who's notifications are being retrieved</param>
        /// <returns>Multiple notifications that belong to specific client</returns>
        public IEnumerable<Notification> GetManyNotificationsByClientId(int id)
        {
            var client = this.ClientRepository.GetById(id);

            if (client != null)
            {
                return client.Notifications;
            }

            return null;
        }

        /// <summary>
        /// Get multiple notifications that belong to specific agent
        /// </summary>
        /// <param name="id">Id of the agent who's notifications are being retrieved</param>
        /// <returns>Multiple notifications that belong to specific agent</returns>
        public IEnumerable<Notification> GetManyNotificationsByAgentId(int id)
        {
            var agent = this.AgentRepository.GetById(id);

            if (agent != null)
            {
                return agent.Notifications;
            }

            return null;
        }

        /// <summary>
        /// Get multiple notifications that belong to specific admin
        /// </summary>
        /// <param name="id">Id of the admin who's notifications are being retrieved</param>
        /// <returns>Multiple notifications that belong to specific admin</returns>
        public IEnumerable<Notification> GetManyNotificationsByAdminId(int id)
        {
            var admin = this.AdminRepository.GetById(id);

            if (admin != null)
            {
                return admin.Notifications;
            }

            return null;
        }

        /// <summary>
        /// Get multiple notifications with matching status
        /// </summary>
        /// <param name="status">Notification status to be compared with the context notifications' data</param>
        /// <returns>Multiple notifications with matching status</returns>
        public IEnumerable<Notification> GetManyNotificationsByStatus(NotificationStatus status)
        {
            return this.NotificationRepository.GetManyByStatus(status);
        }

        /// <summary>
        /// Get multiple notifications with matching creation date
        /// </summary>
        /// <param name="dateCreated">Creation date of the notification to be compared with the context notifications' data</param>
        /// <returns>Multiple notifications with matching creation date</returns>
        public IEnumerable<Notification> GetManyNotificationsByCreationDate(DateTime dateCreated)
        {
            return this.NotificationRepository.GetManyByDate(dateCreated);
        }
        #endregion

        /// <summary>
        /// Create new notification
        /// </summary>
        /// <param name="notification">Notification to be created</param>
        public void CreateNotification(Notification notification)
        {
            this.NotificationRepository.Add(notification);
        }

        /// <summary>
        /// Update existing notification
        /// </summary>
        /// <param name="notification">Notification to be updated</param>
        public void UpdateNotification(Notification notification)
        {
            this.NotificationRepository.Update(notification);
        }

        /// <summary>
        /// Delete existing notification
        /// </summary>
        /// <param name="notification">Notification to be deleted</param>
        public void DeleteNotification(Notification notification)
        {
            this.NotificationRepository.Delete(notification);
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
