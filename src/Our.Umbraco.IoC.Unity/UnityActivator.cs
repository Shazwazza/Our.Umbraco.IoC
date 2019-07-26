using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Configuration;
using System.Web;
using Unity.AspNet.Mvc;

[assembly: PreApplicationStartMethod(typeof(Our.Umbraco.IoC.Unity.UnityActivator), "Start")]

namespace Our.Umbraco.IoC.Unity
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    public static class UnityActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            //If this flag exists and it's not 'true' then this container will be disabled.
            if (ConfigurationManager.AppSettings["Our.Umbraco.IoC.Unity.Enabled"] != null && ConfigurationManager.AppSettings["Our.Umbraco.IoC.Unity.Enabled"] != "true")
                return;

            DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }
    }
}
