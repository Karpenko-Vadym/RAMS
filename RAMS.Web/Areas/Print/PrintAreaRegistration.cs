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
            //context.MapRoute(
            //    "Print_default",
            //    "Print/{controller}/{action}/{id}",
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    new string[] { "RAMS.Web.Areas.Print.Controllers" }
            //);
            context.MapRoute(
                "Print_default",
                "Print/{action}/{positionId}",
                new { controller = "Home", action = "Index", positionId = UrlParameter.Optional }
            );  
        }
    }
}