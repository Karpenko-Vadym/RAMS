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
    /// CandidateArchiveService implements CandidateArchive specific services
    /// </summary>
    public class CandidateArchiveService : ICandidateArchiveService
    {
        private readonly ICandidateArchiveRepository CandidateArchiveRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="candidateArchiveRepository">Parameter for setting CandidateArchiveRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public CandidateArchiveService(ICandidateArchiveRepository candidateArchiveRepository, IUnitOfWork unitOfWork) 
        {
            this.CandidateArchiveRepository = candidateArchiveRepository;

            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Create new candidate archive
        /// </summary>
        /// <param name="candidateArchive">CandidateArchive to be created</param>
        public void CreateCandidateArchive(CandidateArchive candidateArchive)
        {
            this.CandidateArchiveRepository.Add(candidateArchive);
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
