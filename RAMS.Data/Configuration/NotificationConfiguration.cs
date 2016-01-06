using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Configuration
{
    /// <summary>
    /// Database attributes configuration for Notification class
    /// </summary>
    public class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public NotificationConfiguration()
        {
            ToTable("Notifications"); // Select which table will be associated with Notification class

            /* Define configuration for each field (Only for fields that need to be configured) separately */

            Property(n => n.Title).IsRequired().HasMaxLength(100);

            Property(n => n.Details).IsRequired().HasMaxLength(300);
        }
    }
}
