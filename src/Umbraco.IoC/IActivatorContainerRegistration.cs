using System;

namespace Umbraco.IoC
{
    public interface IActivatorContainerRegistration
    {
        Func<Resolver, object> Activator { get; }
    }
}