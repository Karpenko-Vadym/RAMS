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
    /// AgentController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class AgentController : ApiController
    {
        private readonly IAgentService AgentService;
        private readonly IDepartmentService DepartmentService;

        /// <summary>
        /// Controller that sets agent service in order to access context resources
        /// </summary>
        /// <param name="agentService">Parameter for setting agent service</param>
        /// <param name="departmentService">Parameter for setting department service</param>
        public AgentController(IAgentService agentService, IDepartmentService departmentService)
        {
            this.AgentService = agentService;

            this.DepartmentService = departmentService;
        }

        /// <summary>
        /// Get the list of all agents
        /// </summary>
        /// <returns>The list of all agents</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Agent>))]
        public IHttpActionResult GetAllAgents()
        {
            var agents = this.AgentService.GetAllAgents();

            if(agents.Count() > 0)
            {
                return Ok(agents);
            }

            return NotFound();
        }

        /// <summary>
        /// Get an agent by id
        /// </summary>
        /// <param name="id">Id of an agent to be fetched</param>
        /// <returns>An agent with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Agent))]
        public IHttpActionResult GetAgent(int id)
        {
            if (id > 0)
            {
                var agent = this.AgentService.GetOneAgentById(id);

                if (agent != null)
                {
                    return Ok(agent);
                } 
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of all agents that belong to specific department (Agent.DepartmentID = deartmentId)
        /// </summary>
        /// <param name="departmentId">Department id for which all the agents are to be fetched</param>
        /// <returns>List of the agents that belong to specific department</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Agent>))]
        public IHttpActionResult GetManyAgentsByDepartmentId(int departmentId)
        {
            if (departmentId > 0)
            {
                var agents = this.AgentService.GetManyAgentsByDepartmentId(departmentId);

                if (agents.Count() > 0)
                {
                    return Ok(agents);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get an agent by username
        /// </summary>
        /// <param name="userName">Username of an agent to be fetched</param>
        /// <returns>An agent with matching username</returns>
        [HttpGet]
        [ResponseType(typeof(Agent))]
        public IHttpActionResult GetOneAgentByUsername(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var agent = this.AgentService.GetOneAgentByUserName(userName);

                if (agent != null)
                {
                    return Ok(agent);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new agent
        /// </summary>
        /// <param name="agent">An agent to be created</param>
        /// <returns>The Uri of newly created agent</returns>
        [HttpPost]
        [ResponseType(typeof(Agent))]
        public IHttpActionResult PostAgent(Agent agent)
        {
            if (ModelState.IsValid)
            {
                this.AgentService.CreateAgent(agent);

                try
                {
                    this.AgentService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                agent.Department = this.DepartmentService.GetOneDepartmentById(agent.DepartmentId);

                return CreatedAtRoute("DefaultApi", new { id = agent.AgentId }, agent);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Update existing agent
        /// </summary>
        /// <param name="agent">Agent to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Agent))]
        public IHttpActionResult PutAgent(Agent agent)
        {
            if (ModelState.IsValid)
            {
                this.AgentService.UpdateAgent(agent);

                try
                {
                    this.AgentService.SaveChanges();

                    return Ok(agent);
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

                    if(!this.AgentExists(agent.AgentId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
                
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Block or unblock agent by user name
        /// </summary>
        /// <param name="userName">User name of the agent to be blocked or unblocked</param>
        /// <param name="block">Boolean indicating whether agent should be blocked or unblocked</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Agent))]
        public IHttpActionResult BlockUnblockAgentByUserName(string userName, bool block)
        {
            if(!String.IsNullOrEmpty(userName))
            {
                var agent = this.AgentService.GetOneAgentByUserName(userName);

                if (agent != null)
                {
                    if (block == true)
                    {
                        if (agent.UserStatus != Enums.UserStatus.Blocked)
                        {
                            agent.UserStatus = Enums.UserStatus.Blocked;
                        }
                        else
                        {
                            return Ok(agent);
                        }
                    }
                    else
                    {
                        if (agent.UserStatus != Enums.UserStatus.Active)
                        {
                            agent.UserStatus = Enums.UserStatus.Active;
                        }
                        else
                        {
                            return Ok(agent);
                        }
                    }

                    this.AgentService.UpdateAgent(agent);

                    try
                    {
                        this.AgentService.SaveChanges();
                    }
                    catch(DbUpdateConcurrencyException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        return Conflict();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.AgentExists(agent.AgentId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(agent);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing agent by id (Logical and physical)
        /// </summary>
        /// <param name="id">Id of the agent to be deleted</param>
        /// <param name="physicalDelete">Boolean indicating whether delete is physical or logical</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Agent))]
        public IHttpActionResult DeleteAgentById(int id, bool physicalDelete = false)
        {
            if (id > 0)
            {
                var agent = this.AgentService.GetOneAgentById(id);

                if (agent != null)
                {
                    if (physicalDelete)
                    {
                        this.AgentService.DeleteAgent(agent);
                    }
                    else
                    {
                        agent.UserStatus = Enums.UserStatus.Deleted;

                        this.AgentService.UpdateAgent(agent);
                    }

                    try
                    {
                        this.AgentService.SaveChanges();

                        if (agent.UserStatus != Enums.UserStatus.Deleted)
                        {
                            agent.UserStatus = Enums.UserStatus.Deleted;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.AgentExists(agent.AgentId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(agent);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing agent by user name
        /// </summary>
        /// <param name="userName">User name of the agent to be deleted</param>
        /// <param name="physicalDelete">Boolian indicating whether delete is physical or logical</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Agent))]
        public IHttpActionResult DeleteAgentByUserName(string userName, bool physicalDelete = false)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var agent = this.AgentService.GetOneAgentByUserName(userName);

                if (agent != null)
                {
                    if (physicalDelete)
                    {
                        this.AgentService.DeleteAgent(agent);
                    }
                    else
                    {
                        agent.UserStatus = Enums.UserStatus.Deleted;

                        this.AgentService.UpdateAgent(agent);
                    }

                    try
                    {
                        this.AgentService.SaveChanges();

                        if(agent.UserStatus != Enums.UserStatus.Deleted)
                        {
                            agent.UserStatus = Enums.UserStatus.Deleted;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.AgentExists(agent.AgentId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(agent);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// AgentExists is used to check whether the agent is present in data context
        /// </summary>
        /// <param name="id">Id of the agent to check against</param>
        /// <returns>True if agent is present in data context, false otherwise</returns>
        private bool AgentExists(int id)
        {
            return this.AgentService.GetAllAgents().Count(a => a.AgentId == id) > 0;
        }
    }
}
