using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace RAMS.Web.App_Start
{
    /// <summary>
    /// BundleConfig class allows to register multiple css and js files under one bundle
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// RegisterBundles method is called when application starts in order to register bundles
        /// </summary>
        /// <param name="bundles">Collection of bundles</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // JaveScript files
            bundles.Add(new ScriptBundle("~/bootstrap/js").Include("~/Content/JS/jquery-{version}.js", "~/Content/JS/bootstrap.js", "~/Content/JS/modernizr-2.6.2.js", "~/Content/JS/jquery.unobtrusive-ajax.js", "~/Content/JS/site.js"));

            bundles.Add(new ScriptBundle("~/dataTables/js").Include("~/Content/JS/jquery.dataTables.js", "~/Content/JS/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/isLoading/js").Include("~/Content/JS/jquery.isloading.js"));

            //CSS files
            bundles.Add(new StyleBundle("~/bootstrap/css").Include("~/Content/CSS/bootstrap.css", "~/Content/CSS/site.css"));

            bundles.Add(new StyleBundle("~/dataTables/css").Include("~/Content/CSS/dataTables.bootstrap.css"));

            bundles.Add(new StyleBundle("~/isLoading/css").Include("~/Content/CSS/isloading.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}