using RAMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Configuration
{
    /// <summary>
    /// Database attributes configuration for Department class
    /// </summary>
    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public DepartmentConfiguration()
        {
            ToTable("Departments"); // Select which table will be associated with Department class

            /* Define configuration for each field (Only for fields that need to be configured) separately */

            Property(d => d.Name).IsRequired().HasMaxLength(150);
        }
    }
}
