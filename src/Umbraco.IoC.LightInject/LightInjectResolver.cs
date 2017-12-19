using System;
using LightInject;

namespace Umbraco.IoC.LightInject
{
    internal class LightInjectResolver : Resolver
    {
        private IServiceFactory _ctx;
        public LightInjectResolver WithContext(IServiceFactory ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            _ctx = ctx;
            return this;
        }
        public override TService Resolve<TService>()
        {
            return _ctx.GetInstance<TService>();
        }
    }
}