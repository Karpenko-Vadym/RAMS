using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RAMS.Web.Controllers
{
    /// <summary>
    /// BaseController controller implements common methods for accessing WebAPI via HttpClient
    /// </summary>
    public class BaseController : Controller
    {
        protected HttpClient HttpClient { get; set; }

        /// <summary>
        /// BaseController constructor initializes and configures HttpClient allowing controller that inherit from BaseController to access WebAPI using already configured HttpClient 
        /// </summary>
        public BaseController()
        {
            this.HttpClient = new HttpClient();

            var siteUrl = System.Web.HttpContext.Current.Request.Url.LocalPath.ToString();

            if (siteUrl != "/")
            {
                siteUrl = siteUrl.Substring(siteUrl.IndexOf('/') + 1);

                siteUrl = siteUrl.Substring(0, siteUrl.IndexOf('/'));
            }

            this.HttpClient.BaseAddress = new Uri(string.Format("{0}://{1}{2}/api/", System.Web.HttpContext.Current.Request.Url.Scheme.ToString(), System.Web.HttpContext.Current.Request.Url.Host.ToString(), System.Web.HttpContext.Current.Request.Url.Port.ToString() == "80" || System.Web.HttpContext.Current.Request.Url.Port.ToString() == "443" ? siteUrl == "/" ? string.Empty : "/" + siteUrl : ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString()));

            this.HttpClient.DefaultRequestHeaders.Accept.Clear();

            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// GetHttpClient method allows to access HttpClient (HttpClient getter)
        /// </summary>
        /// <returns>Already configured instance of an HttpClient</returns>
        public HttpClient GetHttpClient()
        {
            return this.HttpClient;
        }
    }
}