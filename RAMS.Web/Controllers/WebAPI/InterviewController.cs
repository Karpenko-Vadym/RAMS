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
    /// InterviewController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class InterviewController : ApiController
    {
        private readonly IInterviewService InterviewService;
        private readonly IAgentService AgentService;
        private readonly ICandidateService CandidateService;

        /// <summary>
        /// Controller that sets interview service in order to access context resources
        /// </summary>
        /// <param name="interviewService">Parameter for setting interview service</param>
        /// <param name="agentService">Parameter for setting agent service</param>
        /// <param name="candidateService">Parameter for setting candidate service</param>
        public InterviewController(IInterviewService interviewService, IAgentService agentService, ICandidateService candidateService)
        {
            this.InterviewService = interviewService;
            this.AgentService = agentService;
            this.CandidateService = candidateService;
        }

        /// <summary>
        /// Get the list of all interviews
        /// </summary>
        /// <returns>The list of all interviews</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Interview>))]
        public IHttpActionResult GetAllInterviews()
        {
            var interviews = this.InterviewService.GetAllInterviews();

            interviews.ToList().ForEach(i => { i.Interviewer.Interviews = null; i.Candidate.Interviews = null; });

            if (!Utilities.IsEmpty(interviews))
            {
                return Ok(interviews);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of many interviews for specific agent
        /// </summary>
        /// <param name="agentId">Id of the agent whos interviews are being fetched</param>
        /// <returns>The list of many interviews for specific agent</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Interview>))]
        public IHttpActionResult GetManyInterviewsByAgentId(int agentId)
        {
            var interviews = this.InterviewService.GetManyInterviewsByAgentId(agentId);

            if (!Utilities.IsEmpty(interviews))
            {
                return Ok(interviews);
            }

            return NotFound();
        }

        /// <summary>
        /// Get the list of many interviews for specific agent by username
        /// </summary>
        /// <param name="username">Username of the agent whos interviews are being fetched</param>
        /// <returns>The list of many interviews for specific agent by username</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Interview>))]
        public IHttpActionResult GetManyInterviewsByAgentId(string username)
        {
            var interviews = this.InterviewService.GetManyInterviewsByAgentUsername(username);

            if (!Utilities.IsEmpty(interviews))
            {
                return Ok(interviews);
            }

            return NotFound();
        }

        /// <summary>
        /// Get an interview by id
        /// </summary>
        /// <param name="id">Id of an interview to be fetched</param>
        /// <returns>An interview with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult GetInterview(int id)
        {
            if (id > 0)
            {
                var interview = this.InterviewService.GetOneInterviewById(id);

                if (interview != null)
                {
                    return Ok(interview);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new interview
        /// </summary>
        /// <param name="interview">An interview to be created</param>
        /// <returns>The Uri of newly created interview</returns>
        [HttpPost]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult PostInterview(Interview interview)
        {
            if (ModelState.IsValid)
            {
                this.InterviewService.CreateInterview(interview);

                try
                {
                    this.InterviewService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = interview.InterviewId }, interview);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Create new interview from parameters
        /// </summary>
        /// <param name="candidateId">Setter for CandidateId</param>
        /// <param name="selectedDate">Setter for InterviewDate</param>
        /// <param name="agentUserName">Setter for InterviewerId</param>
        /// <param name="selected">Flag that indicates whether candidate has interviews</param>
        /// <returns>The Uri of newly created interview</returns>
        [HttpPost]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult CreateInterview(int candidateId, string selectedDate, string agentUserName, bool selected)
        {
            if (candidateId > 0 && !String.IsNullOrEmpty(selectedDate) && !String.IsNullOrEmpty(agentUserName))
            {
                if(selected)
                {
                    var interviews = this.InterviewService.GetManyInterviewsByCandidateId(candidateId);

                    if(!Utilities.IsEmpty(interviews))
                    {
                        foreach(var item in interviews.ToList())
                        {
                            this.DeleteInterview(item.InterviewId);
                        }
                    }   
                }

                var interview = new Interview();

                interview.CandidateId = candidateId;
                interview.InterviewDate = Convert.ToDateTime(selectedDate);
                interview.InterviewerId = this.AgentService.GetOneAgentByUserName(agentUserName).AgentId;

                this.InterviewService.CreateInterview(interview);

                var candidate = this.CandidateService.GetOneCandidateById(candidateId);

                candidate.Status = Enums.CandidateStatus.Pending;

                this.CandidateService.UpdateCandidate(candidate);

                try
                {
                    this.InterviewService.SaveChanges();
                    this.CandidateService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                interview.Candidate = this.CandidateService.GetOneCandidateById(candidateId);

                return CreatedAtRoute("DefaultApi", new { id = interview.InterviewId }, interview);

            }

            return BadRequest();
        }

        /// <summary>
        /// Update existing interview
        /// </summary>
        /// <param name="interview">Interview to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult PutInterview(Interview interview)
        {
            if (ModelState.IsValid)
            {
                this.InterviewService.UpdateInterview(interview);

                try
                {
                    this.InterviewService.SaveChanges();

                    return Ok(interview);
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

                    if (!this.InterviewExists(interview.InterviewId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete existing interview
        /// </summary>
        /// <param name="id">Id of the interview to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Interview))]
        public IHttpActionResult DeleteInterview(int id)
        {
            if (id > 0)
            {
                var interview = this.InterviewService.GetOneInterviewById(id);

                if (interview != null)
                {
                    this.InterviewService.DeleteInterview(interview);

                    try
                    {
                        this.InterviewService.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.InterviewExists(interview.InterviewId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(interview);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// InterviewExists is used to check whether the interview is present in data context
        /// </summary>
        /// <param name="id">Id of the interview to check against</param>
        /// <returns>True if interview is present in data context, false otherwise</returns>
        private bool InterviewExists(int id)
        {
            return this.InterviewService.GetAllInterviews().Count(i => i.InterviewId == id) > 0;
        }
    }
}
