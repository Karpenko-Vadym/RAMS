using RAMS.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Web.App_Start
{
    /// <summary>
    /// InitializationConfig class allows to run (Configure) all custom configurations
    /// </summary>
    public class InitializationConfig
    {
        /// <summary>
        /// Initialize method runs all the custom configurations once application starts
        /// </summary>
        public static void Initialize()
        {
            AutofacConfiguration.Configure(); // Configure Autofac

            AutoMapperConfiguration.Configure(); // Configure AutoMapper
        }
    }
}