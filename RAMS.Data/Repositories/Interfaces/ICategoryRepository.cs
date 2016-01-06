using RAMS.Data.Infrastructure;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Repositories
{
    /// <summary>
    /// ICategoryRepository interface declares the Category specific repository operations
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetOneByName(string name);

        Category GetOneByPosition(Position position);

        Category GetOneByPositionId(int id);
    }
}
