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
    public class NotificationListViewModel : BaseEntity
    {
        public int NotificationId { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
