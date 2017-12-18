using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Core.Security;
using Umbraco.Core.Models.Identity;
using System.Web;
using Umbraco.Core.Services;
using Umbraco.Web.HealthCheck;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.HealthChecks;

namespace Umbraco.IoC.Autofac
{
    public class AutofacStartup : IApplicationEventHandler
    {
        public static EventHandler<ContainerBuildingEventArgs> ContainerBuilding;

        private void OnContainerBuilding(ContainerBuildingEventArgs args)
        {
            if (ContainerBuilding != null)
                ContainerBuilding(this, args);
        }
        
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //If this flag exists and it's not 'true' then this container will be disabled.
            if (ConfigurationManager.AppSettings["Umbraco.IoC.Autofac.Enabled"] != null && ConfigurationManager.AppSettings["Umbraco.IoC.Autofac.Enabled"] != "true")
                return;

            var builder = new ContainerBuilder();

            //register ASP.NET System types
            builder.RegisterModule<AutofacWebTypesModule>();
            
            //register umbraco types
            builder.RegisterSource(new AutofacUmbracoRegister(CoreServices.GetContainerRegistrations()));
            builder.RegisterSource(new AutofacUmbracoRegister(WebServices.GetContainerRegistrations()));

            //register umbraco MVC + webapi controllers used by the admin site
            builder.RegisterControllers(typeof(UmbracoApplication).Assembly);
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);

            //Raise event so people can modify the container
            OnContainerBuilding(new ContainerBuildingEventArgs(builder, applicationContext, umbracoApplication));

            var container = builder.Build();

            //set dependency resolvers for both webapi and mvc
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}

