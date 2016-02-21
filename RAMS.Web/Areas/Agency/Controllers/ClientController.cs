using RAMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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








    }
}