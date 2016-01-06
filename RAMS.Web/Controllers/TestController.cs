using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AutoMapper;
using System.Net.Http;
using System.Collections.Generic;

using RAMS.Web.Identity;
using RAMS.Enums;
using RAMS.ViewModels;
using RAMS.Models;
using RAMS.Web.Controllers;
using RAMS.Helpers;

namespace RAMS.Web.Controllers
{
    public class TestController : BaseController
    {
        // GET: Test
        public async Task Index()
        {
            var response = await this.GetHttpClient().GetAsync("Agent?id=1");

            if(response.IsSuccessStatusCode)
            {
                var agent = await response.Content.ReadAsAsync<Agent>();

                if(agent != null)
                {
                    agent.FirstName = "Updated";

                    response = await this.GetHttpClient().PutAsJsonAsync("Agent", agent);

                    if (response.IsSuccessStatusCode)
                    {
                        agent = await response.Content.ReadAsAsync<Agent>();

                        Console.Write("Success");
                    }
                }
            }

        }
    }
}