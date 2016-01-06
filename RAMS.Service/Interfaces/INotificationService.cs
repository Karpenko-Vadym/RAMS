using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// INotificationService interface declares the Notification specific service operations
    /// </summary>
    public interface INotificationService
    {
        IEnumerable<Notification> GetAllNotifications();

        Notification GetOneNotificationById(int id);

        IEnumerable<Notification> GetManyNotificationsByClientId(int id);

        IEnumerable<Notification> GetManyNotificationsByAgentId(int id);

        IEnumerable<Notification> GetManyNotificationsByAdminId(int id);

        IEnumerable<Notification> GetManyNotificationsByStatus(NotificationStatus status);

        IEnumerable<Notification> GetManyNotificationsByCreationDate(DateTime dateCreated);

        void CreateNotification(Notification notification);

        void UpdateNotification(Notification notification);

        void DeleteNotification(Notification notification);

        void SaveChanges();
    }
}
