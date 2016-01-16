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
    /// ClientController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class ClientController : ApiController
    {
        private readonly IClientService ClientService;

        /// <summary>
        /// Controller that sets client service in order to access context resources
        /// </summary>
        /// <param name="clientService">Parameter for setting client service</param>
        public ClientController(IClientService clientService)
        {
            this.ClientService = clientService;
        }

        /// <summary>
        /// Get the list of all clients
        /// </summary>
        /// <returns>The list of all clients</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Client>))]
        public IHttpActionResult GetAllClients()
        {
            var clients = this.ClientService.GetAllClients();

            if (!Utilities.IsEmpty(clients))
            {
                return Ok(clients);
            }

            return NotFound();
        }

        /// <summary>
        /// Get a client by id
        /// </summary>
        /// <param name="id">Id of a client to be fetched</param>
        /// <returns>A client with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClient(int id)
        {
            if (id > 0)
            {
                var client = this.ClientService.GetOneClientById(id);

                if (client != null)
                {
                    return Ok(client);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get a client by username
        /// </summary>
        /// <param name="userName">Username of a client to be fetched</param>
        /// <returns>A client with matching username</returns>
        [HttpGet]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetOneClientByUsername(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var client = this.ClientService.GetOneClientByUserName(userName);

                if (client != null)
                {
                    return Ok(client);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new client
        /// </summary>
        /// <param name="client">A client to be created</param>
        /// <returns>The Uri of newly created client</returns>
        [HttpPost]
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client)
        {
            if (ModelState.IsValid)
            {
                this.ClientService.CreateClient(client);

                try
                {
                    this.ClientService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = client.ClientId }, client);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing client
        /// </summary>
        /// <param name="client">Client to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Client))]
        public IHttpActionResult PutClient(Client client)
        {
            if (ModelState.IsValid)
            {
                this.ClientService.UpdateClient(client);

                try
                {
                    this.ClientService.SaveChanges();

                    return Ok(client);
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

                    if (!this.ClientExists(client.ClientId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Block or unblock client by user name
        /// </summary>
        /// <param name="userName">User name of the client to be blocked or unblocked</param>
        /// <param name="block">Boolean indicating whether client should be blocked or unblocked</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Client))]
        public IHttpActionResult BlockUnblockClientByUserName(string userName, bool block)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var client = this.ClientService.GetOneClientByUserName(userName);

                if (client != null)
                {
                    if (block == true)
                    {
                        if (client.UserStatus != Enums.UserStatus.Blocked)
                        {
                            client.UserStatus = Enums.UserStatus.Blocked;
                        }
                        else
                        {
                            return Ok(client);
                        }
                    }
                    else
                    {
                        if (client.UserStatus != Enums.UserStatus.Active)
                        {
                            client.UserStatus = Enums.UserStatus.Active;
                        }
                        else
                        {
                            return Ok(client);
                        }
                    }

                    this.ClientService.UpdateClient(client);

                    try
                    {
                        this.ClientService.SaveChanges();
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

                        if (!this.ClientExists(client.ClientId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(client);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing client by id (Logical and physical)
        /// </summary>
        /// <param name="id">Id of the client to be deleted</param>
        /// <param name="physicalDelete">Boolean indicating whether delete is physical or logical</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClientById(int id, bool physicalDelete = false)
        {
            if (id > 0)
            {
                var client = this.ClientService.GetOneClientById(id);

                if (client != null)
                {
                    if (physicalDelete)
                    {
                        this.ClientService.DeleteClient(client);
                    }
                    else
                    {
                        client.UserStatus = Enums.UserStatus.Deleted;

                        this.ClientService.UpdateClient(client);
                    }

                    try
                    {
                        this.ClientService.SaveChanges();

                        if (client.UserStatus != Enums.UserStatus.Deleted)
                        {
                            client.UserStatus = Enums.UserStatus.Deleted;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.ClientExists(client.ClientId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(client);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing client by user name
        /// </summary>
        /// <param name="userName">User name of the client to be deleted</param>
        /// <param name="physicalDelete">Boolian indicating whether delete is physical or logical</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClientByUserName(string userName, bool physicalDelete = false)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var client = this.ClientService.GetOneClientByUserName(userName);

                if (client != null)
                {
                    if (physicalDelete)
                    {
                        this.ClientService.DeleteClient(client);
                    }
                    else
                    {
                        client.UserStatus = Enums.UserStatus.Deleted;

                        this.ClientService.UpdateClient(client);
                    }

                    try
                    {
                        this.ClientService.SaveChanges();

                        if (client.UserStatus != Enums.UserStatus.Deleted)
                        {
                            client.UserStatus = Enums.UserStatus.Deleted;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.ClientExists(client.ClientId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(client);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// ClientExists is used to check whether the client is present in data context
        /// </summary>
        /// <param name="id">Id of the client to check against</param>
        /// <returns>True if client is present in data context, false otherwise</returns>
        private bool ClientExists(int id)
        {
            return this.ClientService.GetAllClients().Count(c => c.ClientId == id) > 0;
        }
    }
}
