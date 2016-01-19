using System.Web;
using System.Web.Optimization;

namespace RAMS.Portal
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
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Content/JS/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Content/JS/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Content/JS/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Content/JS/bootstrap.js", "~/Content/JS/respond.js"));

            //CSS files
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/CSS/bootstrap.css", "~/Content/CSS/site.css"));
        }
    }
}
