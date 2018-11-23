using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using LightInject;

namespace Our.Umbraco.IoC.LightInject.Cloud
{
    /// <summary>
    /// Used to register the common web types for LightInject
    /// </summary>
    public class LightInjectWebTypesCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            //these are the same items that are registered with Autofac's AutofacWebTypesModule: https://autofac.org/apidoc/html/DA6737B.htm

            serviceRegistry.Register<HttpContextBase>(factory => new HttpContextWrapper(HttpContext.Current), new PerRequestLifeTime());
            serviceRegistry.Register<HttpApplicationStateBase>(factory => new HttpApplicationStateWrapper(HttpContext.Current.Application), new PerRequestLifeTime());
            serviceRegistry.Register<HttpRequestBase>(factory => new HttpRequestWrapper(HttpContext.Current.Request), new PerRequestLifeTime());
            serviceRegistry.Register<HttpBrowserCapabilitiesBase>(factory => new HttpBrowserCapabilitiesWrapper(HttpContext.Current.Request.Browser), new PerRequestLifeTime());
            serviceRegistry.Register<HttpFileCollectionBase>(factory => new HttpFileCollectionWrapper(HttpContext.Current.Request.Files), new PerRequestLifeTime());
            serviceRegistry.Register<RequestContext>(factory => HttpContext.Current.Request.RequestContext, new PerRequestLifeTime());
            serviceRegistry.Register<HttpResponseBase>(factory => new HttpResponseWrapper(HttpContext.Current.Response), new PerRequestLifeTime());
            serviceRegistry.Register<HttpCachePolicyBase>(factory => new HttpCachePolicyWrapper(HttpContext.Current.Response.Cache), new PerRequestLifeTime());
            serviceRegistry.Register<HttpServerUtilityBase>(factory => new HttpServerUtilityWrapper(HttpContext.Current.Server), new PerRequestLifeTime());
            serviceRegistry.Register<HttpSessionStateBase>(factory => new HttpSessionStateWrapper(HttpContext.Current.Session), new PerRequestLifeTime());
            serviceRegistry.Register<VirtualPathProvider>(factory => HostingEnvironment.VirtualPathProvider, new PerRequestLifeTime());
        }
    }
}