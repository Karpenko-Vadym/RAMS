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
    /// NotificationController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class NotificationController : ApiController
    {
        private readonly INotificationService NotificationService;
        private readonly IAgentService AgentService;
        private readonly IClientService ClientService;
        private readonly IAdminService AdminService;

        /// <summary>
        /// Controller that sets notification service in order to access context resources
        /// </summary>
        /// <param name="notificationService">Parameter for setting NotificationService</param>
        /// <param name="agentService">Parameter for setting AgentService</param>
        /// <param name="clientService">Parameter for setting ClientService</param>
        /// <param name="adminService">Parameter for setting AdminService</param>
        public NotificationController(INotificationService notificationService, IAgentService agentService, IClientService clientService, IAdminService adminService)
        {
            this.NotificationService = notificationService;

            this.AgentService = agentService;

            this.ClientService = clientService;

            this.AdminService = adminService;
        }

        /// <summary>
        /// Get the list of all notifications
        /// </summary>
        /// <returns>The list of all notifications</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Notification>))]
        public IHttpActionResult GetAllNotifications()
        {
            var notifications = this.NotificationService.GetAllNotifications();

            if (!Utilities.IsEmpty(notifications))
            {
                return Ok(notifications);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of notifications for specific agent
        /// </summary>
        /// <param name="agentUserName">User name of the agent whos notification are being fetched</param>
        /// <returns>The list of notifications for specific agent</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Notification>))]
        public IHttpActionResult GetManyNotificationsByAgentUserName(string agentUserName)
        {
            var agent = this.AgentService.GetOneAgentByUserName(agentUserName);

            if (agent != null)
            {
                if (!Utilities.IsEmpty(agent.Notifications))
                {
                    return Ok(agent.Notifications);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of notifications for specific client
        /// </summary>
        /// <param name="clientUserName">User name of the client whos notification are being fetched</param>
        /// <returns>The list of notifications for specific client</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Notification>))]
        public IHttpActionResult GetManyNotificationsByClientUserName(string clientUserName)
        {
            var client = this.ClientService.GetOneClientByUserName(clientUserName);

            if (client != null)
            {
                if (!Utilities.IsEmpty(client.Notifications))
                {
                    return Ok(client.Notifications);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of notifications for specific admin
        /// </summary>
        /// <param name="adminUserName">User name of the admin whos notification are being fetched</param>
        /// <returns>The list of notifications for specific admin</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Notification>))]
        public IHttpActionResult GetManyNotificationsByAdminUserName(string adminUserName)
        {
            var admin = this.AdminService.GetOneAdminByUserName(adminUserName);

            if (admin != null)
            {
                if (!Utilities.IsEmpty(admin.Notifications))
                {
                    return Ok(admin.Notifications);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get a notification by id
        /// </summary>
        /// <param name="id">Id of a notification to be fetched</param>
        /// <returns>A notification with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Notification))]
        public IHttpActionResult GetNotification(int id)
        {
            if (id > 0)
            {
                var notification = this.NotificationService.GetOneNotificationById(id);

                if (notification != null)
                {
                    return Ok(notification);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new notification
        /// </summary>
        /// <param name="notification">Notification to be created</param>
        /// <returns>The Uri of newly created notification</returns>
        [HttpPost]
        [ResponseType(typeof(Notification))]
        public IHttpActionResult PostNotification(Notification notification)
        {
            if (ModelState.IsValid)
            {
                this.NotificationService.CreateNotification(notification);

                try
                {
                    this.NotificationService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = notification.NotificationId }, notification);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Create new notification when username is provided instead of agent id
        /// </summary>
        /// <param name="agentUsername">Username of the agent to whom notification will be assigned</param>
        /// <param name="notification">Notification to be created</param>
        /// <returns>The Uri of newly created notification</returns>
        [HttpPost]
        [ResponseType(typeof(Notification))]
        public IHttpActionResult PostNotificationByAgentUsername(string agentUsername, Notification notification)
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(agentUsername))
            {
                var agent = this.AgentService.GetOneAgentByUserName(agentUsername);

                notification.AgentId = agent.AgentId;

                this.NotificationService.CreateNotification(notification);

                try
                {
                    this.NotificationService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = notification.NotificationId }, notification);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Create new notification when username is provided instead of admin id
        /// </summary>
        /// <param name="adminUsername">Username of the admin to whom notification will be assigned</param>
        /// <param name="notification">Notification to be created</param>
        /// <returns>The Uri of newly created notification</returns>
        [HttpPost]
        [ResponseType(typeof(Notification))]
        public IHttpActionResult PostNotificationByAdminUsername(string adminUsername, Notification notification)
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(adminUsername))
            {
                var admin = this.AdminService.GetOneAdminByUserName(adminUsername);

                notification.AdminId = admin.AdminId;

                this.NotificationService.CreateNotification(notification);

                try
                {
                    this.NotificationService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = notification.NotificationId }, notification);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing notification
        /// </summary>
        /// <param name="notification">Notification to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Notification))]
        public IHttpActionResult PutNotification(Notification notification)
        {
            if (ModelState.IsValid)
            {
                this.NotificationService.UpdateNotification(notification);

                try
                {
                    this.NotificationService.SaveChanges();

                    return Ok(notification);
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

                    if (!this.NotificationExists(notification.NotificationId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update status of the existing notification
        /// </summary>
        /// <param name="id">Id of the notification to be updated</param>
        /// <param name="isReadStatus">Status of the notification depends on the calue of isReadStatus (If isReadStatus is true, status will be set to Read. Otherwise, it will be set to Unread)</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Notification))]
        public IHttpActionResult ChangeNotificationStatus(int id, bool isReadStatus = false)
        {
            if (id > 0)
            {
                var notification = this.NotificationService.GetOneNotificationById(id);

                if (notification != null)
                {
                    if (isReadStatus)
                    {
                        if(notification.Status != Enums.NotificationStatus.Read)
                        {
                            notification.Status = Enums.NotificationStatus.Read;
                        }
                    }
                    else
                    {
                        if (notification.Status != Enums.NotificationStatus.Unread)
                        {
                            notification.Status = Enums.NotificationStatus.Unread;
                        }
                    }

                    this.NotificationService.UpdateNotification(notification);

                    try
                    {
                        this.NotificationService.SaveChanges();   
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

                        if (!this.NotificationExists(notification.NotificationId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(notification);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing notification
        /// </summary>
        /// <param name="id">Id of the notification to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Notification))]
        public IHttpActionResult DeleteNotification(int id)
        {
            if (id > 0)
            {
                var notification = this.NotificationService.GetOneNotificationById(id);

                if (notification != null)
                {
                    this.NotificationService.DeleteNotification(notification);

                    try
                    {
                        this.NotificationService.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.NotificationExists(notification.NotificationId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(notification);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// NotificationExists is used to check whether the notification is present in data context
        /// </summary>
        /// <param name="id">Id of the notification to check against</param>
        /// <returns>True if notification is present in data context, false otherwise</returns>
        private bool NotificationExists(int id)
        {
            return this.NotificationService.GetAllNotifications().Count(n => n.NotificationId == id) > 0;
        }
    }
}
