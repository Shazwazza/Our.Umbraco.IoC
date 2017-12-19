using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.HealthChecks;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.HealthCheck;

namespace Our.Umbraco.IoC
{    
    public class UmbracoServices
    {
        public static IEnumerable<IContainerRegistration> GetAllRegistrations()
        {
            return GetCoreRegistrations().Concat(GetWebRegistrations());
        }

        /// <summary>
        /// Returns all required container registrations for Umbraco.Web
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IContainerRegistration> GetWebRegistrations()
        {
            var defs = new List<IContainerRegistration>
            {
                new ContainerRegistration<IOwinContext>(x => x.Resolve<HttpContextBase>().GetOwinContext()),
                new ContainerRegistration<IHealthCheckResolver>(x => HealthCheckResolver.Current),
                new ContainerRegistration<UmbracoContext>(x => UmbracoContext.Current),
                new ContainerRegistration<UmbracoHelper>(Lifetime.Request, x => new UmbracoHelper(x.Resolve<UmbracoContext>())),
                new ContainerRegistration<BackOfficeUserManager<BackOfficeIdentityUser>>(x => x.Resolve<IOwinContext>().GetBackOfficeUserManager()) //lifetime managed by OWIN
            };
            return defs;
        }

        /// <summary>
        /// Returns all required container registrations for Umbraco.Core
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IContainerRegistration> GetCoreRegistrations()
        {
            var defs = new List<IContainerRegistration>
            {
                new ContainerRegistration<ApplicationContext>(x => ApplicationContext.Current),
                new ContainerRegistration<ServiceContext>(x => ApplicationContext.Current.Services),
                new ContainerRegistration<DatabaseContext>(x => ApplicationContext.Current.DatabaseContext),
                new ContainerRegistration<IHealthChecks>(x => UmbracoConfig.For.HealthCheck()),
            };
            return defs;
        }
    }
}