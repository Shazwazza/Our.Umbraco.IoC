using System;
using Unity;

namespace Our.Umbraco.IoC.Unity
{
    internal class UnityResolver : Resolver
    {
        private IUnityContainer _container;
        public UnityResolver WithContext(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
            return this;
        }
        public override TService Resolve<TService>()
        {
            return _container.Resolve<TService>();
        }
    }
}