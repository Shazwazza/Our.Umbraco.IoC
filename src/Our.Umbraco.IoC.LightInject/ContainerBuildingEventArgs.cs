using System;
using LightInject;
using Umbraco.Core;

namespace Our.Umbraco.IoC.LightInject
{
    public class ContainerBuildingEventArgs : EventArgs
    {
        public ContainerBuildingEventArgs(ServiceContainer container, ApplicationContext appCtx, UmbracoApplicationBase umbApp)
        {
            Container = container;
            AppCtx = appCtx;
            UmbApp = umbApp;
        }

        public ServiceContainer Container { get; }
        public ApplicationContext AppCtx { get; }
        public UmbracoApplicationBase UmbApp { get; }
    }
}