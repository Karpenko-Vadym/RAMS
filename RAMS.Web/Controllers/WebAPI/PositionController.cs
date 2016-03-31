using AutoMapper;
using RAMS.Enums;
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
    /// PositionController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class PositionController : ApiController
    {
        private readonly IPositionService PositionService;
        private readonly IAgentService AgentService;
        private readonly ICandidateService CandidateService;
        private readonly IInterviewService InterviewService;
        private readonly IPositionArchiveService PositionArchiveService;
        private readonly ICandidateArchiveService CandidateArchiveService;

        /// <summary>
        /// Controller that sets position service in order to access context resources
        /// </summary>
        /// <param name="positionService">Parameter for setting position service</param>
        /// <param name="agentService">Parameter for setting agent service</param>
        /// <param name="candidateService">Parameter for setting candidate service</param>
        /// <param name="interviewService">Parameter for setting interview service</param>
        /// <param name="archiveService">Parameter for setting archive service</param>
        /// <param name="candidateArchiveService">Parameter for setting candidate archive service</param>
        public PositionController(IPositionService positionService, IAgentService agentService, ICandidateService candidateService, IInterviewService interviewService, IPositionArchiveService positionArchiveService, ICandidateArchiveService candidateArchiveService)
        {
            this.PositionService = positionService;
            this.AgentService = agentService;
            this.CandidateService = candidateService;
            this.InterviewService = interviewService;
            this.PositionArchiveService = positionArchiveService;
            this.CandidateArchiveService = candidateArchiveService;
        }

        /// <summary>
        /// Get the list of all positions
        /// </summary>
        /// <returns>The list of all positions</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Position>))]
        public IHttpActionResult GetAllPositions()
        {
            var positions = this.PositionService.GetAllPositions();

            if (!Utilities.IsEmpty(positions))
            {
                return Ok(positions);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of all positions for specific client
        /// </summary>
        /// <param name="clientName">User name of the client who's positions are being retrieved</param>
        /// <returns>The list of all positions for specific client</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Position>))]
        public IHttpActionResult GetAllPositionsForClient(string clientName)
        {
            var positions = this.PositionService.GetManyPositionsByClientName(clientName);

            if (!Utilities.IsEmpty(positions))
            {
                return Ok(positions);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of all positions for specific agent
        /// </summary>
        /// <param name="agentName">User name of the agent who's positions are being retrieved</param>
        /// <returns>The list of all positions for specific agent</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Position>))]
        public IHttpActionResult GetAllPositionsForAgent(string agentName)
        {
            var positions = this.PositionService.GetManyPositionsByAgentName(agentName);

            if (!Utilities.IsEmpty(positions))
            {
                return Ok(positions);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of multiple positions with specific category
        /// </summary>
        /// <param name="categoryName">Category for which positions are being retrieved</param>
        /// <returns>The list of multiple positions with specific category</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Position>))]
        public IHttpActionResult GetManyPositionByCategoryName(string categoryName)
        {
            var positions = this.PositionService.GetManyPositionsByCategoryName(categoryName);

            if (!Utilities.IsEmpty(positions))
            {
                return Ok(positions);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of multiple positions that match the keyword
        /// </summary>
        /// <param name="keyword">Keyword to match with positions' data</param>
        /// <returns>The list of multiple positions that match the keyword</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Position>))]
        public IHttpActionResult GetManyPositionByKeyword(string keyword)
        {
            var positions = this.PositionService.GetManyPositionsByKeyword(keyword);

            if (!Utilities.IsEmpty(positions))
            {
                return Ok(positions);
            }

            return NotFound();
        }

        /// <summary>
        /// Get a position by id
        /// </summary>
        /// <param name="id">Id of a position to be fetched</param>
        /// <returns>A position with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Position))]
        public IHttpActionResult GetPosition(int id)
        {
            if (id > 0)
            {
                var position = this.PositionService.GetOnePositionById(id);

                if (position != null)
                {
                    return Ok(position);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new position
        /// </summary>
        /// <param name="position">Position to be created</param>
        /// <returns>The Uri of newly created position</returns>
        [HttpPost]
        [ResponseType(typeof(Position))]
        public IHttpActionResult PostPosition(Position position)
        {
            if (ModelState.IsValid)
            {
                this.PositionService.CreatePosition(position);

                try
                {
                    this.PositionService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = position.PositionId }, position);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing position
        /// </summary>
        /// <param name="position">Position to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Position))]
        public IHttpActionResult PutPosition(Position position)
        {
            if (ModelState.IsValid)
            {
                this.PositionService.UpdatePosition(position);

                try
                {
                    this.PositionService.SaveChanges();

                    return Ok(position);
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

                    if (!this.PositionExists(position.PositionId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing position's status
        /// </summary>
        /// <param name="positionId">Id of the position for which status is going to be updated</param>
        /// <param name="status">Integer representation of the status to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult UpdatePositionStatus(int positionId, int status)
        {
            if (positionId > 0)
            {
                var position = this.PositionService.GetOnePositionById(positionId);

                if (position != null)
                {
                    if (status == (int)position.Status)
                    {
                        return Ok(position);
                    }

                    position.Status = (PositionStatus)status;

                    if ((PositionStatus)status == PositionStatus.Closed)
                    {
                        position.CloseDate = DateTime.Now;
                    }

                    this.PositionService.UpdatePosition(position);

                    try
                    {
                        this.PositionService.SaveChanges();
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

                        if (!this.PositionExists(position.PositionId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(position);
                }
            }

            return NotFound();
        }


        /// <summary>
        /// Assign an agent to an existing position
        /// </summary>
        /// <param name="positionId">Id of the position to which an agent to be assigned</param>
        /// <param name="agentId">Id of the agent to be assigned</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult AssignPosition(int positionId, int agentId)
        {
            if (positionId > 0)
            {
                var position = this.PositionService.GetOnePositionById(positionId);

                if (position != null)
                {
                    if (agentId == position.AgentId)
                    {
                        return Ok(position);
                    }

                    if (agentId != 0)
                    {
                        position.AgentId = agentId;
                    }
                    else
                    {
                        position.AgentId = null;
                    }

                    this.PositionService.UpdatePosition(position);

                    try
                    {
                        this.PositionService.SaveChanges();
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

                        if (!this.PositionExists(position.PositionId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    position.Agent = this.AgentService.GetOneAgentById((int)position.AgentId);

                    return Ok(position);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing position and its dependants
        /// </summary>
        /// <param name="positionIds">Ids of the positions to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Position))]
        public IHttpActionResult DeletePosition([FromUri]Int32[] positionIds)
        {
            if (positionIds.Length > 0)
            {
                try
                {
                    for(int i = 0; i < positionIds.Length; i++)
                    {
                        var position = this.PositionService.GetOnePositionById(positionIds[i]);

                        if(position != null)
                        {
                            this.PositionArchiveService.CreateArchivePosition(Mapper.Map<Position, PositionArchive>(position));

                            if(!Utilities.IsEmpty(position.Candidates))
                            {
                                foreach (var candidate in position.Candidates.ToList())
                                {
                                    this.CandidateArchiveService.CreateCandidateArchive(Mapper.Map<Candidate, CandidateArchive>(candidate));


                                    if (!Utilities.IsEmpty(candidate.Interviews))
                                    {
                                        foreach (var interview in candidate.Interviews.ToList())
                                        {
                                            this.InterviewService.DeleteInterview(interview);

                                            this.InterviewService.SaveChanges();
                                        
                                        }
                                    }

                                    this.CandidateService.DeleteCandidate(candidate);

                                    
                                }
                            }

                            this.PositionService.DeletePosition(position);
                            
                        }
                    }

                    this.PositionService.SaveChanges();
                    this.CandidateService.SaveChanges();
                    this.PositionArchiveService.SaveChanges();
                    this.CandidateArchiveService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return Ok();
                
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing position
        /// </summary>
        /// <param name="id">Id of the position to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Position))]
        public IHttpActionResult DeletePosition(int id)
        {
            if (id > 0)
            {
                var position = this.PositionService.GetOnePositionById(id);

                if (position != null)
                {
                    this.PositionService.DeletePosition(position);

                    try
                    {
                        this.PositionService.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.PositionExists(position.PositionId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(position);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// PositionExists is used to check whether the position is present in data context
        /// </summary>
        /// <param name="id">Id of the position to check against</param>
        /// <returns>True if position is present in data context, false otherwise</returns>
        private bool PositionExists(int id)
        {
            return this.PositionService.GetAllPositions().Count(p => p.PositionId == id) > 0;
        }
    }
}
