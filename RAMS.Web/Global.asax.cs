using RAMS.Helpers;
using RAMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using RAMS.Web.App_Start;
using System.Web.Optimization;

namespace RAMS.Web
{
    /// <summary>
    /// MvcApplication class implements global application configuration
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application_Start method is executed when application starts and it is used to configure all the components of the application
        /// </summary>
        protected void Application_Start()
        {
            // Register ViewEngine
            ViewEngines.Engines.Add(new ExtendedViewEngine());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Formatters configuration (Remove XML and avoid self-referencing loop)
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            // Register bundles
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initializa all the custom configuration
            InitializationConfig.Initialize();
        }
    }
}
