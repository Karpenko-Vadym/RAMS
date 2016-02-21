using RAMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace RAMS.Web.Areas.Customer.Controllers
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
                return RedirectToAction("Index", "Home", new { Area = "Agency" });
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                return View();
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = "SystemAdmin" });
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }





    }
}