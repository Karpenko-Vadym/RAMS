using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RAMS.Web
{
    /// <summary>
    /// RouteConfig class allow to configure URL routing for the application
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// RegisterRoutes method register default/custom URL routes
        /// </summary>
        /// <param name="routes">Route collection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "RAMS.Web.Controllers" }
            );
        }
    }
}
