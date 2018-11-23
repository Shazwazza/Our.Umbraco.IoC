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

            defs.AddRange(GetUmbracoServiceRegistrations());

            return defs;
        }

        private static IEnumerable<IContainerRegistration> GetUmbracoServiceRegistrations()
        {
            var defs = new List<ContainerRegistration>
            {
                new ContainerRegistration<IMigrationEntryService>(x => ApplicationContext.Current.Services.MigrationEntryService),
                new ContainerRegistration<IPublicAccessService>(x => ApplicationContext.Current.Services.PublicAccessService),
                new ContainerRegistration<ITaskService>(x => ApplicationContext.Current.Services.TaskService),
                new ContainerRegistration<IDomainService>(x => ApplicationContext.Current.Services.DomainService),
                new ContainerRegistration<IAuditService>(x => ApplicationContext.Current.Services.AuditService),
                new ContainerRegistration<ILocalizedTextService>(x => ApplicationContext.Current.Services.TextService),
                new ContainerRegistration<ITagService>(x => ApplicationContext.Current.Services.TagService),
                new ContainerRegistration<IContentService>(x => ApplicationContext.Current.Services.ContentService),
                new ContainerRegistration<IUserService>(x => ApplicationContext.Current.Services.UserService),
                new ContainerRegistration<IMemberService>(x => ApplicationContext.Current.Services.MemberService),
                new ContainerRegistration<IMediaService>(x => ApplicationContext.Current.Services.MediaService),
                new ContainerRegistration<IContentTypeService>(x => ApplicationContext.Current.Services.ContentTypeService),
                new ContainerRegistration<IDataTypeService>(x => ApplicationContext.Current.Services.DataTypeService),
                new ContainerRegistration<IFileService>(x => ApplicationContext.Current.Services.FileService),
                new ContainerRegistration<ILocalizationService>(x => ApplicationContext.Current.Services.LocalizationService),
                new ContainerRegistration<IPackagingService>(x => ApplicationContext.Current.Services.PackagingService),
                new ContainerRegistration<IServerRegistrationService>(x => ApplicationContext.Current.Services.ServerRegistrationService),
                new ContainerRegistration<IEntityService>(x => ApplicationContext.Current.Services.EntityService),
                new ContainerRegistration<IRelationService>(x => ApplicationContext.Current.Services.RelationService),
                new ContainerRegistration<IApplicationTreeService>(x => ApplicationContext.Current.Services.ApplicationTreeService),
                new ContainerRegistration<ISectionService>(x => ApplicationContext.Current.Services.SectionService),
                new ContainerRegistration<IMacroService>(x => ApplicationContext.Current.Services.MacroService),
                new ContainerRegistration<IMemberTypeService>(x => ApplicationContext.Current.Services.MemberTypeService),
                new ContainerRegistration<IMemberGroupService>(x => ApplicationContext.Current.Services.MemberGroupService),
                new ContainerRegistration<INotificationService>(x => ApplicationContext.Current.Services.NotificationService),
                new ContainerRegistration<IExternalLoginService>(x => ApplicationContext.Current.Services.ExternalLoginService),
                new ContainerRegistration<IRedirectUrlService>(x => ApplicationContext.Current.Services.RedirectUrlService),

                // TODO: Uncomment when Umbraco.Core updated
                //new ContainerRegistration<IConsentService>(x => ApplicationContext.Current.Services.ConsentService),
            };

            return defs;
        }
    }
}