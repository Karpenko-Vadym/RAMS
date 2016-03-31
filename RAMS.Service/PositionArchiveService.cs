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
    /// PositionArchiveService implements PositionArchive specific services
    /// </summary>
    public class PositionArchiveService : IPositionArchiveService
    {
        private readonly IPositionArchiveRepository PositionArchiveRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="positionArchiveRepository">Parameter for setting PositionArchiveRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public PositionArchiveService(IPositionArchiveRepository positionArchiveRepository, IUnitOfWork unitOfWork)
        {
            this.PositionArchiveRepository = positionArchiveRepository;

            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Create new position archive
        /// </summary>
        /// <param name="positionArchive">PositionArchive to be created</param>
        public void CreateArchivePosition(PositionArchive positionArchive)
        {
            this.PositionArchiveRepository.Add(positionArchive);
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
