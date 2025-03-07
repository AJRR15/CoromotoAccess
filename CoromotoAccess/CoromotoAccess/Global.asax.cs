using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CoromotoAccess
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
       
        protected void Application_BeginRequest()
        {
            if (Request.Cookies["Language"] != null)
            {
                string lang = Request.Cookies["Language"].Value;
                SetCulture(lang);
            }
        }

        private void SetCulture(string lang)
        {
            CultureInfo culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

    }

}
