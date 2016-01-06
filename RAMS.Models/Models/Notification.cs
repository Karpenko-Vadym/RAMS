using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    /// <summary>
    /// Notification class represents a message that employee gets from the system when certain event is triggered
    /// </summary>
    public class Notification : BaseEntity
    {
        public int NotificationId { get; set; }

        public int? AgentId { get; set; }

        public int? ClientId { get; set; }

        public int? AdminId { get; set; }

        public virtual Agent Agent { get; set; }

        public virtual Client Client { get; set; }

        public virtual Admin Admin { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTime DateCreated { get; set; }
    }
}