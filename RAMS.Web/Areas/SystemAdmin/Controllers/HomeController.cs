using RAMS.Email;
using RAMS.Models;
using RAMS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        /// User will be redirected to Index method of User controller
        /// </summary>
        /// <returns>Redirects to Index method of User controller</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "User", new { Area = "SystemAdmin" });
        }
    }
}