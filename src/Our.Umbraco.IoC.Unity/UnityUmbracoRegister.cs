using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using Unity.Lifetime;

namespace Our.Umbraco.IoC.Unity
{
    /// <summary>
    /// Used to register Umbraco services with Unity
    /// </summary>
    class UnityUmbracoRegister
    {
        IUnityContainer _container;
        IEnumerable<IContainerRegistration> _registrations;

        private readonly UnityResolver _resolver = new UnityResolver();

        public UnityUmbracoRegister(IUnityContainer container)
        {
            _container = container;
            _registrations = UmbracoServices.GetAllRegistrations().ToList();
        }

        public UnityUmbracoRegister(IUnityContainer container, IEnumerable<IContainerRegistration> registrations)
        {
            _container = container;
            _registrations = registrations.ToList();
        }

        private static LifetimeManager GetLifetime(IContainerRegistration reg)
        {
            return reg.Lifetime == Lifetime.Transient
                ? new TransientLifetimeManager()
                : reg.Lifetime == Lifetime.ExternallyOwned
                    ? new ExternallyControlledLifetimeManager()
                    : reg.Lifetime == Lifetime.Request
                        ? new PerRequestLifetimeManager()
                        : (LifetimeManager)new TransientLifetimeManager();
        }

        public void RegisterTypes()
        {
            foreach (var registration in _registrations)
            {
                if (registration is IActivatorContainerRegistration activatorRegistration)
                {
                    _container.RegisterType(registration.Type, GetLifetime(registration), new InjectionFactory(c => activatorRegistration.Activator(_resolver.WithContext(c))));
                }
                else
                {
                    _container.RegisterType(registration.Type, GetLifetime(registration));
                }
            }
        }
    }
}
