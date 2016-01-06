using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RAMS.Helpers
{
    /// <summary>
    /// ExtendedViewEngine allows to specify to the razor view engine which directories to search when looking for specific content
    /// </summary>
    public class ExtendedViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Default constructor that enables to specify the paths for views, partial views, and master locations
        /// </summary>
        public ExtendedViewEngine() : base()
        {
            this.ViewLocationFormats = new string[] { "~/Views/Identity/{1}/{0}.cshtml", "~/Views/Identity/{1}/{0}.vbhtml" };

            this.PartialViewLocationFormats = new string[] { "~/Views/Identity/{1}/{0}.cshtml", "~/Views/Identity/{1}/{0}.vbhtml" };

            this.MasterLocationFormats = new string[] { "~/Views/Identity/{1}/{0}.cshtml", "~/Views/Identity/{1}/{0}.vbhtml" };
        }

        /// <summary>
        /// Methor that creates partial view
        /// </summary>
        /// <param name="controllerContext">Controller context which allows to determine which view is being called</param>
        /// <param name="partialPath">Path to partial view</param>
        /// <returns>Partial view</returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
 	            return base.CreatePartialView(controllerContext, partialPath);
        }

        /// <summary>
        /// Method that creates view
        /// </summary>
        /// <param name="controllerContext">Controller context which allows to determine which view is being called</param>
        /// <param name="viewPath">Path to view</param>
        /// <param name="masterPath">Path to master location</param>
        /// <returns>View</returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
 	            return base.CreateView(controllerContext, viewPath, masterPath);
        }    
    }
}
