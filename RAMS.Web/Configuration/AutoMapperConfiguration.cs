using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Web.Configuration
{
    /// <summary>
    /// AutoMapperConfiguration class allows to setup AutoMapper configuration
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// Configure method is called when application starts and allows to setup custom mappings for AutoMapper
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(m => { m.AddProfile<ModelToViewModelMapping>(); m.AddProfile<ViewModelToModelMapping>(); });
        }
    }
}