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
    /// Database attributes configuration for Category class
    /// </summary>
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public CategoryConfiguration()
        {
            ToTable("Categorys"); // Select which table will be associated with Category class

            /* Define configuration for each field (Only for fields that need to be configured) separately */

            Property(n => n.Name).IsRequired().HasMaxLength(200);
        }
    }
}
