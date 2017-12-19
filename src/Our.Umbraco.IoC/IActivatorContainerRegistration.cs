using System;

namespace Our.Umbraco.IoC
{
    public interface IActivatorContainerRegistration
    {
        Func<Resolver, object> Activator { get; }
    }
}