using RAMS.Data.Infrastructure;
using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Repositories
{
    /// <summary>
    /// CandidateArchive repository implements CandidateArchive specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class CandidateArchiveRepository : RepositoryBase<CandidateArchive>, ICandidateArchiveRepository
    {
        public CandidateArchiveRepository(IDataFactory dataFactory) : base(dataFactory) { }
    }
}
