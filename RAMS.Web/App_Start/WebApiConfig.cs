using System.Web.Http;

class WebApiConfig
{
    /// <summary>
    /// Register method allows to register "api/ControllerName/Parameters" path for WebAPI
    /// </summary>
    /// <param name="configuration">Http configuration that allows to register path for WebAPI</param>    
    public static void Register(HttpConfiguration configuration)
    {
        configuration.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
    }
}