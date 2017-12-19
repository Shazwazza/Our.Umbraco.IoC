using System;
using Autofac;

namespace Our.Umbraco.IoC.Autofac
{
    internal class AutofacResolver : Resolver
    {
        private IComponentContext _ctx;
        public AutofacResolver WithContext(IComponentContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            _ctx = ctx;
            return this;
        }
        public override TService Resolve<TService>()
        {
            return _ctx.Resolve<TService>();
        }
    }
}