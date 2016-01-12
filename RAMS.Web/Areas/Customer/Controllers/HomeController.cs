using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace RAMS.Web.Areas.Customer.Controllers
{
    public class HomeController : Controller
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
                return RedirectToAction("Index", "Profile", new { Area = "Customer" });
            //}
            //else if (identity.HasClaim("UserType", "Admin"))
            //{
            //    return RedirectToAction("Index", "User", new { Area = "SystemAdmin" });
            //}

            //return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}