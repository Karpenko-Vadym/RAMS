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

namespace RAMS.Web.Areas.Agency.Controllers
{
    /// <summary>
    /// PositionController implements position related methods
    /// </summary>
    public class PositionController : BaseController
    {
        /// <summary>
        /// Default action method that returns main view of Position controller
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <returns>Main view of Position controller</returns>
        [HttpGet]
        public ActionResult Index()
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

        #region Edit Position
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

                        stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to retrieve the list of categories. Please try again in a moment.</div>", response.StatusCode));

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
                var stringBuilder = new StringBuilder();

                try
                {
                    var position = Mapper.Map<PositionEditViewModel, Position>(model);

                    response = await this.GetHttpClient().PutAsJsonAsync("Position", position); // Attempt to persist position to the data context

                    if (response.IsSuccessStatusCode)
                    {
                        position = await response.Content.ReadAsAsync<Position>();

                        var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel("Position Update Confirmation", String.Format("Position '{0}' ({1}) has been successfully updated.", position.Title, position.PositionId.ToString("POS00000"))));

                        response = await this.GetHttpClient().PostAsJsonAsync(String.Format("Notification?agentUsername={0}", User.Identity.Name), notification); // Attempt to persist notification to the data context

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
                        // If position could not be edited, throw PositionEditException exception
                        throw new PositionEditException("Position " + position.Title + " could not be edited. Response: " + response.StatusCode);
                    }
                }
                catch (PositionEditException ex)
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

