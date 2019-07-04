using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web.Editors;
using Umbraco.Web.HealthCheck;
using Umbraco.Web.Trees;
using Unity;
using Unity.Injection;

namespace Our.Umbraco.IoC.Unity
{
    public class UnityStartup : IApplicationEventHandler
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
            ////If this flag exists and it's not 'true' then this container will be disabled.
            if (ConfigurationManager.AppSettings["Our.Umbraco.IoC.Unity.Enabled"] != null && ConfigurationManager.AppSettings["Our.Umbraco.IoC.Unity.Enabled"] != "true")
                return;

            var container = UnityConfig.GetConfiguredContainer();
            container.RegisterWebTypes();

            ////register umbraco types
            var umbracoRegistrations = new UnityUmbracoRegister(container, UmbracoServices.GetAllRegistrations());
            umbracoRegistrations.RegisterTypes();

            ////Raise event so people can modify the container
            OnContainerBuilding(new ContainerBuildingEventArgs(container, applicationContext, umbracoApplication));

            ////set dependency resolvers for both webapi and mvc
            GlobalConfiguration.Configuration.DependencyResolver = new global::Unity.AspNet.WebApi.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new global::Unity.AspNet.Mvc.UnityDependencyResolver(container));
        }
    }
}
