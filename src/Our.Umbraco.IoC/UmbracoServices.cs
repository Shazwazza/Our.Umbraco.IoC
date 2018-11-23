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

            defs.AddRange(GetGranularUmbracoServiceRegistrations());

            return defs;
        }

        private static IEnumerable<IContainerRegistration> GetGranularUmbracoServiceRegistrations()
        {
            var defs = new List<ContainerRegistration>
            {
                new ContainerRegistration<IMigrationEntryService>(x => x.Resolve<ServiceContext>().MigrationEntryService),
                new ContainerRegistration<IPublicAccessService>(x => x.Resolve<ServiceContext>().PublicAccessService),
                new ContainerRegistration<ITaskService>(x => x.Resolve<ServiceContext>().TaskService),
                new ContainerRegistration<IDomainService>(x => x.Resolve<ServiceContext>().DomainService),
                new ContainerRegistration<IAuditService>(x => x.Resolve<ServiceContext>().AuditService),
                new ContainerRegistration<ILocalizedTextService>(x => x.Resolve<ServiceContext>().TextService),
                new ContainerRegistration<ITagService>(x => x.Resolve<ServiceContext>().TagService),
                new ContainerRegistration<IContentService>(x => x.Resolve<ServiceContext>().ContentService),
                new ContainerRegistration<IUserService>(x => x.Resolve<ServiceContext>().UserService),
                new ContainerRegistration<IMemberService>(x => x.Resolve<ServiceContext>().MemberService),
                new ContainerRegistration<IMediaService>(x => x.Resolve<ServiceContext>().MediaService),
                new ContainerRegistration<IContentTypeService>(x => x.Resolve<ServiceContext>().ContentTypeService),
                new ContainerRegistration<IDataTypeService>(x => x.Resolve<ServiceContext>().DataTypeService),
                new ContainerRegistration<IFileService>(x => x.Resolve<ServiceContext>().FileService),
                new ContainerRegistration<ILocalizationService>(x => x.Resolve<ServiceContext>().LocalizationService),
                new ContainerRegistration<IPackagingService>(x => x.Resolve<ServiceContext>().PackagingService),
                new ContainerRegistration<IServerRegistrationService>(x => x.Resolve<ServiceContext>().ServerRegistrationService),
                new ContainerRegistration<IEntityService>(x => x.Resolve<ServiceContext>().EntityService),
                new ContainerRegistration<IRelationService>(x => x.Resolve<ServiceContext>().RelationService),
                new ContainerRegistration<IApplicationTreeService>(x => x.Resolve<ServiceContext>().ApplicationTreeService),
                new ContainerRegistration<ISectionService>(x => x.Resolve<ServiceContext>().SectionService),
                new ContainerRegistration<IMacroService>(x => x.Resolve<ServiceContext>().MacroService),
                new ContainerRegistration<IMemberTypeService>(x => x.Resolve<ServiceContext>().MemberTypeService),
                new ContainerRegistration<IMemberGroupService>(x => x.Resolve<ServiceContext>().MemberGroupService),
                new ContainerRegistration<INotificationService>(x => x.Resolve<ServiceContext>().NotificationService),
                new ContainerRegistration<IExternalLoginService>(x => x.Resolve<ServiceContext>().ExternalLoginService),
                new ContainerRegistration<IRedirectUrlService>(x => x.Resolve<ServiceContext>().RedirectUrlService),

                // TODO: Uncomment when Umbraco.Core updated
                //new ContainerRegistration<IConsentService>(x => x.Resolve<ServiceContext>().ConsentService),
            };

            return defs;
        }
    }
}