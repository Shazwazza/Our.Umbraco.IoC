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
        readonly IUnityContainer _container;
        readonly IEnumerable<IContainerRegistration> _registrations;

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

        private static IFactoryLifetimeManager GetFactoryLifetime(IContainerRegistration reg)
        {
            return reg.Lifetime == Lifetime.Transient
                ? new TransientLifetimeManager()
                : reg.Lifetime == Lifetime.ExternallyOwned
                    ? new ExternallyControlledLifetimeManager()
                    : reg.Lifetime == Lifetime.Request
                        ? new PerRequestLifetimeManager()
                        : (IFactoryLifetimeManager)new TransientLifetimeManager();
        }

        private static ITypeLifetimeManager GetLifetime(IContainerRegistration reg)
        {
            return reg.Lifetime == Lifetime.Transient
                ? new TransientLifetimeManager()
                : reg.Lifetime == Lifetime.ExternallyOwned
                    ? new ExternallyControlledLifetimeManager()
                    : reg.Lifetime == Lifetime.Request
                        ? new PerRequestLifetimeManager()
                        : (ITypeLifetimeManager)new TransientLifetimeManager();
        }

        public void RegisterTypes()
        {
            foreach (var registration in _registrations)
            {
                if (registration is IActivatorContainerRegistration activatorRegistration)
                {
                    _container.RegisterFactory(registration.Type, c => activatorRegistration.Activator(_resolver.WithContext(c)), GetFactoryLifetime(registration));
                }
                else
                {
                    _container.RegisterType(registration.Type, GetLifetime(registration));
                }
            }
        }
    }
}
