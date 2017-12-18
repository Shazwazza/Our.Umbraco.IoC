using System;

namespace Umbraco.IoC
{
    public interface IContainerRegistration
    {
        Lifetime Lifetime { get; }
        Type Type { get; }
    }
}