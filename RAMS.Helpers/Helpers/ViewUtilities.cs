using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RAMS.Helpers
{
    /// <summary>
    /// ViewUtilities contains various utilities for views
    /// </summary>
    public static class ViewUtilities
    {

        /// <summary>
        /// IsActive method allows to determine active controller and action method in order to mark current menu selection
        /// </summary>
        /// <param name="html">Html helper that allows to access route data</param>
        /// <param name="controller">Controller name to be compared to current controller name</param>
        /// <param name="action">Action name to be compared to current action name</param>
        /// <returns>Returns string "active" if both controller name and action name passed as parameters match to current controller name and action name</returns>
        public static string IsActive(this HtmlHelper html, string controller, string action = "")
        {
            var result = "";

            var routeData = html.ViewContext.RouteData;

            if (String.IsNullOrEmpty(action))
            {
                if ((string)routeData.Values["controller"] == controller)
                {
                    result = "active";
                }
            }
            else
            {
                if ((string)routeData.Values["controller"] == controller && (string)routeData.Values["action"] == action)
                {
                    result = "active";
                }
            }

            return result;
        }
    }
}
