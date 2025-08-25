using CoromotoAccess.Filters;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ValidarSesionActivaAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
