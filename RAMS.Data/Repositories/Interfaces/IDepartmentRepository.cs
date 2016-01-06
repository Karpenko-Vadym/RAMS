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
    /// IDepartmentRepository interface declares the Department specific repository operations
    /// </summary>
    public interface IDepartmentRepository : IRepository<Department> 
    {
        Department GetOneByName(string name);

        Department GetOneByAgentId(int id);

        Department GetOneByAgent(Agent agent);
    }
}
