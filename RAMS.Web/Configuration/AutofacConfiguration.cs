using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RAMS.Web.Configuration
{
    /// <summary>
    /// AutofacConfiguration class allows to setup Autofac configuration
    /// </summary>
    public class AutofacConfiguration
    {
        /// <summary>
        /// Configure method is called when application starts and allows to set container and register types
        /// </summary>
        public static void Configure()
        {
            var containerBuilder = new ContainerBuilder();

            var configuration = GlobalConfiguration.Configuration;

            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            containerBuilder.RegisterWebApiFilterProvider(configuration);

            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest(); // Register UnitOfWork

            containerBuilder.RegisterType<DataFactory>().As<IDataFactory>().InstancePerRequest(); // Register DataFactory

            // Register repositories
            var repositoryAssembly = typeof(AgentRepository).Assembly;

            containerBuilder.RegisterAssemblyTypes(repositoryAssembly).Where(r => r.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            // Register services
            var serviceAssembly = typeof(AgentService).Assembly;

            containerBuilder.RegisterAssemblyTypes(serviceAssembly).Where(s => s.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();

            // Build container
            IContainer container = containerBuilder.Build();

            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}