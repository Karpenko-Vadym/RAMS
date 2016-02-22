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
    /// ClientController implements client related methods
    /// </summary>
    public class ClientController : BaseController
    {
        /// <summary>
        /// Default action method that returns main view of Client controller
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <returns>Main view of Client controller</returns>
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

        #region Client List
        /// <summary>
        /// ClientList action method gets the list of all the clients and passes it to _ClientList partial view
        /// </summary>
        /// <returns>_ClientList partial view with the list of all the clients</returns>
        [HttpGet]
        public async Task<PartialViewResult> ClientList()
        {
            var identity = User.Identity as ClaimsIdentity;

            var clients = new List<ClientListViewModel>();

            if (identity.HasClaim("Role", "Manager"))
            {
                var response = await this.GetHttpClient().GetAsync("Client");

                if (response.IsSuccessStatusCode)
                {
                    clients.AddRange(Mapper.Map<List<Client>, List<ClientListViewModel>>(await response.Content.ReadAsAsync<List<Client>>()));

                    return PartialView("_ClientList", clients);
                }
            }

            return PartialView("_ClientList");
        }
        #endregion

        #region Client Details
        /// <summary>
        /// ClientDetails action method gets requested client's details and passes it to _ClientDetails partial view
        /// </summary>
        /// <param name="clientId">Id of the client that is being fetched</param>
        /// <returns>_ClientDetails partial view with client details</returns>
        [HttpGet]
        public async Task<PartialViewResult> ClientDetails(int clientId)
        {
            if (clientId > 0)
            {
                var response = await this.GetHttpClient().GetAsync(String.Format("Client?id={0}", clientId));

                if (response.IsSuccessStatusCode)
                {
                    var clientDetailsViewModel = Mapper.Map<Client, ClientDetailsViewModel>(await response.Content.ReadAsAsync<Client>());

                    return PartialView("_ClientDetails", clientDetailsViewModel);
                }
            }

            return PartialView("_ClientDetails");
        }
        #endregion
    }
}