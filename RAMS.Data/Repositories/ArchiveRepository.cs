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
    /// Archive repository implements Archive specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class ArchiveRepository : RepositoryBase<Archive>, IArchiveRepository
    {
        public ArchiveRepository(IDataFactory dataFactory) : base(dataFactory) { }
    }
}
