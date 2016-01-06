using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using RAMS.Models;
using System.Security.Claims;

namespace RAMS.Web.Controllers
{
    /// <summary>
    /// HomeController controller will be (By default) accessed as soon as application starts, or user navigates to the application by it's root URL
    /// </summary>
    public class HomeController : Controller
    {
        public HomeController() : base() { }

        /// <summary>
        /// Index action method will be called as soon as application starts, or user navigates to the application by it's root URL
        /// Once authenticated, user will be redirected to default controller and default action of the area where user belongs (UserType determines which area user belongs to)
        /// </summary>
        /// <returns>Redirects to default controller and default action of the area where user belongs</returns>
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
            else if (identity.HasClaim("UserType", "Admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = "SystemAdmin" });
            }

            return RedirectToAction("Index", "Home", new { Area = "Portal" });
        }
    }
}