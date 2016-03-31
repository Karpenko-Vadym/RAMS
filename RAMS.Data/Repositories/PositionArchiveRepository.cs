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
    /// PositionArchive repository implements PositionArchive specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class PositionArchiveRepository : RepositoryBase<PositionArchive>, IPositionArchiveRepository
    {
        public PositionArchiveRepository(IDataFactory dataFactory) : base(dataFactory) { }
    }
}
