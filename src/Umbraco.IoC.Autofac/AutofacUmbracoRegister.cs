using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;

namespace Umbraco.IoC.Autofac
{
    /// <summary>
    /// Used to dynamically register services with Autofac
    /// </summary>
    public class AutofacUmbracoRegister : IRegistrationSource
    {
        private readonly List<ContainerRegistration> _registrations;
        private readonly AutofacResolver _resolver = new AutofacResolver();

        public AutofacUmbracoRegister(IEnumerable<ContainerRegistration> registrations)
        {
            if (registrations == null) throw new ArgumentNullException(nameof(registrations));
            _registrations = registrations.ToList();
        }
        
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            if (!(service is IServiceWithType swt))
                return Enumerable.Empty<IComponentRegistration>();

            var reg = _registrations.FirstOrDefault(x => x.Type == swt.ServiceType);
            if (reg == null)
                return Enumerable.Empty<IComponentRegistration>();

            var activatorRegistration = reg as IActivatorContainerRegistration;

            if (activatorRegistration != null)
            {
                var registration = new ComponentRegistration(
                    Guid.NewGuid(),
                    new DelegateActivator(swt.ServiceType, (c, p) =>
                    {
                        var result = activatorRegistration.Activator(_resolver.WithContext(c));
                        return result;
                    }),
                    reg.Lifetime == Lifetime.Request ? PerRequestLifetime() : reg.Lifetime == Lifetime.Singleton ? (IComponentLifetime) new RootScopeLifetime() : new CurrentScopeLifetime(),
                    reg.Lifetime == Lifetime.Request || reg.Lifetime == Lifetime.Singleton ? InstanceSharing.Shared : InstanceSharing.None,
                    reg.Lifetime == Lifetime.ExternallyOwned ? InstanceOwnership.ExternallyOwned : InstanceOwnership.OwnedByLifetimeScope,
                    new[] {service},
                    new Dictionary<string, object>());
                return new IComponentRegistration[] {registration};
            }
            else
            {
                var activatorData = new ConcreteReflectionActivatorData(reg.Type);
                var registration = new ComponentRegistration(
                    Guid.NewGuid(),
                    activatorData.Activator,
                    reg.Lifetime == Lifetime.Request ? PerRequestLifetime() : reg.Lifetime == Lifetime.Singleton ? (IComponentLifetime)new RootScopeLifetime() : new CurrentScopeLifetime(),
                    reg.Lifetime == Lifetime.Request || reg.Lifetime == Lifetime.Singleton ? InstanceSharing.Shared : InstanceSharing.None,
                    reg.Lifetime == Lifetime.ExternallyOwned ? InstanceOwnership.ExternallyOwned : InstanceOwnership.OwnedByLifetimeScope,
                    new[] { service },
                    new Dictionary<string, object>());

                return new IComponentRegistration[] { registration };
            }

        }

        public bool IsAdapterForIndividualComponents => false;

        private IComponentLifetime PerRequestLifetime()
        {
            var lifetimeScopeTag = new[] {MatchingScopeLifetimeTags.RequestLifetimeScopeTag};
            var life = new MatchingScopeLifetime(lifetimeScopeTag);
            return life;
        }
    }
}