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
using RAMS.Web.Identity;
using RAMS.Enums;
using RAMS.ViewModels;
using RAMS.Models;
using RAMS.Web.Controllers;
using RAMS.Helpers;

namespace RAMS.Web.Areas.Customer.Controllers
{
    public class ProfileController : BaseController
    {
        
        public ActionResult Index()
        {
            //var identity = User.Identity as ClaimsIdentity;

            //if (identity.HasClaim("UserType", "Agent"))
            //{
            //    return RedirectToAction("Index", "Home", new { Area = "Agency" });
            //}
            //else if (identity.HasClaim("UserType", "Client"))
            //{
                return View();
            //}
            //else if (identity.HasClaim("UserType", "Admin"))
            //{
            //    return RedirectToAction("Index", "User", new { Area = "SystemAdmin" });
            //}

            //return RedirectToAction("Index", "Home", new { Area = "" });
        }

        #region Profile Details

        [HttpGet]
        public async Task<PartialViewResult> ProfileDetails()
        {
            var response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", User.Identity.Name));

            if (response.IsSuccessStatusCode)
            {
                var clientProfileDetailsViewModel = Mapper.Map<Client, ClientProfileDetailsViewModel>(await response.Content.ReadAsAsync<Client>());

                return PartialView("_ProfileDetails", clientProfileDetailsViewModel);
            }

            return PartialView("_ProfileDetails");
        }
        #endregion
    }
}