using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// ArchiveService implements Archive specific services
    /// </summary>
    public class ArchiveService : IArchiveService
    {
        private readonly IArchiveRepository ArchiveRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="archiveRepository">Parameter for setting ArchiveRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public ArchiveService(IArchiveRepository archiveRepository, IUnitOfWork unitOfWork)
        {
            this.ArchiveRepository = archiveRepository;

            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Create new archive
        /// </summary>
        /// <param name="archive">Archive to be created</param>
        public void CreateArchivePosition(Archive archive)
        {
            this.ArchiveRepository.Add(archive);
        }

        /// <summary>
        /// Save changes by using UnitOfWork's Commit method
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                this.UnitOfWork.Commit();
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
        }
    }
}
