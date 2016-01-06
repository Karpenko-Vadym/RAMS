using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RAMS.Web.Startup))]
namespace RAMS.Web
{
    /// <summary>
    /// Startup class allows to configure OWIN application pipeline
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configuration method is executed when WebAPI is invoked
        /// </summary>
        /// <param name="app">IAppBuilder is used to add components into the Owin application pipeline</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
