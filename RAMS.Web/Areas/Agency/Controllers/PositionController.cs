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


        [HttpPost]
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
                // TODO - Continue here...
            }

             

            return PartialView("_EditPosition");
        }
    }
}