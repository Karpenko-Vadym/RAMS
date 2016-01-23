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

        /// <summary>
        /// Controller that sets position service in order to access context resources
        /// </summary>
        /// <param name="positionService">Parameter for setting position service</param>
        public PositionController(IPositionService positionService)
        {
            this.PositionService = positionService;
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
