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
    /// AgentController implements agent related methods
    /// </summary>
    public class AgentController : BaseController
    {
        /// <summary>
        /// Default action method that returns main view of Agent controller
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <returns>Main view of Agent controller</returns>
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

        #region Agent List
        /// <summary>
        /// AgentList action method gets the list of all the agents and passes it to _AgentList partial view
        /// </summary>
        /// <returns>_AgentList partial view with the list of all the agents</returns>
        [HttpGet]
        public async Task<PartialViewResult> AgentList()
        {
            var identity = User.Identity as ClaimsIdentity;

            var agents = new List<AgentListViewModel>();

            if (identity.HasClaim("Role", "Manager"))
            {
                var response = await this.GetHttpClient().GetAsync("Agent");

                if (response.IsSuccessStatusCode)
                {
                    agents.AddRange(Mapper.Map<List<Agent>, List<AgentListViewModel>>((await response.Content.ReadAsAsync<List<Agent>>()).Where(a => a.UserStatus == Enums.UserStatus.Active).ToList()));

                    return PartialView("_AgentList", agents);
                }
            }

            return PartialView("_AgentList");
        }
        #endregion

        #region Agent Details
        /// <summary>
        /// AgentDetails action method gets requested agent's details and passes it to _AgentDetails partial view
        /// </summary>
        /// <param name="agentId">Id of the agent that is being fetched</param>
        /// <returns>_AgentDetails partial view with agent details</returns>
        [HttpGet]
        public async Task<PartialViewResult> AgentDetails(int agentId)
        {
            if (agentId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Agent?id={0}", agentId));

                if (response.IsSuccessStatusCode)
                {
                    var agent = await response.Content.ReadAsAsync<Agent>();

                    var agentDetailsViewModel = Mapper.Map<Agent, AgentDetailsViewModel>(agent);

                    agentDetailsViewModel.InterviewsCompleted = agent.Interviews.Where(i => i.Status == Enums.InterviewStatus.Complete).Count();

                    agentDetailsViewModel.InterviewsPending = agent.Interviews.Where(i => i.Status == Enums.InterviewStatus.Scheduled).Count();

                    return PartialView("_AgentDetails", agentDetailsViewModel);
                }
            }

            return PartialView("_AgentDetails");
        }
        #endregion
    }
}