using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Security;
using Umbraco.Web;

namespace Our.Umbraco.IoC.WebTest.Controllers
{
    public class TestController : Controller
    {
        private readonly UmbracoContext _umbCtx;
        private readonly BackOfficeUserManager<BackOfficeIdentityUser> _backOfficeUserManager;

        public TestController(UmbracoContext umbCtx, BackOfficeUserManager<BackOfficeIdentityUser> backOfficeUserManager)
        {
            _umbCtx = umbCtx;
            _backOfficeUserManager = backOfficeUserManager;
        }

        // GET: Test
        public ActionResult Index()
        {
            return Content($"Hello world. IsFrontEndUmbracoRequest = {_umbCtx.IsFrontEndUmbracoRequest}. Container Type: {DependencyResolver.Current.GetType()}");
        }
    }
}
