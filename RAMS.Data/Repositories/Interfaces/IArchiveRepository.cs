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
    /// IArchiveRepository interface declares the Archive specific repository operations
    /// </summary>
    public interface IArchiveRepository : IRepository<Archive> { }
}
