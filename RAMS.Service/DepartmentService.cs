using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service.Interfaces
{
    /// <summary>
    /// DepartmentService implements Department specific services
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository DepartmentRepository;

        private readonly IAgentRepository AgentRepository;

        private readonly IPositionRepository PositionRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="departmentRepository">Parameter for setting DepartmentRepository</param>
        /// <param name="agentRepository">Parameter for setting AgentRepository</param>
        /// <param name="positionRepository">Parameter for setting PositionRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public DepartmentService(IDepartmentRepository departmentRepository, IAgentRepository agentRepository, IPositionRepository positionRepository, IUnitOfWork unitOfWork) 
        {
            this.DepartmentRepository = departmentRepository;

            this.AgentRepository = agentRepository;

            this.PositionRepository = positionRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the departments
        /// </summary>
        /// <returns>All the departments</returns>
        public IEnumerable<Department> GetAllDepartments()
        {
            return this.DepartmentRepository.GetAllDepartments();
        }

        /// <summary>
        /// Get department with matching id
        /// </summary>
        /// <param name="id">Id of the department to be compared with the context departments' data</param>
        /// <returns>Department with matching id</returns>
        public Department GetOneDepartmentById(int id)
        {
            return this.DepartmentRepository.GetOneByDepartmentId(id);
        }

        /// <summary>
        /// Get department of specific agent 
        /// </summary>
        /// <param name="id">Id of the agent who's department is being retrieved</param>
        /// <returns>Department of specific agent</returns>
        public Department GetOneDepartmentByAgentId(int id)
        {
            var agent = this.AgentRepository.GetOneByAgentId(id);

            if (agent != null)
            {
                return agent.Department;
            }

            return null;
        }
        #endregion

        /// <summary>
        /// Create new department
        /// </summary>
        /// <param name="department">Department to be created</param>
        public void CreateDepartment(Department department)
        {
            this.DepartmentRepository.Add(department);
        }

        /// <summary>
        /// Update existing department
        /// </summary>
        /// <param name="department">Department to be updated</param>
        public void UpdateDepartment(Department department)
        {
            this.DepartmentRepository.Update(department);
        }

        /// <summary>
        /// Delete existing department
        /// </summary>
        /// <param name="department">Department to be deleted</param>
        public void DeleteDepartment(Department department)
        {
            this.DepartmentRepository.Delete(department);
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
            catch (DbUpdateException ex)
            #pragma warning restore 0168
            {
                throw;
            }
        }
    }
}
