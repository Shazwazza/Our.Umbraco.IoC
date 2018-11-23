using System;
using System.Collections.Generic;
using System.Linq;
using LightInject;

namespace Our.Umbraco.IoC.LightInject.Cloud
{
    public class LightInjectUmbracoRegister : ICompositionRoot
    {
        private readonly List<IContainerRegistration> _registrations;

        public LightInjectUmbracoRegister()
        {
            _registrations = UmbracoServices.GetAllRegistrations().ToList();
        }

        public LightInjectUmbracoRegister(IEnumerable<IContainerRegistration> registrations)
        {
            _registrations = registrations.ToList();
        }

        private static readonly LightInjectResolver Resolver = new LightInjectResolver();
        
        private static ILifetime GetLifetime(IContainerRegistration reg)
        {
            return reg.Lifetime == Lifetime.Transient
                ? new PerContainerLifetime()
                : reg.Lifetime == Lifetime.ExternallyOwned
                    ? new ExternallyOwnedLifetime()
                    : reg.Lifetime == Lifetime.Request
                        ? new PerRequestLifeTime()
                        : (ILifetime) new PerContainerLifetime();
        }

        public void Compose(IServiceRegistry container)
        {
            //register umbraco types
            foreach (var reg in _registrations)
            {
                var activatorRegistration = reg as IActivatorContainerRegistration;

                if (activatorRegistration != null)
                {
                    object Factory(IServiceFactory x) => activatorRegistration.Activator(Resolver.WithContext(x));

                    container.Register(new ServiceRegistration
                    {                        
                        Lifetime = GetLifetime(reg),
                        ServiceType = reg.Type,
                        FactoryExpression = (Func<IServiceFactory, object>)Factory,
                        ServiceName = string.Empty
                    });
                }
                else
                {
                    container.Register(reg.Type, GetLifetime(reg));
                }
            }
        }
    }
}