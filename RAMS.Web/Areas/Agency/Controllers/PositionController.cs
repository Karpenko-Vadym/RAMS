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

namespace RAMS.Web.Areas.Agency.Controllers
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
        /// <param name="message">Message that will be displayed in the view. This message will inform the user about success or failure of particular operation</param>
        /// <returns>Main view of Position controller</returns>
        [HttpGet]
        public ActionResult Index(string message = "")
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity.HasClaim("UserType", "Agent"))
            {
                return View();
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Customer" });
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = "SystemAdmin" });
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        #region Position List
        /// <summary>
        /// PositionList action method gets the list of all the positions and passes it to _PositionList partial view
        /// </summary>
        /// <returns>_PositionList partial view with the list of all the positions</returns>
        [HttpGet]
        public async Task<PartialViewResult> PositionList()
        {
            var identity = User.Identity as ClaimsIdentity;

            var positions = new List<PositionListViewModel>();

            var response = new HttpResponseMessage();

            if (identity.HasClaim("Role", "Manager"))
            {
                response = await this.GetHttpClient().GetAsync("Position");

                if (response.IsSuccessStatusCode)
                {
                    positions.AddRange(Mapper.Map<List<Position>, List<PositionListViewModel>>(await response.Content.ReadAsAsync<List<Position>>()));

                    return PartialView("_PositionList", positions);
                }
            }
            else if (identity.HasClaim("Role", "Employee"))
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Position?agentName={0}", User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    positions.AddRange(Mapper.Map<List<Position>, List<PositionListViewModel>>(await response.Content.ReadAsAsync<List<Position>>()));

                    return PartialView("_PositionList", positions);
                }
            }

            return PartialView("_PositionList");
        }
        #endregion
        
        /// <summary>
        /// EditPosition action method gets requested position's details and passes it to _EditPosition partial view
        /// </summary>
        /// <param name="positionId">Id of the position that is being fetched</param>
        /// <returns>_EditPosition partial view with position details</returns>
        [HttpGet]
        public async Task<PartialViewResult> EditPosition(int positionId)
        {
            if (positionId > 0)
            {
                var categories = new List<Category>();

                var response = await this.GetHttpClient().GetAsync(String.Format("Position?id={0}", positionId));

                if (response.IsSuccessStatusCode)
                {
                    var positionEditViewModel = Mapper.Map<Position, PositionEditViewModel>(await response.Content.ReadAsAsync<Position>());

                    response = await this.GetHttpClient().GetAsync("Category"); // Get all categories

                    if (response.IsSuccessStatusCode)
                    {
                        categories = await response.Content.ReadAsAsync<List<Category>>();
                    }
                    else
                    {
                        var stringBuilder = new StringBuilder();

                        stringBuilder.Append("<div class='text-center'><h4><strong>Failed to retrieve the list of categories.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to retrieve the list of categories. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                        return PartialView("_Error", confirmationViewModel);
                    }

                    // Populate select list for categories
                    positionEditViewModel.Categories = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(categories.Select(c => new SelectListItem { Text = c.Name, Value = c.CategoryId.ToString() }).ToList()).ToList();

                    return PartialView("_EditPosition", positionEditViewModel);
                }
            }

            return PartialView("_EditPosition");
        }

        /// <summary>
        /// EditPosition action method attempts to update position details
        /// </summary>
        /// <param name="model">Position information required to update the position</param>
        /// <returns>_PositionConfirmation partial view if position has been updated successfully, _Error partial view otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditPosition(PositionEditViewModel model)
        {
            model.DateCreated = DateTime.Now;

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

                stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to retrieve the list of categories. Please try again in a moment.</div>");

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                return PartialView("_Error", confirmationViewModel);
            }

            // Perform manual model validation
            if (model.AcceptanceScore == 0 && model.ClientId == 0)
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

            if (model.PeopleNeeded < 1)
            {
                ModelState.AddModelError("PeopleNeeded", "The People Needed field is required and should be greater than 0.");
            }

            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "The Category field is required.");
            }

            if(ModelState.IsValid)
            {
                try
                {
                    var position = Mapper.Map<PositionEditViewModel, Position>(model);

                    response = await this.GetHttpClient().PutAsJsonAsync("Position", position); // Attempt to persist position to the data context

                    if (response.IsSuccessStatusCode)
                    {
                        position = await response.Content.ReadAsAsync<Position>();

                        position.Category = categories.FirstOrDefault(c => c.CategoryId == position.CategoryId);

                        var positionConfirmationViewModel = Mapper.Map<Position, PositionConfirmationViewModel>(position);

                        return PartialView("_PositionConfirmation", positionConfirmationViewModel);
                    }
                    else
                    {
                        // If position could not be edited, throw PositionEditException exception
                        throw new PositionEditException("Position " + position.Title + " could not be edited. Response: " + response.StatusCode);
                    }
                }
                catch (PositionEditException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to save position details.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            // If model state is not valid, re-populate Categories select list and redisplay the form
            model.Categories = new[] { new SelectListItem { Text = "", Value = string.Empty } }.Concat(categories.Select(c => new SelectListItem { Text = c.Name, Value = c.CategoryId.ToString() }).ToList()).ToList();

            return PartialView("_EditPosition", model);
        }

        /// <summary>
        /// EditCandidate action method retrieves candidate's data and displays it in _EditCandidate partial view
        /// </summary>
        /// <param name="candidateId">Id of the candidate whos information will be displayed</param>
        /// <param name="positionStatus">Status of the position (Needed for access control)</param>
        /// <returns>_EditCandidate partial view with candidates information</returns>
        [HttpGet]
        public async Task<PartialViewResult> EditCandidate(int candidateId, string positionStatus)
        {
            if (candidateId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Candidate?id={0}", candidateId));

                if (response.IsSuccessStatusCode)
                {
                    var candidateEditViewModel = Mapper.Map<Candidate, CandidateEditViewModel>(await response.Content.ReadAsAsync<Candidate>());

                    candidateEditViewModel.PositionStatus = positionStatus;

                    return PartialView("_EditCandidate", candidateEditViewModel);
                }
            }

            return PartialView("_EditCandidate");
        }

        /// <summary>
        /// EditCandidate action method attempts to update candidate's feedback
        /// </summary>
        /// <param name="model">Candidate information required to update candidate's feedback</param>
        /// <returns>_CandidateEditConfirmation partial view if candidate's feedback has been updated successfully, _Error partial view otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditCandidate(CandidateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await this.GetHttpClient().PutAsync(String.Format("Candidate?candidateId={0}&feedback={1}", model.CandidateId, model.Feedback), null); // Attempt to update the feedback

                    if (response.IsSuccessStatusCode)
                    {
                        var candidate = await response.Content.ReadAsAsync<Candidate>();

                        var candidateEditConfirmationViewModel = Mapper.Map<Candidate, CandidateEditConfirmationViewModel>(candidate);

                        return PartialView("_CandidateEditConfirmation", candidateEditConfirmationViewModel);
                    }
                    else
                    {
                        // If candidate could not be updated, throw CandidateEditException exception
                        throw new CandidateEditException("Candidate " + model.FirstName + model.LastName + " could not be updated. Response: " + response.StatusCode);
                    }
                }
                catch (CandidateEditException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update candidate details.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist candidate details to the database. Please try again in a moment.</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            return PartialView("_EditCandidate", model);
        }

        /// <summary>
        /// ApprovePosition action method displays confirmation for position approval in _ApprovePosition partial view
        /// </summary>
        /// <param name="positionId">Id of the position to be approved</param>
        /// <param name="positionTitle">Title of the position to be approved</param>
        /// <returns>_ApprovePosition partial view with prompt of confirmation to approve the position</returns>
        [HttpGet]
        public PartialViewResult ApprovePosition(int positionId, string positionTitle)
        {
            var positionApprovalViewModel = new PositionApprovalViewModel(positionId, positionTitle);

            return PartialView("_ApprovePosition", positionApprovalViewModel);
        }

        /// <summary>
        /// ApprovePosition action method attempts to update position status to approved
        /// </summary>
        /// <param name="model">Position information required to update position status</param>
        /// <returns>_SuccessConfirmation partial view if position status has been updated successfully, _FailureConfirmation partial view otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> ApprovePosition(PositionApprovalViewModel model)
        {
            var stringBuilder = new StringBuilder();

            var positionResultViewModel = new PositionResultViewModel();

            if(ModelState.IsValid)
            {
                try
                {
                    var response = await this.GetHttpClient().PutAsync(String.Format("Position?positionId={0}&status={1}", model.PositionId, (int)PositionStatus.Approved), null); // Attempt to update the status

                    if (response.IsSuccessStatusCode)
                    {
                        var position = await response.Content.ReadAsAsync<Position>();

                        if (position.Status == PositionStatus.Approved)
                        {
                            stringBuilder.Append("<div class='text-center'><h4><strong>Position has been approved successfully!</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Position will now be available for applicants to apply.</div>");

                            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Details of this position can be modified at anytime, unless position is closed.</div></div>");

                            positionResultViewModel.Message = stringBuilder.ToString();

                            positionResultViewModel.RefreshList = true;

                            positionResultViewModel.RefreshEditForm = true;

                            positionResultViewModel.PositionId = model.PositionId;

                            return PartialView("_SuccessConfirmation", positionResultViewModel);
                        }
                        else
                        {
                            stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}', but position status was not updated. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            positionResultViewModel.Message = stringBuilder.ToString();

                            return PartialView("_FailureConfirmation", positionResultViewModel);
                        }
                    }
                    else
                    {
                        // If position could not be updated, throw PositionEditException exception
                        throw new PositionEditException("Position " + model.Title + " could not be updated. Response: " + response.StatusCode);
                    }
                }
                catch (PositionEditException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                    positionResultViewModel.Message = stringBuilder.ToString();

                    return PartialView("_FailureConfirmation", positionResultViewModel);
                }
            }

            stringBuilder.Append("<div class='text-center'><h4><strong>Position details could NOT be retrieved at this moment.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Model state is not valid. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

            positionResultViewModel.Message = stringBuilder.ToString();

            return PartialView("_FailureConfirmation", positionResultViewModel); 
        }

        /// <summary>
        /// ClosePosition action method displays confirmation for position closure in _ClosePosition partial view
        /// </summary>
        /// <param name="positionId">Id of the position to be closed</param>
        /// <param name="positionTitle">Title of the position to be closed</param>
        /// <returns>_ClosePosition partial view with prompt of confirmation to close the position</returns>
        [HttpGet]
        public PartialViewResult ClosePosition(int positionId, string positionTitle)
        {
            var positionClosureViewModel = new PositionClosureViewModel(positionId, positionTitle);

            return PartialView("_ClosePosition", positionClosureViewModel);
        }

        /// <summary>
        /// ClosePosition action method attempts to update position status to closed
        /// </summary>
        /// <param name="model">Position information required to update position status</param>
        /// <returns>_SuccessConfirmation partial view if position status has been updated successfully, _FailureConfirmation partial view otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> ClosePosition(PositionClosureViewModel model)
        {
            var stringBuilder = new StringBuilder();

            var positionResultViewModel = new PositionResultViewModel();

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await this.GetHttpClient().PutAsync(String.Format("Position?positionId={0}&status={1}", model.PositionId, (int)PositionStatus.Closed), null); // Attempt to update the status

                    if (response.IsSuccessStatusCode)
                    {
                        var position = await response.Content.ReadAsAsync<Position>();

                        if (position.Status == PositionStatus.Closed)
                        {
                            stringBuilder.Append("<div class='text-center'><h4><strong>Position has been closed successfully!</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Position is no longer available on job portal.</div>");

                            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Details of this position CANNOT be modified anymore.</div></div>");

                            positionResultViewModel.Message = stringBuilder.ToString();

                            positionResultViewModel.RefreshList = true;

                            positionResultViewModel.RefreshEditForm = true;

                            positionResultViewModel.PositionId = model.PositionId;

                            return PartialView("_SuccessConfirmation", positionResultViewModel);
                        }
                        else
                        {
                            stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}', but position status was not updated. Please try again in a moment.</div>");

                            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                            positionResultViewModel.Message = stringBuilder.ToString();

                            return PartialView("_FailureConfirmation", positionResultViewModel);
                        }
                    }
                    else
                    {
                        // If position could not be updated, throw PositionEditException exception
                        throw new PositionEditException("Position " + model.Title + " could not be updated. Response: " + response.StatusCode);
                    }
                }
                catch (PositionEditException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>");

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                    positionResultViewModel.Message = stringBuilder.ToString();

                    return PartialView("_FailureConfirmation", positionResultViewModel);
                }
            }

            stringBuilder.Append("<div class='text-center'><h4><strong>Position details could NOT be retrieved at this moment.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Model state is not valid. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

            positionResultViewModel.Message = stringBuilder.ToString();

            return PartialView("_FailureConfirmation", positionResultViewModel);
        }

        /// <summary>
        /// AssignPosition action method displays the list of agents that can be assigned to the position
        /// </summary>
        /// <param name="positionId">Id of the position to which an agent to be assigned</param>
        /// <param name="agentId">Id of the agent that is currently assigned to this position (If any)</param>
        /// <param name="positionTitle">Title of the position</param>
        /// <returns>_AssignPosition partial view with the list of agents that can be assigned to the position</returns>
        [HttpGet]
        public async Task<PartialViewResult> AssignPosition(int positionId, int agentId, string positionTitle)
        {
            var positionAssignViewModel = new PositionAssignViewModel(positionId, agentId, positionTitle);

            var response = await this.GetHttpClient().GetAsync("Agent");

            if (response.IsSuccessStatusCode)
            {
                positionAssignViewModel.Agents.AddRange(Mapper.Map<List<Agent>, List<AgentAssignPositionViewModel>>(await response.Content.ReadAsAsync<List<Agent>>()));

                return PartialView("_AssignPosition", positionAssignViewModel);
            }

            return PartialView("_AssignPosition");
        }

        /// <summary>
        /// AssignPosition action method attempts to assign an agent to the position
        /// </summary>
        /// <param name="model">Position information required to assign an agent to the position</param>
        /// <returns>_SuccessConfirmation partial view if an agent has been successfully assigned to the position, _FailureConfirmation partial view otherwise</returns>
        [HttpPost]
        public async Task<PartialViewResult> AssignPosition(PositionAssignViewModel model)
        {
            var stringBuilder = new StringBuilder();

            var positionResultViewModel = new PositionResultViewModel();
             
            if(ModelState.IsValid)
            {
                if(model.AgentId != model.SelectedAgentId)
                {
                    try
                    {
                        var response = await this.GetHttpClient().PutAsync(String.Format("Position?positionId={0}&agentId={1}", model.PositionId, model.SelectedAgentId), null); // Attempt to assign an agent

                        if (!response.IsSuccessStatusCode)
                        {
                            // If position could not be updated, throw PositionEditException exception
                            throw new PositionEditException("Position " + model.Title + " could not be updated. Response: " + response.StatusCode);
                        }
                        else
                        {
                            var position = await response.Content.ReadAsAsync<Position>();

                            if (position.AgentId != model.SelectedAgentId)
                            {
                                stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                                stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}', but position status was not updated. Please try again in a moment.</div>");

                                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                                positionResultViewModel.Message = stringBuilder.ToString();

                                return PartialView("_FailureConfirmation", positionResultViewModel);
                            }
                        }
                    }
                    catch (PositionEditException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>");

                        stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        positionResultViewModel.Message = stringBuilder.ToString();

                        return PartialView("_FailureConfirmation", positionResultViewModel);
                    }
                }

                stringBuilder.Append(String.Format("<div class='text-center'><h4><strong>An agent has been successfully assigned to \"{0}\" position!</strong></h4></div>", model.Title));

                stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Position \"{0}\" is now available for the assigned agent</div>", model.Title));

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> An agent can be re-assigned at any time as long as position is not closed.</div></div>");

                positionResultViewModel.Message = stringBuilder.ToString();

                positionResultViewModel.RefreshList = true;

                positionResultViewModel.RefreshEditForm = true;

                positionResultViewModel.PositionId = model.PositionId;

                return PartialView("_SuccessConfirmation", positionResultViewModel);
            }

            stringBuilder.Append("<div class='text-center'><h4><strong>Agent could not be assigned to this position at the moment.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Model state is not valid. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

            positionResultViewModel.Message = stringBuilder.ToString();

            return PartialView("_FailureConfirmation", positionResultViewModel);
        }
    }
}