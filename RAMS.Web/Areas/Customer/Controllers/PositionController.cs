using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AutoMapper;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using RAMS.ViewModels;
using RAMS.Models;
using RAMS.Web.Controllers;
using RAMS.Helpers;
using RAMS.Enums;
using System.Net.Mime;

namespace RAMS.Web.Areas.Customer.Controllers
{
    /// <summary>
    /// PositionController implements position related methods
    /// </summary>
    public class PositionController : BaseController
    {
        /// <summary>
        /// Default action method that returns main view of Profile controller
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <returns>Main view of Position controller</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity.HasClaim("UserType", "Agent"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Agency" });
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                return View();
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = "SystemAdmin" });
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        #region Position List
        /// <summary>
        /// PositionList action method gets the list of all the positions for current client and passes it to _PositionList partial view
        /// </summary>
        /// <returns>_PositionList partial view with the list of all the positions</returns>
        [HttpGet]
        public async Task<PartialViewResult> PositionList()
        {
            var positions = new List<PositionListViewModel>();

            var response = await this.GetHttpClient().GetAsync(String.Format("Position?clientName={0}", User.Identity.Name));

            if (response.IsSuccessStatusCode)
            {
                positions.AddRange(Mapper.Map<List<Position>, List<PositionListViewModel>>(await response.Content.ReadAsAsync<List<Position>>()));

                return PartialView("_PositionList", positions);
            }

            return PartialView("_PositionList");
        }
        #endregion

        #region New Position
        /// <summary>
        /// NewPosition action method populates select lists and client id for the view model and passes it to _NewPosition partial view
        /// </summary>
        /// <returns>_NewPosition partial view if select lists and client id were populated successfully, _Error partial view with detailed error message otherwise</returns>
        [HttpGet]
        public async Task<PartialViewResult> NewPosition()
        {
            var categories = new List<Category>();

            var client = new Client();

            var response = await this.GetHttpClient().GetAsync("Category"); // Get all categories

            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<Category>>();
            }
            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append("<div class='text-center'><h4><strong>Failed to retrieve the list of categories.</strong></h4></div>");

                stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to retrieve the list of categories. Please try again in a moment.</div>", response.StatusCode));

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                return PartialView("_Error", confirmationViewModel);
            }

            response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", User.Identity.Name)); // Get current user (Client)

            if (response.IsSuccessStatusCode)
            {
                client = await response.Content.ReadAsAsync<Client>();
            }
            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append("<div class='text-center'><h4><strong>Failed to retrieve client's information.</strong></h4></div>");

                stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to retrieve client's information. Please try again in a moment.</div>", response.StatusCode));

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                return PartialView("_Error", confirmationViewModel);
            }

            var positionAddViewModel = new PositionAddViewModel();

            // Populate client id
            positionAddViewModel.ClientId = client.ClientId;

            // Populate select list for categories
            positionAddViewModel.Categories = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(categories.Select(c => new SelectListItem { Text = c.Name, Value = c.CategoryId.ToString() }).ToList()).ToList();

            return PartialView("_NewPosition", positionAddViewModel);
        }

        /// <summary>
        /// NewPosition action method attempts to persist new position to the data context 
        /// </summary>
        /// <param name="model">Position information required persist position to the data context</param>
        /// <returns>_PositionConfirmation partial view if position has been persisted successfully, _Error partial view otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> NewPosition(PositionAddViewModel model)
        {
            model.DateCreated = DateTime.UtcNow;

            var response = new HttpResponseMessage();

            var categories = new List<Category>();

            response = await this.GetHttpClient().GetAsync("Category"); // Get all categories

            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<Category>>();
            }
            else
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append("<div class='text-center'><h4><strong>Failed to retrieve the list of categories.</strong></h4></div>");

                stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to retrieve the list of categories. Please try again in a moment.</div>", response.StatusCode));

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                return PartialView("_Error", confirmationViewModel);
            }

            // Perform manual model validation
            if(model.AcceptanceScore == 0 && model.ClientId == 0) 
            { 
                ModelState.AddModelError(String.Empty, "Model state is not valid. Please try again in a moment"); 
            }
            else if (model.AcceptanceScore == 0)
            {
                ModelState.AddModelError(String.Empty, "Initial Acceptance Score could not be determined. Please try again in a moment");
            }
            else if (model.ClientId == 0)
            {
                ModelState.AddModelError(String.Empty, "Client could not be determined. Please try again in a moment");
            }

            if (model.ExpiryDate < DateTime.Now)
            {
                ModelState.AddModelError("ExpiryDate", "The Expiry Date field cannot be set to the current or past date.");
            }

            if(model.PeopleNeeded < 1) 
            { 
                ModelState.AddModelError("PeopleNeeded", "The People Needed field is required and should be greater than 0."); 
            }

            if (model.CategoryId == 0) 
            { 
                ModelState.AddModelError("CategoryId", "The Category field is required."); 
            }

            if(ModelState.IsValid)
            {
                var stringBuilder = new StringBuilder();

                try
                {
                    var position = Mapper.Map<PositionAddViewModel, Position>(model);

                    response = await this.GetHttpClient().PostAsJsonAsync("Position", position); // Attempt to persist position to the data context

                    if (response.IsSuccessStatusCode)
                    {
                        position = await response.Content.ReadAsAsync<Position>();

                        var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel("New Position Confirmation", String.Format("Position '{0}' has been successfully created and sent for approval.", position.Title), null, position.ClientId));

                        response = await this.GetHttpClient().PostAsJsonAsync("Notification", notification); // Attempt to persist notification to the data context

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new NotificationAddException(String.Format("Notification could NOT be added. Status Code: {1}", response.StatusCode));
                        }

                        position.Category = categories.FirstOrDefault(c => c.CategoryId == position.CategoryId);

                        var positionConfirmationViewModel = Mapper.Map<Position, PositionConfirmationViewModel>(position);

                        return PartialView("_PositionConfirmation", positionConfirmationViewModel);
                    }
                    else
                    {
                        // If position could not be created, throw PositionAddException exception
                        throw new PositionAddException("Position " + position.Title + " could not be created. Response: " + response.StatusCode);   
                    }
                }
                catch(PositionAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to save position details.</strong></h4></div>");

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>", response.StatusCode));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
                catch (NotificationAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to create new notification.</strong></h4></div>");

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been thrown while attempting to create new notification.</div>"));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Please review an exception log for more information about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            // If model state is not valid, re-populate Categories select list and redisplay the form
            model.Categories = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(categories.Select(c => new SelectListItem { Text = c.Name, Value = c.CategoryId.ToString() }).ToList()).ToList();

            return PartialView("_NewPosition", model);
        }
        #endregion

        /// <summary>
        /// PositionDetails action method retrieves the details of the position and passes it to _PositionDetails partial view
        /// </summary>
        /// <param name="positionId">Id of the position which details are being fetched</param>
        /// <returns>_PositionDetails partial view with the details for the position</returns>
        [HttpGet]
        public async Task<PartialViewResult> PositionDetails(int positionId)
        {
            if (positionId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Position?id={0}", positionId));

                if (response.IsSuccessStatusCode)
                {
                    var positionDetailsViewModel = Mapper.Map<Position, PositionDetailsViewModel>(await response.Content.ReadAsAsync<Position>());

                    return PartialView("_PositionDetails", positionDetailsViewModel);
                }
            }

            return PartialView("_PositionDetails");
        }

        #region Position Closure
        /// <summary>
        /// PositionClosure action method sets PositionClosureConfirmationViewModel and passes it to _PositionClosureConfirmation partial view
        /// </summary>
        /// <param name="agentId">Setter for AgentId</param>
        /// <param name="positionId">Setter for PositionId</param>
        /// <param name="positionTitle">Setter for PositionTitle</param>
        /// <param name="clientUserName">Setter for ClientUserName</param>
        /// <param name="clientFullName">Setter for ClientFullName</param>
        /// <returns>_PositionClosureConfirmation partial view</returns>
        [HttpGet]
        public PartialViewResult PositionClosure(int agentId, int positionId, string positionTitle, string clientUserName, string clientFullName)
        {
            var positionClosureConfirmationViewModel = new PositionClosureConfirmationViewModel(agentId, positionId, positionTitle, clientUserName, clientFullName);

            return PartialView("_PositionClosureConfirmation", positionClosureConfirmationViewModel);
        }

        /// <summary>
        /// PositionClosure action method validates the model and attempts to create new notification for the agent who is assigned to current position
        /// </summary>
        /// <param name="model">Information required to create notification</param>
        /// <returns>_SuccessConfirmation partial view if notification was created successfully, _FailureConfirmation otherwise</returns>
        [HttpPost]
        public async Task<PartialViewResult> PositionClosure(PositionClosureConfirmationViewModel model)
        {
            var positionResultViewModel = new PositionResultViewModel();

            var stringBuilder = new StringBuilder();

            if(ModelState.IsValid)
            {
                try
                {
                    var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel("Position Removal Request", String.Format("Position '{0}' ({1}) has been flagged for removal by client '{2}' ({3})", model.PositionTitle, model.PositionId.ToString("POS00000"), model.ClientFullName, model.ClientUserName), model.AgentId));

                    var response = await this.GetHttpClient().PostAsJsonAsync("Notification", notification); // Attempt to persist notification to the data context

                    if (response.IsSuccessStatusCode)
                    {
                        notification = await response.Content.ReadAsAsync<Notification>();

                        return PartialView("_SuccessConfirmation");
                    }
                    else
                    {
                        throw new NotificationAddException(String.Format("Notification could NOT be added. Status Code: {1}", response.StatusCode));
                    }
                }
                catch (NotificationAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to create new notification.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been thrown while attempting to create new notification.</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Please review an exception log for more information about the exception.</div></div>");

                    positionResultViewModel.Message = stringBuilder.ToString();

                    return PartialView("_FailureConfirmation", positionResultViewModel);
                }
            }

            stringBuilder.Append("<div class='text-center'><h4><strong>Failed to send a close request!</strong></h4></div>");

            stringBuilder.Append("<div class='text-center'>Position closure request could not be subitted at this time.</div>");

            stringBuilder.Append("<div class='text-center'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div>");

            positionResultViewModel.Message = stringBuilder.ToString();

            return PartialView("_FailureConfirmation", positionResultViewModel);
        }
        #endregion

        /// <summary>
        /// GetResume method attempts to fetch candidates resume and return it as s file
        /// </summary>
        /// <param name="resumeId">Id of the candidate whos resume is being fetched</param>
        /// <returns>Resume as a file</returns>
        [HttpGet]
        public async Task<FileResult> GetResume(string resumeId)
        {
            var response = await this.GetHttpClient().GetAsync(String.Format("Candidate?Id={0}", (RAMS.Helpers.Utilities.ConvertBase64StringToInt(resumeId) - Int32.Parse(Session[String.Format("{0}_resume_request", User.Identity.Name)].ToString()))));

            if (response.IsSuccessStatusCode)
            {
                var candidateResumeDownloadViewModel = await response.Content.ReadAsAsync<CandidateResumeDownloadViewModel>();

                if (candidateResumeDownloadViewModel.FileContent != null)
                {
                    Response.AppendHeader("Content-Disposition", new ContentDisposition { FileName = candidateResumeDownloadViewModel.FileName, Inline = true }.ToString());

                    return File(candidateResumeDownloadViewModel.FileContent, candidateResumeDownloadViewModel.MediaType);
                }
            }

            return null;
        }
    }
}