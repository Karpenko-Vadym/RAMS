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

        /// <summary>
        /// Controller that sets notification service in order to access context resources
        /// </summary>
        /// <param name="notificationService">Parameter for setting notification service</param>
        public NotificationController(INotificationService notificationService)
        {
            this.NotificationService = notificationService;
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

            if (notifications.Count() > 0)
            {
                return Ok(notifications);
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