            return PartialView("_EditPosition", model);
        }
        #endregion

        #region Edit Candidate
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
                var response = new HttpResponseMessage();

                var stringBuilder = new StringBuilder();

                try
                {
                    response = await this.GetHttpClient().PutAsync(String.Format("Candidate?candidateId={0}&feedback={1}&isInterviewed={2}", model.CandidateId, model.Feedback, model.IsInterviewed), null); // Attempt to update the feedback

                    if (response.IsSuccessStatusCode)
                    {
                        var candidate = await response.Content.ReadAsAsync<Candidate>();

                        var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel("Candidate Update Confirmation", String.Format("Candidate '{0}' ({1}) has been successfully updated.", String.Format("{0} {1}", candidate.FirstName, candidate.LastName), candidate.CandidateId.ToString("CAN00000"))));

                        response = await this.GetHttpClient().PostAsJsonAsync(String.Format("Notification?agentUsername={0}", User.Identity.Name), notification); // Attempt to persist notification to the data context

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new NotificationAddException(String.Format("Notification could NOT be added. Status Code: {1}", response.StatusCode));
                        }

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

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update candidate details.</strong></h4></div>");

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist candidate details to the database. Please try again in a moment.</div>", response.StatusCode));

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

            return PartialView("_EditCandidate", model);
        }
        #endregion

        #region Approve Position
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
                var response = new HttpResponseMessage();

                try
                {
                    response = await this.GetHttpClient().PutAsync(String.Format("Position?positionId={0}&status={1}", model.PositionId, (int)PositionStatus.Approved), null); // Attempt to update the status

                    if (response.IsSuccessStatusCode)
                    {
                        var position = await response.Content.ReadAsAsync<Position>();

                        if (position.Status == PositionStatus.Approved)
                        {
                            var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel(String.Format("Position Approval Confirmation", model.Title), String.Format("Position '{0}' ({1}) has been successfully approved and now available on a job portal.", model.Title, model.PositionId.ToString("POS00000")), null, position.ClientId));

                            response = await this.GetHttpClient().PostAsJsonAsync("Notification", notification); // Attempt to persist notification to the data context

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new NotificationAddException(String.Format("Notification could NOT be added. Status Code: {1}", response.StatusCode));
                            }
                            
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

                            stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}', but position status was not updated. Please try again in a moment.</div>", response.StatusCode));

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

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>", response.StatusCode));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                    positionResultViewModel.Message = stringBuilder.ToString();

                    return PartialView("_FailureConfirmation", positionResultViewModel);
                }
                catch (NotificationAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to create new notification.</strong></h4></div>");

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been thrown while attempting to create new notification.</div>"));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Please review an exception log for more information about the exception.</div></div>");

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
        #endregion

        #region Close Position
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
                var response = new HttpResponseMessage();

                try
                {
                    response = await this.GetHttpClient().PutAsync(String.Format("Position?positionId={0}&status={1}", model.PositionId, (int)PositionStatus.Closed), null); // Attempt to update the status

                    if (response.IsSuccessStatusCode)
                    {
                        var position = await response.Content.ReadAsAsync<Position>();

                        if (position.Status == PositionStatus.Closed)
                        {
                            var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel(String.Format("Position Closure Confirmation", model.Title), String.Format("Position '{0}' ({1}) has been successfully closed and no longer available on a job portal.", model.Title, model.PositionId.ToString("POS00000")), null, position.ClientId));

                            response = await this.GetHttpClient().PostAsJsonAsync("Notification", notification); // Attempt to persist notification to the data context

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new NotificationAddException(String.Format("Notification could NOT be added. Status Code: {1}", response.StatusCode));
                            }

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

                            stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}', but position status was not updated. Please try again in a moment.</div>", response.StatusCode));

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

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>", response.StatusCode));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                    positionResultViewModel.Message = stringBuilder.ToString();

                    return PartialView("_FailureConfirmation", positionResultViewModel);
                }
                catch (NotificationAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to create new notification.</strong></h4></div>");

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been thrown while attempting to create new notification.</div>"));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Please review an exception log for more information about the exception.</div></div>");

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
        #endregion

        #region Assign Position
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
                    var response = new HttpResponseMessage();

                    var position = new Position();

                    try
                    {
                        response = await this.GetHttpClient().PutAsync(String.Format("Position?positionId={0}&agentId={1}", model.PositionId, model.SelectedAgentId), null); // Attempt to assign an agent

                        if (!response.IsSuccessStatusCode)
                        {
                            // If position could not be updated, throw PositionEditException exception
                            throw new PositionEditException("Position " + model.Title + " could not be updated. Response: " + response.StatusCode);
                        }
                        else
                        {
                            position = await response.Content.ReadAsAsync<Position>();

                            if (position.AgentId != model.SelectedAgentId)
                            {
                                stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                                stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}', but position status was not updated. Please try again in a moment.</div>", response.StatusCode));

                                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                                positionResultViewModel.Message = stringBuilder.ToString();

                                return PartialView("_FailureConfirmation", positionResultViewModel);
                            }
                        }

                        var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel(String.Format("Position Assignment Confirmation", model.Title), String.Format("Position '{0}' ({1}) has been successfully assigned to '{2}'.", model.Title, model.PositionId.ToString("POS00000"), String.Format("{0} {1}", position.Agent.FirstName, position.Agent.LastName)), model.SelectedAgentId));

                        response = await this.GetHttpClient().PostAsJsonAsync("Notification", notification); // Attempt to persist notification to the data context

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new NotificationAddException(String.Format("Notification could NOT be added. Status Code: {1}", response.StatusCode));
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
                    catch (PositionEditException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        stringBuilder.Append("<div class='text-center'><h4><strong>Failed to update position details.</strong></h4></div>");

                        stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist position details to the database. Please try again in a moment.</div>", response.StatusCode));

                        stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

                        positionResultViewModel.Message = stringBuilder.ToString();

                        return PartialView("_FailureConfirmation", positionResultViewModel);
                    }
                    catch (NotificationAddException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                        stringBuilder.Append("<div class='text-center'><h4><strong>Failed to create new notification.</strong></h4></div>");

                        stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been thrown while attempting to create new notification.</div>"));

                        stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Please review an exception log for more information about the exception.</div></div>");

                        positionResultViewModel.Message = stringBuilder.ToString();

                        return PartialView("_FailureConfirmation", positionResultViewModel);
                    }
                }

                stringBuilder.Append(String.Format("<div class='text-center'><h4><strong>An agent has been successfully re-assigned to \"{0}\" position!</strong></h4></div>", model.Title));

                stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Position \"{0}\" is available for the re-assigned agent</div>", model.Title));

                stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> An agent can be re-assigned at any time as long as position is not closed.</div></div>");

                positionResultViewModel.Message = stringBuilder.ToString();

                

                return PartialView("_SuccessConfirmation", positionResultViewModel);
            }

            stringBuilder.Append("<div class='text-center'><h4><strong>Agent could not be assigned to this position at the moment.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Model state is not valid. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

            positionResultViewModel.Message = stringBuilder.ToString();

            return PartialView("_FailureConfirmation", positionResultViewModel);
        }
        #endregion

        #region Schedule Interview
        /// <summary>
        /// ScheduleInterview action method retrieves all the interviews for current user and displays the schedule for the week in _ScheduleInterview partial view
        /// </summary>
        /// <param name="candidateId">Id of the candidate for whom the interview is being scheduled</param>
        /// <param name="displayDate">Schedule is displayed for the week of this date</param>
        /// <param name="selected">Indicates whether Candidate has previously scheduled interview</param>
        /// <returns>_ScheduleInterview partial view with the schedule for the week</returns>
        [HttpGet]
        public async Task<PartialViewResult> ScheduleInterview(int candidateId, string displayDate, bool selected)
        {
            var interviewScheduleViewModel = new InterviewScheduleViewModel(candidateId, displayDate, selected);

            var response = await this.GetHttpClient().GetAsync(String.Format("Interview?username={0}", User.Identity.Name));

            if (response.IsSuccessStatusCode)
            {
                interviewScheduleViewModel.Interviews.AddRange(Mapper.Map<List<Interview>, List<InterviewListViewModel>>(await response.Content.ReadAsAsync<List<Interview>>()));

                return PartialView("_ScheduleInterview", interviewScheduleViewModel);
            }

            return PartialView("_ScheduleInterview");
        }

        /// <summary>
        /// ScheduleInterview action method attempts to persist the interview
        /// </summary>
        /// <param name="model">Data required to persist the interview</param>
        /// <returns>_SuccessConfirmation partial view if an interview has been successfully persisted, _FailureConfirmation partial view otherwise</returns>
        [HttpPost]
        public async Task<PartialViewResult> ScheduleInterview(InterviewScheduleViewModel model)
        {
            var stringBuilder = new StringBuilder();

            var positionResultViewModel = new PositionResultViewModel();

            if (model.CandidateId  > 0 && !String.IsNullOrEmpty(model.SelectedDateTime))
            {
                var response = new HttpResponseMessage();

                try
                {
                    response = await this.GetHttpClient().PostAsync(String.Format("Interview?candidateId={0}&selectedDate={1}&agentUserName={2}&selected={3}", model.CandidateId, model.SelectedDateTime, User.Identity.Name, model.Selected), null); // Attempt to create new interview

                    if (response.IsSuccessStatusCode)
                    {
                        var interview = await response.Content.ReadAsAsync<Interview>();

                        var notification = Mapper.Map<NotificationAddViewModel, Notification>(new NotificationAddViewModel("Interview Scheduling Confirmation", String.Format("Interview with '{0}' ({1}) has been successfully scheduled on {2}.", String.Format("{0} {1}", interview.Candidate.FirstName, interview.Candidate.LastName), interview.Candidate.CandidateId.ToString("CAN00000"), String.Format("{0} at {1}", interview.InterviewDate.ToString("dddd, MMMM dd, yyyy"), interview.InterviewDate.ToString("hh:mm tt"))), interview.Interviewer.AgentId));

                        response = await this.GetHttpClient().PostAsJsonAsync("Notification", notification); // Attempt to persist notification to the data context

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new NotificationAddException(String.Format("Notification could NOT be added. Status Code: {1}", response.StatusCode));
                        }

                        // Send email
                        var template = HttpContext.Server.MapPath("~/App_Data/ScheduleInterviewEmailTemplate.txt");

                        var message = System.IO.File.ReadAllText(template);

                        message = message.Replace("%name%", interview.Candidate.FirstName).Replace("%datetime%", String.Format("{0} at {1}", interview.InterviewDate.ToString("dddd, MMMM dd, yyyy"), interview.InterviewDate.ToString("hh:mm tt"))).Replace("%interviewer%", String.Format("{0} {1}", interview.Interviewer.FirstName, interview.Interviewer.LastName)).Replace("%email%", interview.Interviewer.Email);

                        // TODO - Change "atomix0x@gmail.com" to the email address of the applicant
                        Email.EmailService.SendEmail("atomix0x@gmail.com", "Your interview has been scheduled.", message); // Send interview confirmation via email

                        stringBuilder.Append(String.Format("<div class='text-center'><h4><strong>An interview with {0} {1} has been scheduled successfully!</strong></h4></div>", interview.Candidate.FirstName, interview.Candidate.LastName));

                        stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An interview can be re-scheduled at anytime by clicking on Re-Schedule button.</div>");

                        stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> An applicant has been notified via email.</div></div>");

                        positionResultViewModel.Message = stringBuilder.ToString();

                        positionResultViewModel.PositionId = interview.Candidate.PositionId;

                        positionResultViewModel.RefreshEditForm = true;

                        return PartialView("_SuccessConfirmation", positionResultViewModel);
                    }
                    else
                    {
                        // If interview could not be created, throw InterviewAddException exception
                        throw new InterviewAddException("Interview could not be created. Response: " + response.StatusCode);
                    }
                }
                catch (InterviewAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to schedule an interview.</strong></h4></div>");

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Server returned status code '{0}' while attempting to persist new interview details to the database.</div>", response.StatusCode));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Please review an exception log for more details about the exception.</div></div>");

                    positionResultViewModel.Message = stringBuilder.ToString();

                    return PartialView("_FailureConfirmation", positionResultViewModel);
                }
                catch (NotificationAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                    stringBuilder.Append("<div class='text-center'><h4><strong>Failed to create new notification.</strong></h4></div>");

                    stringBuilder.Append(String.Format("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been thrown while attempting to create new notification.</div>"));

                    stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> Please review an exception log for more information about the exception.</div></div>");

                    positionResultViewModel.Message = stringBuilder.ToString();

                    return PartialView("_FailureConfirmation", positionResultViewModel);
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                }
                catch (System.IO.IOException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));
                }
            }

            stringBuilder.Append("<div class='text-center'><h4><strong>Failed to schedule an interview.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Model state is not valid. Please try again in a moment.</div>");

            stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'><strong>NOTE:</strong> If you encounter this issue again in the future, please contact Technical Support with exact steps to reproduce this issue.</div></div>");

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