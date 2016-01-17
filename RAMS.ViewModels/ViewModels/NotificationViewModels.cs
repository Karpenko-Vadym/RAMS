using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.ViewModels
{
    /// <summary>
    /// NotificationListViewModel view model declares properties for _NotificationList partial view
    /// </summary>
    public class NotificationListViewModel
    {
        public int NotificationId { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTime DateCreated { get; set; }
    }

    /// <summary>
    /// NotificationChangeStatusViewModel view model declares properties for _ChangeNotificationStatus partial view
    /// </summary>
    public class NotificationChangeStatusViewModel
    {
        public int NotificationId { get; set; }

        public string NotificationTitle { get; set; }

        public string NotificationStatus { get; set; }

        /// <summary>
        /// Default NotificationChangeStatusViewModel constructor
        /// </summary>
        public NotificationChangeStatusViewModel() { }

        /// <summary>
        /// NotificationChangeStatusViewModel constructor sets all properties
        /// </summary>
        /// <param name="notificationId">Setter for NotificationId property</param>
        /// <param name="notificationTitle">Setter for NotificationTitle property</param>
        /// <param name="notificationStatus">Setter for NotificationStatus property</param>
        public NotificationChangeStatusViewModel(int notificationId, string notificationTitle, string notificationStatus)
        {
            this.NotificationId = notificationId;

            this.NotificationTitle = notificationTitle;

            this.NotificationStatus = notificationStatus;
        }
    }
}
