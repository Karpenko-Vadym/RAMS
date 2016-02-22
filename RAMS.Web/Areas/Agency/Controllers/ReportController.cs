using AutoMapper;
using RAMS.Models;
using RAMS.ViewModels;
using RAMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RAMS.Web.Areas.Agency.Controllers
{
    /// <summary>
    /// ReportController implements report related methods
    /// </summary>
    public class ReportController : BaseController
    {
        /// <summary>
        /// Default action method that returns main view of Report controller
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <returns>Main view of Report controller</returns>
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
            var positions = new List<PositionListForReportViewModel>();

            var response = new HttpResponseMessage();

            response = await this.GetHttpClient().GetAsync("Position");

            if (response.IsSuccessStatusCode)
            {
                positions.AddRange(Mapper.Map<List<Position>, List<PositionListForReportViewModel>>(await response.Content.ReadAsAsync<List<Position>>()));

                return PartialView("_PositionList", positions);
            }

            return PartialView("_PositionList");
        }
        #endregion

        #region Position Report
        /// <summary>
        /// PositionStatusReport action method gets requested position's details and passes it to _PositionStatusReport partial view
        /// </summary>
        /// <param name="positionId">Id of the position that is being fetched</param>
        /// <returns>_PositionStatusReport partial view with position details</returns>
        [HttpGet]
        public async Task<PartialViewResult> PositionStatusReport(int positionId)
        {
            if (positionId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Position?id={0}", positionId));

                if (response.IsSuccessStatusCode)
                {
                    var positionEditViewModel = Mapper.Map<Position, PositionReportDetailsViewModel>(await response.Content.ReadAsAsync<Position>());

                    return PartialView("_PositionStatusReport", positionEditViewModel);
                }
            }

            return PartialView("_PositionStatusReport");
        }

        /// <summary>
        /// PositionFinalReport action method gets requested position's details and passes it to _PositionFinalReport partial view
        /// </summary>
        /// <param name="positionId">Id of the position that is being fetched</param>
        /// <returns>_PositionFinalReport partial view with position details</returns>
        [HttpGet]
        public async Task<PartialViewResult> PositionFinalReport(int positionId)
        {
            if (positionId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Position?id={0}", positionId));

                if (response.IsSuccessStatusCode)
                {
                    var positionEditViewModel = Mapper.Map<Position, PositionReportDetailsViewModel>(await response.Content.ReadAsAsync<Position>());

                    return PartialView("_PositionFinalReport", positionEditViewModel);
                }
            }

            // TODO - Add mapping for PositionReportDetailsViewModel

            return PartialView("_PositionFinalReport");
        }

        #endregion

    }
}