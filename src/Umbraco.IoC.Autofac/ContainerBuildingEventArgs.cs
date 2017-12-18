using System;
using Autofac;
using Umbraco.Core;

namespace Umbraco.IoC.Autofac
{
    public class ContainerBuildingEventArgs : EventArgs
    {
        public ContainerBuildingEventArgs(ContainerBuilder builder, ApplicationContext appCtx, UmbracoApplicationBase umbApp)
        {
            Builder = builder;
            AppCtx = appCtx;
            UmbApp = umbApp;
        }

        public ContainerBuilder Builder { get; }
        public ApplicationContext AppCtx { get; }
        public UmbracoApplicationBase UmbApp { get; }
    }
}