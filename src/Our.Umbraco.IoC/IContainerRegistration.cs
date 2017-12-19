using System;

namespace Our.Umbraco.IoC
{
    public interface IContainerRegistration
    {
        Lifetime Lifetime { get; }
        Type Type { get; }
    }
}