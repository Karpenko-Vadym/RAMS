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
    /// Database attributes configuration for Interview class
    /// </summary>
    public class InterviewConfiguration : EntityTypeConfiguration<Interview>
    {
        /// <summary>
        /// Default constructor that defines all the configurations
        /// </summary>
        public InterviewConfiguration()
        {
            ToTable("Interviews"); // Select which table will be associated with Interview class
        }
    }
}
