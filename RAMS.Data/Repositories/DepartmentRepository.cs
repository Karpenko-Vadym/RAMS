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
    /// Department repository implements Department specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get first Department with matching name
        /// </summary>
        /// <param name="name">Name for comparing with Departments' name</param>
        /// <returns>First Department with matching name</returns>
        public Department GetOneByName(string name)
        {
            return this.GetContext.Departments.Where(d => d.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Get first Department with matching agent id
        /// </summary>
        /// <param name="id">Id of the agent who's departments is being returned</param>
        /// <returns>First Department with matching agent id</returns>
        public Department GetOneByAgentId(int id)
        {
            var agent = this.GetContext.Agents.Where(a => a.AgentId == id).FirstOrDefault() ?? new Agent();

            return this.GetContext.Departments.Where(d => d.DepartmentId == agent.DepartmentId).FirstOrDefault();
        }

        /// <summary>
        /// Get first Department with matching agent
        /// </summary>
        /// <param name="agent">Agent who's departments is being returned</param>
        /// <returns>First Department with matching agent</returns>
        public Department GetOneByAgent(Agent agent)
        {
            return this.GetContext.Departments.Where(d => d.DepartmentId == agent.DepartmentId).FirstOrDefault();
        }
        #endregion
    }
}
