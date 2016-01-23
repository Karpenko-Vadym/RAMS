using Microsoft.AspNet.Identity.EntityFramework;
using RAMS.Data.Configuration;
using RAMS.Helpers;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data
{
    /// <summary>
    /// DataContext class is responsible for database access
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext() : base("RAMSConnection") { } // Pass the name of the connection to the base (DbContext) class

        public DbSet<Candidate> Candidates { get; set; } // Database setter and getter for Candidate class

        public DbSet<Category> Categories { get; set; } // Database setter and getter for Category class

        public DbSet<Department> Departments { get; set; } // Database setter and getter for Employee class

        public DbSet<Admin> Admins { get; set; } // Database setter and getter for Admin class

        public DbSet<Agent> Agents { get; set; } // Database setter and getter for Agent class

        public DbSet<Client> Clients { get; set; } // Database setter and getter for Client class

        public DbSet<Interview> Interviews { get; set; } // Database setter and getter for Interview class

        public DbSet<Notification> Notifications { get; set; } // Database setter and getter for Notification class

        public DbSet<Position> Positions { get; set; } // Database setter and getter for Position class

        /// <summary>
        /// Commit method allows to commit changes to database (Without commit, changes will NOT be persisted to database)
        /// </summary>
        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateConcurrencyException ex)
            #pragma warning restore 0168 
            {
                throw;
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateException ex)
            #pragma warning restore 0168 
            {
                throw;
            }
            catch(DbEntityValidationException ex)
            {
                // Extract properties and related errors and log them
                var exceptionInfo = "";

                if(!Utilities.IsEmpty(ex.EntityValidationErrors))
                {
                    foreach(var errors in ex.EntityValidationErrors)
                    {
                        if(!Utilities.IsEmpty(errors.ValidationErrors))
                        {
                            foreach(var error in errors.ValidationErrors)
                            {
                                exceptionInfo += String.Format("Property: {0} Error: {1}" + Environment.NewLine, error.PropertyName, error.ErrorMessage);
                            }
                        }
                    }
                }

                // Log exception
                ErrorHandlingUtilities.LogException(ex.ToString(), null, null, exceptionInfo);
            }
        }

        /// <summary>
        /// OnModelCreating method gives an access to ModelBuilder instance which allows to configure the model
        /// </summary>
        /// <param name="modelBuilder">Instance of a ModelBuilder that allows to configure the model</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CandidateConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new AdminConfiguration());
            modelBuilder.Configurations.Add(new AgentConfiguration());
            modelBuilder.Configurations.Add(new ClientConfiguration());
            modelBuilder.Configurations.Add(new InterviewConfiguration());
            modelBuilder.Configurations.Add(new NotificationConfiguration());
            modelBuilder.Configurations.Add(new PositionConfiguration());

            /* Set up table relations */

            modelBuilder.Entity<Candidate>().HasRequired(c => c.Position).WithMany(p => p.Candidates).HasForeignKey(c => c.PositionId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Candidate>().HasMany(c => c.Interviews).WithRequired(i => i.Candidate).HasForeignKey(c => c.CandidateId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>().HasMany(c => c.Positions).WithRequired(p => p.Category).HasForeignKey(c => c.CategoryId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>().HasMany(d => d.Agents).WithRequired(a => a.Department).HasForeignKey(d => d.DepartmentId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Interview>().HasRequired(i => i.Interviewer).WithMany(a => a.Interviews).HasForeignKey(i => i.InterviewerId).WillCascadeOnDelete(true); ;

            modelBuilder.Entity<Notification>().HasOptional(n => n.Client).WithMany(c => c.Notifications).HasForeignKey(n => n.ClientId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Notification>().HasOptional(n => n.Agent).WithMany(a => a.Notifications).HasForeignKey(n => n.AgentId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Notification>().HasOptional(n => n.Admin).WithMany(a => a.Notifications).HasForeignKey(n => n.AdminId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Position>().HasOptional(p => p.Agent).WithMany(a => a.Positions).HasForeignKey(p => p.AgentId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Position>().HasRequired(p => p.Client).WithMany(a => a.Positions).HasForeignKey(p => p.CleintId).WillCascadeOnDelete(true);
        }
    }
}
