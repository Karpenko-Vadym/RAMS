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

    /// <summary>
    /// NotificationAddViewModel view model declares properties required in order to create new notification
    /// </summary>
    public class NotificationAddViewModel
    {
        public int? AgentId { get; set; }

        public int? ClientId { get; set; }

        public int? AdminId { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Default NotificationAddViewModel constructor
        /// </summary>
        public NotificationAddViewModel() 
        {
            this.Status = NotificationStatus.Unread;

            this.DateCreated = DateTime.Now;
        }

        /// <summary>
        /// NotificationAddViewModel constructor that sets all of its properties
        /// </summary>
        /// <param name="title">Setter for Title</param>
        /// <param name="details">Setter for Details</param>
        /// <param name="agentId">Setter for AgentId</param>
        /// <param name="clientId">Setter for ClientId</param>
        /// <param name="adminId">Setter for AdminId</param>
        public NotificationAddViewModel(string title, string details, int? agentId = null, int? clientId = null, int? adminId = null)
        {
            this.AgentId = agentId;

            this.ClientId = clientId;

            this.AdminId = adminId;

            this.Title = title;

            this.Details = details;

            this.Status = NotificationStatus.Unread;

            this.DateCreated = DateTime.Now;
        }
    }
}
