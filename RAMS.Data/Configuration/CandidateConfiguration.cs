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
    /// Database attributes configuration for Candidate class
    /// </summary>
    public class CandidateConfiguration : EntityTypeConfiguration<Candidate>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public CandidateConfiguration()
        {
            ToTable("Candidates"); // Select which table will be associated with Candidate class

            /* Define configuration for each field (Only for fields that need to be configured) separately */

            Property(c => c.FirstName).IsRequired().HasMaxLength(200);

            Property(c => c.LastName).IsRequired().HasMaxLength(200);

            Property(c => c.Email).IsRequired().HasMaxLength(300);

            Property(c => c.Country).IsRequired().HasMaxLength(100);

            Property(c => c.PostalCode).IsRequired().HasMaxLength(10);

            Property(c => c.PhoneNumber).IsOptional().HasMaxLength(20);

            Property(c => c.Feedback).IsOptional().HasMaxLength(1000);

            Property(c => c.Score).IsOptional();
        }
    }
}
