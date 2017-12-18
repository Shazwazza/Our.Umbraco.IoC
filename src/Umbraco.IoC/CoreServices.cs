using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.HealthChecks;
using Umbraco.Core.Services;

namespace Umbraco.IoC
{    
    public class CoreServices
    {

        /// <summary>
        /// Returns all required container registrations for Umbraco.Core
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ContainerRegistration> GetContainerRegistrations()
        {
            var defs = new List<ContainerRegistration>
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