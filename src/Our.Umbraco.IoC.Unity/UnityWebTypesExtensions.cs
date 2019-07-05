using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

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

            container.RegisterFactory<HttpContextBase>(c => new HttpContextWrapper(HttpContext.Current), new PerRequestLifetimeManager());
            container.RegisterFactory<HttpApplicationStateBase>(c => new HttpApplicationStateWrapper(HttpContext.Current.Application), new PerRequestLifetimeManager());
            container.RegisterFactory<HttpRequestBase>(c => new HttpRequestWrapper(HttpContext.Current.Request), new PerRequestLifetimeManager());
            container.RegisterFactory<HttpBrowserCapabilitiesBase>(c => new HttpBrowserCapabilitiesWrapper(HttpContext.Current.Request.Browser), new PerRequestLifetimeManager());
            container.RegisterFactory<HttpFileCollectionBase>(c => new HttpFileCollectionWrapper(HttpContext.Current.Request.Files), new PerRequestLifetimeManager());
            container.RegisterFactory<RequestContext>(c => HttpContext.Current.Request.RequestContext, new PerRequestLifetimeManager());
            container.RegisterFactory<HttpResponseBase>(c => new HttpResponseWrapper(HttpContext.Current.Response), new PerRequestLifetimeManager());
            container.RegisterFactory<HttpCachePolicyBase>(c => new HttpCachePolicyWrapper(HttpContext.Current.Response.Cache), new PerRequestLifetimeManager());
            container.RegisterFactory<HttpServerUtilityBase>(c => new HttpServerUtilityWrapper(HttpContext.Current.Server), new PerRequestLifetimeManager());
            container.RegisterFactory<HttpSessionStateBase>(c => new HttpSessionStateWrapper(HttpContext.Current.Session), new PerRequestLifetimeManager());
            container.RegisterFactory<VirtualPathProvider>(c => HostingEnvironment.VirtualPathProvider, new PerRequestLifetimeManager());
        }
    }
}
