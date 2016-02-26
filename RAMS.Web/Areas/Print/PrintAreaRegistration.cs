using System.Web.Mvc;

namespace RAMS.Web.Areas.Print
{
    public class PrintAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Print";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Print_default",
                "Print/{action}/{positionId}",
                new { controller = "Home", action = "Index", positionId = UrlParameter.Optional }
            );  
        }
    }
}