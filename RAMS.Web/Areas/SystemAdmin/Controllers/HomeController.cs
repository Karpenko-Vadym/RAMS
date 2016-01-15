using RAMS.Email;
using RAMS.Models;
using RAMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RAMS.Web.Areas.SystemAdmin.Controllers
{
    /// <summary>
    /// HomeController controller will be (By default) accessed as soon as user navigates (Or gets redirected) to SystemAdmin area by it's root URL
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index action method will be called as soon as user navigates (Or gets redirected) to SystemAdmin area by it's root URL
        /// User will be redirected to Index method of Profile controller if user's UserType is Admin, otherwise user will be redirected to appropriate location depending on his/her UserType
        /// </summary>
        /// <returns>Redirects to Index method of Profile controller if user's UserType is Admin, otherwise user will be redirected to appropriate location depending on his/her UserType</returns>
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
                return RedirectToAction("Index", "Profile", new { Area = "SystemAdmin" });
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}