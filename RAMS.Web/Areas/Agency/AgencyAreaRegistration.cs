using System.Web.Mvc;

namespace RAMS.Web.Areas.Agency
{
    public class AgencyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Agency";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Agency_default",
                "Agency/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "RAMS.Web.Areas.Agency.Controllers" }
            );
        }
    }
}