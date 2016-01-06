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
    /// Database attributes configuration for Client class
    /// </summary>
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public ClientConfiguration()
        {
            ToTable("Clients"); // Select which table will be associated with Client class

            /* Define configuration for each field (Only for fields that need to be configured) separately */

            Property(c => c.UserName).IsRequired().HasMaxLength(100)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_UserName") { IsUnique = true })); // Unique Constraint

            Property(c => c.UserType).IsRequired();

            Property(c => c.Role).IsRequired();

            Property(c => c.UserStatus).IsRequired();

            Property(c => c.FirstName).IsRequired().HasMaxLength(200);

            Property(c => c.LastName).IsRequired().HasMaxLength(200);

            Property(c => c.JobTitle).IsRequired().HasMaxLength(100);

            Property(c => c.Company).IsRequired().HasMaxLength(200);

            Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);

            Property(c => c.Email).IsRequired().HasMaxLength(300)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Email") { IsUnique = true })); // Unique Constraint
        }
    }
}
