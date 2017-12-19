using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Umbraco.Core;
using Umbraco.Web;

namespace Our.Umbraco.IoC.Autofac
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
            if (ConfigurationManager.AppSettings["Our.Umbraco.IoC.Autofac.Enabled"] != null && ConfigurationManager.AppSettings["Our.Umbraco.IoC.Autofac.Enabled"] != "true")
                return;

            var builder = new ContainerBuilder();

            //register ASP.NET System types
            builder.RegisterModule<AutofacWebTypesModule>();
            
            //register umbraco types
            builder.RegisterSource(new AutofacUmbracoRegister(UmbracoServices.GetAllRegistrations()));

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

