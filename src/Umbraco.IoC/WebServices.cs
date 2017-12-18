using System.Collections.Generic;
using System.Web;
using Microsoft.Owin;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Security;
using Umbraco.Web;
using Umbraco.Web.HealthCheck;

namespace Umbraco.IoC
{
    public class WebServices
    {
        /// <summary>
        /// Returns all required container registrations for Umbraco.Web
        /// </summary>
        /// <returns></returns>
        public static List<ContainerRegistration> GetContainerRegistrations()
        {
            var defs = new List<ContainerRegistration>
            {
                new ContainerRegistration<IOwinContext>(x => x.Resolve<HttpContextBase>().GetOwinContext()),
                new ContainerRegistration<IHealthCheckResolver>(x => HealthCheckResolver.Current),
                new ContainerRegistration<UmbracoContext>(x => UmbracoContext.Current),
                new ContainerRegistration<UmbracoHelper>(Lifetime.Request, x => new UmbracoHelper(x.Resolve<UmbracoContext>())),
                new ContainerRegistration<BackOfficeUserManager<BackOfficeIdentityUser>>(x => x.Resolve<IOwinContext>().GetBackOfficeUserManager()) //lifetime managed by OWIN
            };
            return defs;
        }
    }
}