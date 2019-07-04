using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace Our.Umbraco.IoC.Unity
{
    /// <summary>
    /// Used to register the common web types for LightInject
    /// </summary>
    static class UnityWebTypesExtensions
    {
        public static void RegisterWebTypes(this IUnityContainer container)
        {
            //these are the same items that are registered with Autofac's AutofacWebTypesModule: https://autofac.org/apidoc/html/DA6737B.htm

            container.RegisterType<HttpContextBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpContextWrapper(HttpContext.Current)));
            container.RegisterType<HttpApplicationStateBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpApplicationStateWrapper(HttpContext.Current.Application)));
            container.RegisterType<HttpRequestBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpRequestWrapper(HttpContext.Current.Request)));
            container.RegisterType<HttpBrowserCapabilitiesBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpBrowserCapabilitiesWrapper(HttpContext.Current.Request.Browser)));
            container.RegisterType<HttpFileCollectionBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpFileCollectionWrapper(HttpContext.Current.Request.Files)));
            container.RegisterType<RequestContext>(new PerRequestLifetimeManager(), new InjectionFactory(c => HttpContext.Current.Request.RequestContext));
            container.RegisterType<HttpResponseBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpResponseWrapper(HttpContext.Current.Response)));
            container.RegisterType<HttpCachePolicyBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpCachePolicyWrapper(HttpContext.Current.Response.Cache)));
            container.RegisterType<HttpServerUtilityBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpServerUtilityWrapper(HttpContext.Current.Server)));
            container.RegisterType<HttpSessionStateBase>(new PerRequestLifetimeManager(), new InjectionFactory(c => new HttpSessionStateWrapper(HttpContext.Current.Session)));
            container.RegisterType<VirtualPathProvider>(new PerRequestLifetimeManager(), new InjectionFactory(c => HostingEnvironment.VirtualPathProvider));
        }
    }
}
