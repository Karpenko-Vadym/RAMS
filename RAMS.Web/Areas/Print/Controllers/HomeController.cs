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

namespace RAMS.Web.Areas.Print.Controllers
{
    /// <summary>
    /// HomeController implements print related methods
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// PositionStatusReportPrint action method gets requested position's details and passes it to PositionStatusReportPrint view
        /// </summary>
        /// <param name="positionId">Id of the position that is being fetched</param>
        /// <returns>PositionStatusReportPrint view with position details</returns>
        [HttpGet]
        public async Task<ViewResult> PositionStatusReport(int positionId)
        {
            if (positionId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Position?id={0}", positionId));

                if (response.IsSuccessStatusCode)
                {
                    var positionReportDetailsForPrintViewModel = Mapper.Map<Position, PositionReportDetailsForPrintViewModel>(await response.Content.ReadAsAsync<Position>());

                    if (positionReportDetailsForPrintViewModel.TotalCandidates > 0)
                    {
                        positionReportDetailsForPrintViewModel.TopCandidates = positionReportDetailsForPrintViewModel.Candidates.Where(c => c.Score >= positionReportDetailsForPrintViewModel.AcceptanceScore).Count();

                        positionReportDetailsForPrintViewModel.CandidatesSelected = positionReportDetailsForPrintViewModel.Candidates.Where(c => c.Selected == true).Count();

                        positionReportDetailsForPrintViewModel.AverageScore = positionReportDetailsForPrintViewModel.Candidates.Average(c => c.Score);
                    }

                    return View("PositionStatusReport", positionReportDetailsForPrintViewModel);
                }
            }

            return View("PositionStatusReport");
        }


        /// <summary>
        /// PositionFinalReportPrint action method gets requested position's details and passes it to PositionFinalReportPrint view
        /// </summary>
        /// <param name="positionId">Id of the position that is being fetched</param>
        /// <returns>PositionFinalReportPrint view with position details</returns>
        [HttpGet]
        public async Task<ViewResult> PositionFinalReport(int positionId)
        {
            if (positionId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Position?id={0}", positionId));

                if (response.IsSuccessStatusCode)
                {
                    var positionReportDetailsForPrintViewModel = Mapper.Map<Position, PositionReportDetailsForPrintViewModel>(await response.Content.ReadAsAsync<Position>());

                    if (positionReportDetailsForPrintViewModel.TotalCandidates > 0)
                    {
                        positionReportDetailsForPrintViewModel.TopCandidates = positionReportDetailsForPrintViewModel.Candidates.Where(c => c.Score >= positionReportDetailsForPrintViewModel.AcceptanceScore).Count();

                        positionReportDetailsForPrintViewModel.CandidatesSelected = positionReportDetailsForPrintViewModel.Candidates.Where(c => c.Selected == true).Count();

                        positionReportDetailsForPrintViewModel.AverageScore = positionReportDetailsForPrintViewModel.Candidates.Average(c => c.Score);
                    }

                    return View("PositionFinalReport", positionReportDetailsForPrintViewModel);
                }
            }

            return View("PositionFinalReport");
        }
    }
}