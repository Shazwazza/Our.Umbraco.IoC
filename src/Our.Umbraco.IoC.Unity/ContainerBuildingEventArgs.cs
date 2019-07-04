using System;
using Umbraco.Core;
using Unity;

namespace Our.Umbraco.IoC.Unity
{
    public class ContainerBuildingEventArgs : EventArgs
    {
        public ContainerBuildingEventArgs(IUnityContainer container, ApplicationContext appCtx, UmbracoApplicationBase umbApp)
        {
            Container = container;
            AppCtx = appCtx;
            UmbApp = umbApp;
        }

        public IUnityContainer Container { get; }
        public ApplicationContext AppCtx { get; }
        public UmbracoApplicationBase UmbApp { get; }
    }
}