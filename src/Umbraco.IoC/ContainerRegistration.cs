using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.IoC
{
    
    /// <summary>
    /// Represents a registration to be added to a container
    /// </summary>
    public class ContainerRegistration : IContainerRegistration
    {
        public ContainerRegistration(Lifetime lifetime, Type type)
        {
            Lifetime = lifetime;
            Type = type;
        }

        public Lifetime Lifetime { get; private set; }
        public Type Type { get; private set; }        
    }

    /// <summary>
    /// Represents a registration to be added to a container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContainerRegistration<T> : ContainerRegistration, IActivatorContainerRegistration
    {
        public ContainerRegistration(Lifetime lifetime) : base(lifetime, typeof(T))
        {
        }

        public ContainerRegistration(Lifetime lifetime, Func<Resolver, object> activator) : base(lifetime, typeof(T))
        {
            Activator = activator;
        }

        public ContainerRegistration(Func<Resolver, object> activator) : base(Lifetime.ExternallyOwned, typeof(T))
        {
            Activator = activator;
        }

        public Func<Resolver, object> Activator { get; private set; }
    }
}
