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
    /// Database attributes configuration for Agent class
    /// </summary>
    public class AgentConfiguration : EntityTypeConfiguration<Agent>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public AgentConfiguration()
        {
            ToTable("Agents"); // Select which table will be associated with Agent class

            /* Define configuration for each field (Only for fields that need to be configured) separately */

            Property(a => a.UserName).IsRequired().HasMaxLength(100)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_UserName") { IsUnique = true })); // Unique Constraint

            Property(a => a.UserType).IsRequired();

            Property(a => a.Role).IsRequired();

            Property(c => c.UserStatus).IsRequired();

            Property(a => a.FirstName).IsRequired().HasMaxLength(200);

            Property(a => a.LastName).IsRequired().HasMaxLength(200);

            Property(a => a.JobTitle).IsRequired().HasMaxLength(100);

            Property(a => a.Company).IsRequired().HasMaxLength(200);

            Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);

            Property(a => a.Email).IsRequired().HasMaxLength(300)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Email") { IsUnique = true })); // Unique Constraint
        }
    }
}
