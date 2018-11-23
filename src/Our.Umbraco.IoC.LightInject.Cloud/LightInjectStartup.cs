using System;
using System.Configuration;
using System.Web.Http;
using LightInject;
using Umbraco.Core;
using Umbraco.Web;

namespace Our.Umbraco.IoC.LightInject.Cloud
{
    public class LightInjectStartup : IApplicationEventHandler
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
            if (ConfigurationManager.AppSettings["Our.Umbraco.IoC.LightInject.Cloud.Enabled"] != null && ConfigurationManager.AppSettings["Our.Umbraco.IoC.LightInject.Cloud.Enabled"] != "true")
                return;

            var container = new ServiceContainer();

            container.EnablePerWebRequestScope();

            //register ASP.NET System types
            container.RegisterFrom<LightInjectWebTypesCompositionRoot>();

            //register umbraco types
            container.RegisterFrom<LightInjectUmbracoRegister>();

            //register umbraco MVC + webapi controllers used by the admin site
            container.RegisterControllers(typeof(UmbracoApplication).Assembly);
            container.RegisterApiControllers(typeof(UmbracoApplication).Assembly);

            //Raise event so people can modify the container
            OnContainerBuilding(new ContainerBuildingEventArgs(container, applicationContext, umbracoApplication));

            //set dependency resolvers for both webapi and mvc
            container.EnableMvc();
            container.EnableWebApi(GlobalConfiguration.Configuration);
        }
    }
}
