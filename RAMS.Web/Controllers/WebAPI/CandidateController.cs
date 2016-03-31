using RAMS.Helpers;
using RAMS.Models;
using RAMS.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RAMS.Web.Controllers.WebAPI
{
    /// <summary>
    /// CandidateController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class CandidateController : ApiController
    {
        private readonly ICandidateService CandidateService;

        /// <summary>
        /// Controller that sets candidate service in order to access context resources
        /// </summary>
        /// <param name="candidateService">Parameter for setting candidate service</param>
        public CandidateController(ICandidateService candidateService)
        {
            this.CandidateService = candidateService;
        }

        /// <summary>
        /// Get the list of all candidates
        /// </summary>
        /// <returns>The list of all candidates</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Candidate>))]
        public IHttpActionResult GetAllCandidates()
        {
            var candidates = this.CandidateService.GetAllCandidates();

            if (!Utilities.IsEmpty(candidates))
            {
                return Ok(candidates);
            }

            return NotFound();
        }

        /// <summary>
        /// Get a candidate by id
        /// </summary>
        /// <param name="id">Id of a candidate to be fetched</param>
        /// <returns>A candidate with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult GetCandidate(int id)
        {
            if (id > 0)
            {
                var candidate = this.CandidateService.GetOneCandidateById(id);

                if (candidate != null)
                {
                    return Ok(candidate);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get candidates resume as file by candidate id
        /// </summary>
        /// <param name="candidateId">Id of the candidate whos resume is being fetched</param>
        /// <returns>Resume as a file</returns>
        [HttpGet]
        [ResponseType(typeof(Candidate))]
        public HttpResponseMessage GetCandidateResumeById(int candidateId)
        {
            var result = new HttpResponseMessage(HttpStatusCode.NotFound);

            if (candidateId > 0)
            {
                var candidate = this.CandidateService.GetOneCandidateById(candidateId);

                if (candidate != null)
                {
                    result.StatusCode = HttpStatusCode.OK;

                    result.Content = new StreamContent(new MemoryStream(candidate.FileContent));

                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(candidate.MediaType);

                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"){ FileName = candidate.FileName };
                    
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Create new candidate
        /// </summary>
        /// <param name="candidate">A candidate to be created</param>
        /// <returns>The Uri of newly created candidate</returns>
        [HttpPost]
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult PostCandidate(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                this.CandidateService.CreateCandidate(candidate);

                try
                {
                    this.CandidateService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = candidate.CandidateId }, candidate);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing candidate
        /// </summary>
        /// <param name="candidate">Candidate to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult PutCandidate(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                this.CandidateService.UpdateCandidate(candidate);

                try
                {
                    this.CandidateService.SaveChanges();

                    return Ok(candidate);
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

                    if (!this.CandidateExists(candidate.CandidateId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update candidate feedback
        /// </summary>
        /// <param name="candidateId">If of the candidate to be updated</param>
        /// <param name="feedback">Feedback to be updated</param>
        /// <param name="isInterviewed">Flag to determine whether candidate was interviewd</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult UpdateCandidateFeedback(int candidateId, string feedback, bool isInterviewed)
        {
            if (candidateId > 0)
            {
                var candidate = this.CandidateService.GetOneCandidateById(candidateId);

                if (candidate != null)
                {
                    if (feedback == candidate.Feedback)
                    {
                        return Ok(candidate);
                    }

                    candidate.Feedback = feedback;

                    if(isInterviewed)
                    {
                        candidate.Status = Enums.CandidateStatus.Interviewed;
                    }

                    this.CandidateService.UpdateCandidate(candidate);

                    try
                    {
                        this.CandidateService.SaveChanges();
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

                        if (!this.CandidateExists(candidate.CandidateId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(candidate);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Delete existing candidate
        /// </summary>
        /// <param name="id">Id of the candidate to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult DeleteCandidate(int id)
        {
            if (id > 0)
            {
                var candidate = this.CandidateService.GetOneCandidateById(id);

                if (candidate != null)
                {
                    this.CandidateService.DeleteCandidate(candidate);

                    try
                    {
                        this.CandidateService.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.CandidateExists(candidate.CandidateId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(candidate);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// CandidateExists is used to check whether the candidate is present in data context
        /// </summary>
        /// <param name="id">Id of the candidate to check against</param>
        /// <returns>True if candidate is present in data context, false otherwise</returns>
        private bool CandidateExists(int id)
        {
            return this.CandidateService.GetAllCandidates().Count(c => c.CandidateId == id) > 0;
        }
    }
}
