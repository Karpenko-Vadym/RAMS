using RAMS.Helpers;
using RAMS.Models;
using RAMS.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RAMS.Web.Controllers.WebAPI
{
    /// <summary>
    /// DepartmentController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class DepartmentController : ApiController
    {
        private readonly IDepartmentService DepartmentService;

        /// <summary>
        /// Controller that sets department service in order to access context resources
        /// </summary>
        /// <param name="departmentService">Parameter for setting department service</param>
        public DepartmentController(IDepartmentService departmentService)
        {
            this.DepartmentService = departmentService;
        }

        /// <summary>
        /// Get the list of all departments
        /// </summary>
        /// <returns>The list of all departments</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Department>))]
        public IHttpActionResult GetAllDepartments()
        {
            var departments = this.DepartmentService.GetAllDepartments();

            if (!Utilities.IsEmpty(departments))
            {
                return Ok(departments);
            }

            return NotFound();
        }

        /// <summary>
        /// Get a department by id
        /// </summary>
        /// <param name="id">Id of a department to be fetched</param>
        /// <returns>A department with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Department))]
        public IHttpActionResult GetDepartment(int id)
        {
            if (id > 0)
            {
                var department = this.DepartmentService.GetOneDepartmentById(id);

                if (department != null)
                {
                    return Ok(department);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new department
        /// </summary>
        /// <param name="department">Department to be created</param>
        /// <returns>The Uri of newly created department</returns>
        [HttpPost]
        [ResponseType(typeof(Department))]
        public IHttpActionResult PostDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                this.DepartmentService.CreateDepartment(department);

                try
                {
                    this.DepartmentService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing department
        /// </summary>
        /// <param name="department">Department to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Department))]
        public IHttpActionResult PutDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                this.DepartmentService.UpdateDepartment(department);

                try
                {
                    this.DepartmentService.SaveChanges();

                    return Ok(department);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    if (!this.DepartmentExists(department.DepartmentId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete existing department
        /// </summary>
        /// <param name="id">Id of the department to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Department))]
        public IHttpActionResult DeleteDepartment(int id)
        {
            if (id > 0)
            {
                var department = this.DepartmentService.GetOneDepartmentById(id);

                if (department != null)
                {
                    this.DepartmentService.DeleteDepartment(department);

                    try
                    {
                        this.DepartmentService.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.DepartmentExists(department.DepartmentId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(department);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// DepartmentExists is used to check whether the department is present in data context
        /// </summary>
        /// <param name="id">Id of the department to check against</param>
        /// <returns>True if department is present in data context, false otherwise</returns>
        private bool DepartmentExists(int id)
        {
            return this.DepartmentService.GetAllDepartments().Count(d => d.DepartmentId == id) > 0;
        }
    }
}
