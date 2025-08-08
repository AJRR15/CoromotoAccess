using System.Web.Mvc;
using CoromotoAccess.Filters;

public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
        filters.Add(new AuthRequiredAttribute());      // Bloquea todo por defecto
        filters.Add(new RequireHttpsAttribute());      // Opcional pero recomendado (HTTPS)
    }
}
