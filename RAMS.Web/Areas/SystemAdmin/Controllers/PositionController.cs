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

namespace RAMS.Web.Areas.SystemAdmin.Controllers
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
                return RedirectToAction("Index", "Home", new { Area = "Customer" });
            }
            else if (identity.HasClaim("UserType", "Admin") && identity.HasClaim("FullName", "superuser"))
            {
                return View();
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        #region Position Delete
        /// <summary>
        /// PositionDelete action method gets the list of all the positions and passes it to _PositionDelete partial view
        /// </summary>
        /// <param name="months">Number of months after position has been closed</param>
        /// <returns>_PositionDelete partial view with the list of all the positions</returns>
        [HttpGet]
        public async Task<PartialViewResult> PositionDelete(int months = 0)
        {

            var positionListForDeleteViewModel = new PositionListForDeleteViewModel(months);

            var response = await this.GetHttpClient().GetAsync("Position");

            if (response.IsSuccessStatusCode)
            {
                positionListForDeleteViewModel.Positions.AddRange(Mapper.Map<List<Position>, List<PositionListViewModel>>((await response.Content.ReadAsAsync<List<Position>>())).Where(p => p.CloseDate < DateTime.Now.AddMonths(months * (-1)) && p.Status == PositionStatus.Closed));

                return PartialView("_PositionDelete", positionListForDeleteViewModel);
            }
            

            return PartialView("_PositionDelete");
        }

        /// <summary>
        /// PositionDelete action method attempts to archive and delete selected Positions
        /// </summary>
        /// <param name="selectedIds">Ids of the Positions to be archived/deleted</param>
        /// <returns>_SuccessConfirmation partial view upon success, _SelectionFailureConfirmation or _FailureConfirmation otherwise</returns>
        [HttpPost]
        public async Task<PartialViewResult> PositionDelete(int[] selectedIds)
        {
            if(selectedIds != null)
            {
                string url = String.Format("Position?positionIds={0}", selectedIds[0]);

                for(int i = 1; i < selectedIds.Length; i++)
                {
                    url = String.Format("{0}&positionIds={1}", url, selectedIds[i]);
                }

                var response = await this.GetHttpClient().DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return PartialView("_SuccessConfirmation");
                }

                return PartialView("_FailureConfirmation");
            }

            return PartialView("_SelectionFailureConfirmation");
        }

        /// <summary>
        /// PositionDeleteConfirmation action method is a getter for _PositionDeleteConfirmation partial view
        /// </summary>
        /// <returns>_PositionDeleteConfirmation partial view</returns>
        public PartialViewResult PositionDeleteConfirmation()
        {
            return PartialView("_PositionDeleteConfirmation");
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
    }
}