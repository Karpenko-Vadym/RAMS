using System.Web.Mvc;

namespace RAMS.Web.Areas.SystemAdmin
{
    /// <summary>
    /// SystemAdminAreaRegistration class allows to register SystemAdmin area
    /// </summary>
    public class SystemAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SystemAdmin";
            }
        }

        /// <summary>
        /// RegisterArea method allows to access MapRoute method of AreaRegistrationContext class and configure routing for SystemAdmin area
        /// </summary>
        /// <param name="context">Area registration context</param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SystemAdmin_default",
                "SystemAdmin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "RAMS.Web.Areas.SystemAdmin.Controllers" }
            );
        }
    }
}