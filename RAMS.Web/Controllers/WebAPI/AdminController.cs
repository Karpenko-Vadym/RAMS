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
    /// AdminController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class AdminController : ApiController
    {
        private readonly IAdminService AdminService;

        /// <summary>
        /// Controller that sets admin service in order to access context resources
        /// </summary>
        /// <param name="adminService">Parameter for setting admin service</param>
        public AdminController(IAdminService adminService)
        {
            this.AdminService = adminService;
        }

        /// <summary>
        /// Get the list of all admins
        /// </summary>
        /// <returns>The list of all admins</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Admin>))]
        public IHttpActionResult GetAllAdmins()
        {
            var admins = this.AdminService.GetAllAdmins();

            if (admins.Count() > 0)
            {
                return Ok(admins);
            }

            return NotFound();
        }

        /// <summary>
        /// Get an admin by id
        /// </summary>
        /// <param name="id">Id of an admin to be fetched</param>
        /// <returns>An admin with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult GetAdmin(int id)
        {
            if (id > 0)
            {
                var admin = this.AdminService.GetOneAdminById(id);

                if (admin != null)
                {
                    return Ok(admin);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get an admin by username
        /// </summary>
        /// <param name="userName">Username of an admin to be fetched</param>
        /// <returns>An admin with matching username</returns>
        [HttpGet]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult GetOneAdminByUsername(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var admin = this.AdminService.GetOneAdminByUserName(userName);

                if (admin != null)
                {
                    return Ok(admin);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new admin
        /// </summary>
        /// <param name="admin">An admin to be created</param>
        /// <returns>The Uri of newly created admin</returns>
        [HttpPost]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult PostAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                this.AdminService.CreateAdmin(admin);

                try
                {
                    this.AdminService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = admin.AdminId }, admin);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing admin
        /// </summary>
        /// <param name="admin">Admin to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult PutAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                this.AdminService.UpdateAdmin(admin);

                try
                {
                    this.AdminService.SaveChanges();

                    return Ok(admin);
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

                    if (!this.AdminExists(admin.AdminId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Block or unblock admin by user name
        /// </summary>
        /// <param name="userName">User name of the admin to be blocked or unblocked</param>
        /// <param name="block">Boolean indicating whether admin should be blocked or unblocked</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult BlockUnblockAdminByUserName(string userName, bool block)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var admin = this.AdminService.GetOneAdminByUserName(userName);

                if (admin != null)
                {
                    if (block == true)
                    {
                        if (admin.UserStatus != Enums.UserStatus.Blocked)
                        {
                            admin.UserStatus = Enums.UserStatus.Blocked;
                        }
                        else
                        {
                            return Ok(admin);
                        }
                    }
                    else
                    {
                        if (admin.UserStatus != Enums.UserStatus.Active)
                        {
                            admin.UserStatus = Enums.UserStatus.Active;
                        }
                        else
                        {
                            return Ok(admin);
                        }
                    }

                    this.AdminService.UpdateAdmin(admin);

                    try
                    {
                        this.AdminService.SaveChanges();
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

                        if (!this.AdminExists(admin.AdminId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(admin);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing admin by id (Logical and physical)
        /// </summary>
        /// <param name="id">Id of the admin to be deleted</param>
        /// <param name="physicalDelete">Boolean indicating whether delete is physical or logical</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult DeleteAdminById(int id, bool physicalDelete = false)
        {
            if (id > 0)
            {
                var admin = this.AdminService.GetOneAdminById(id);

                if (admin != null)
                {
                    if (physicalDelete)
                    {
                        this.AdminService.DeleteAdmin(admin);
                    }
                    else
                    {
                        admin.UserStatus = Enums.UserStatus.Deleted;

                        this.AdminService.UpdateAdmin(admin);
                    }

                    try
                    {
                        this.AdminService.SaveChanges();

                        if (admin.UserStatus != Enums.UserStatus.Deleted)
                        {
                            admin.UserStatus = Enums.UserStatus.Deleted;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.AdminExists(admin.AdminId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(admin);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing admin by user name
        /// </summary>
        /// <param name="userName">User name of the admin to be deleted</param>
        /// <param name="physicalDelete">Boolian indicating whether delete is physical or logical</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Admin))]
        public IHttpActionResult DeleteAdminByUserName(string userName, bool physicalDelete = false)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var admin = this.AdminService.GetOneAdminByUserName(userName);

                if (admin != null)
                {
                    if (physicalDelete)
                    {
                        this.AdminService.DeleteAdmin(admin);
                    }
                    else
                    {
                        admin.UserStatus = Enums.UserStatus.Deleted;

                        this.AdminService.UpdateAdmin(admin);
                    }

                    try
                    {
                        this.AdminService.SaveChanges();

                        if (admin.UserStatus != Enums.UserStatus.Deleted)
                        {
                            admin.UserStatus = Enums.UserStatus.Deleted;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.AdminExists(admin.AdminId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(admin);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// AdminExists is used to check whether the admin is present in data context
        /// </summary>
        /// <param name="id">Id of the admin to check against</param>
        /// <returns>True if admin is present in data context, false otherwise</returns>
        private bool AdminExists(int id)
        {
            return this.AdminService.GetAllAdmins().Count(a => a.AdminId == id) > 0;
        }
    }
}
