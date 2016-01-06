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
    /// Notification repository implements Notification specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get multiple notifications with matching agent id
        /// </summary>
        /// <param name="id">Agent id to match with notifications' data</param>
        /// <returns>Multiple notifications with matching agent id</returns>
        public IEnumerable<Notification> GetManyByAgentId(int id)
        {
            return this.GetContext.Notifications.Where(n => n.AgentId == id).ToList();
        }

        /// <summary>
        /// Get multiple notifications with matching agent
        /// </summary>
        /// <param name="agent">Agent to match with notifications' data</param>
        /// <returns>Multiple notifications with matching agent</returns>
        public IEnumerable<Notification> GetManyByAgent(Agent agent)
        {
            return this.GetContext.Notifications.Where(n => n.AgentId == agent.AgentId).ToList();
        }

        /// <summary>
        /// Get multiple notifications with matching client id
        /// </summary>
        /// <param name="id">Client id to match with notifications' data</param>
        /// <returns>Multiple notifications with matching client id</returns>
        public IEnumerable<Notification> GetManyByClientId(int id)
        {
            return this.GetContext.Notifications.Where(n => n.ClientId == id).ToList();
        }

        /// <summary>
        /// Get multiple notifications with matching client
        /// </summary>
        /// <param name="client">Client to match with notifications' data</param>
        /// <returns>Multiple notifications with matching client</returns>
        public IEnumerable<Notification> GetManyByClient(Client client)
        {
            return this.GetContext.Notifications.Where(n => n.ClientId == client.ClientId).ToList();
        }

        /// <summary>
        /// Get multiple notifications with title containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="title">Title phrase to match with Notifications' title</param>
        /// <returns>Multiple notifications with title containing the phrase provided in the parameter list</returns>
        public IEnumerable<Notification> GetManyByTitle(string title)
        {
            return this.GetContext.Notifications.Where(n => n.Title.ToLower().Trim().Contains(title.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple notifications with details containing the phrase provided in the parameter list
        /// </summary>
        /// <param name="details">Details phrase to match with Notifications' details field</param>
        /// <returns>Multiple notifications with details containing the phrase provided in the parameter list</returns>
        public IEnumerable<Notification> GetManyByDetails(string details)
        {
            return this.GetContext.Notifications.Where(n => n.Details.ToLower().Trim().Contains(details.ToLower().Trim())).ToList();
        }

        /// <summary>
        /// Get multiple notifications with matching status
        /// </summary>
        /// <param name="status">Status to match with notifications' data</param>
        /// <returns>Multiple notifications with matching status</returns>
        public IEnumerable<Notification> GetManyByStatus(NotificationStatus status)
        {
            return this.GetContext.Notifications.Where(n => n.Status == status).ToList();
        }

        /// <summary>
        /// Get multiple notifications with matching date
        /// </summary>
        /// <param name="date">Date to match with notifications' data</param>
        /// <returns>Multiple notifications with matching date</returns>
        public IEnumerable<Notification> GetManyByDate(DateTime date)
        {
            return this.GetContext.Notifications.Where(n => n.DateCreated == date).ToList();
        }
        #endregion
    }
}
