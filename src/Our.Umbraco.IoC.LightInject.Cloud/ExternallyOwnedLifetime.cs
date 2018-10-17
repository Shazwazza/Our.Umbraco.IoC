using System;
using LightInject;

namespace Our.Umbraco.IoC.LightInject.Cloud
{
    /// <summary>
    /// Used if the lifetime shouldn't be managed by the container
    /// </summary>
    /// <remarks>
    /// This is useful if you are registering managed singletons in the container. In this case the container will not be responsible for disposing, etc...
    /// </remarks>
    public class ExternallyOwnedLifetime : ILifetime
    {        
        public object GetInstance(Func<object> createInstance, Scope scope)
        {
            var instance = createInstance();
            return instance;
        }        
    }
}