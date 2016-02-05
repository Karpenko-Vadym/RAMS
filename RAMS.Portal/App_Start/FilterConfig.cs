using System.Web;
using System.Web.Mvc;

namespace RAMS.Portal
{
    /// <summary>
    /// FilterConfig class allow to configure global filters for the application
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// RegisterGlobalFilters mothod registers global filters that will be applied on every action and controller
        /// </summary>
        /// <param name="filters">Collection of global filters</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
