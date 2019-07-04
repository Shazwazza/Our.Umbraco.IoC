using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;

namespace Our.Umbraco.IoC.WebTest.App_Start
{
    public class Startup : ApplicationEventHandler
    {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            RouteTable.Routes.MapRoute("test", "test", new { controller = "Test", action = "Index" });
        }

    }
}
