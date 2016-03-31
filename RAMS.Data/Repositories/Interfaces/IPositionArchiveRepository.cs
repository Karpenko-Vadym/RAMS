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
    /// IPositionArchiveRepository interface declares the PositionArchive specific repository operations
    /// </summary>
    public interface IPositionArchiveRepository : IRepository<PositionArchive> { }
}
