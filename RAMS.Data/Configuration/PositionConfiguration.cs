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
    /// Database attributes configuration for Position class
    /// </summary>
    public class PositionConfiguration : EntityTypeConfiguration<Position>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public PositionConfiguration()
        {
            ToTable("Positions"); // Select which table will be associated with Position class

            /* Define configuration for each field (Only for fields that need to be configured) separately */

            Property(p => p.Title).IsRequired().HasMaxLength(100);

            Property(p => p.Description).IsRequired().HasMaxLength(2000);

            Property(p => p.CompanyDetails).IsRequired().HasMaxLength(1000);

            Property(p => p.Location).IsRequired().HasMaxLength(200);

            Property(p => p.Qualifications).IsRequired();

            Property(p => p.PeopleNeeded).IsRequired();

            Property(p => p.AcceptanceScore).IsRequired();
        }
    }
}
