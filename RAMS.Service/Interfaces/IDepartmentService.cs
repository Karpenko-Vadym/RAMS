using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// IDepartmentService interface declares the Department specific service operations
    /// </summary>
    public interface IDepartmentService
    {
        IEnumerable<Department> GetAllDepartments();

        Department GetOneDepartmentById(int id);

        Department GetOneDepartmentByAgentId(int id);

        void CreateDepartment(Department department);

        void UpdateDepartment(Department department);

        void DeleteDepartment(Department department);

        void SaveChanges();
    }
}
