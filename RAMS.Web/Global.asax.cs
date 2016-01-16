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

        /// <summary>
        /// Application_BeginRequest method is executed at the begining of each request
        /// </summary>
        protected void Application_BeginRequest()
        {
            // Server Certificate is needed to be able to use HTTPS
            // While Self-Signes Certificate work, browsers still complain about insecurity
            // In order to avoid such errors, we only allow HTTP requiests (We can allow HTTPS requests if we get Verified Certificate)
            if(Context.Request.IsSecureConnection) 
            {
                Response.Redirect(Context.Request.Url.ToString().Replace("https", "http")); 
            }
        }
    }
}
