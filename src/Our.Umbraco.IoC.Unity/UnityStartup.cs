using System;
using System.Configuration;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Core;
using Unity;
using Unity.AspNet.Mvc;

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
            //If this flag exists and it's not 'true' then this container will be disabled.
            if (ConfigurationManager.AppSettings["Our.Umbraco.IoC.Unity.Enabled"] != null && ConfigurationManager.AppSettings["Our.Umbraco.IoC.Unity.Enabled"] != "true")
                return;

            var container = new UnityContainer();

            //register for shutdown
            HostingEnvironment.RegisterObject(new UnityShutdown(container));

            //register ASP.NET System types
            container.RegisterWebTypes();

            //register umbraco types
            var umbracoRegistrations = new UnityUmbracoRegister(container, UmbracoServices.GetAllRegistrations());
            umbracoRegistrations.RegisterTypes();

            //it is NOT necessary to register your controllers for Unity!
            //see https://code.msdn.microsoft.com/Dependency-Injection-in-11d54863

            //Raise event so people can modify the container
            OnContainerBuilding(new ContainerBuildingEventArgs(container, applicationContext, umbracoApplication));

            //set dependency resolvers for both webapi and mvc
            GlobalConfiguration.Configuration.DependencyResolver = new global::Unity.AspNet.WebApi.UnityDependencyResolver(container);
            DependencyResolver.SetResolver(new global::Unity.AspNet.Mvc.UnityDependencyResolver(container));

            //custom MVC requirements for unity
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
        }

        /// <summary>
        /// Disposes the container when the application terminates
        /// </summary>
        private class UnityShutdown : IRegisteredObject
        {
            private IUnityContainer _container;

            public UnityShutdown(IUnityContainer container)
            {
                _container = container;
            }
            public void Stop(bool immediate)
            {
                _container?.Dispose();
                _container = null;
            }
        }
    }
}
