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
    /// INotificationRepository interface declares the Notification specific repository operations
    /// </summary>
    public interface INotificationRepository : IRepository<Notification> 
    {
        Notification GetOneByNotificationId(int id);

        IEnumerable<Notification> GetAllNotifications();

        IEnumerable<Notification> GetManyByAgentId(int id);

        IEnumerable<Notification> GetManyByAgent(Agent agent);

        IEnumerable<Notification> GetManyByClientId(int id);

        IEnumerable<Notification> GetManyByClient(Client client);

        IEnumerable<Notification> GetManyByTitle(string title);

        IEnumerable<Notification> GetManyByDetails(string details);

        IEnumerable<Notification> GetManyByStatus(NotificationStatus status);

        IEnumerable<Notification> GetManyByDate(DateTime date);
    }
}
