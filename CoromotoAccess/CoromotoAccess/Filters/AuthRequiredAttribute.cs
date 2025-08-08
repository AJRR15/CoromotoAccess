using System;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Filters
{
    public class AuthRequiredAttribute : AuthorizeAttribute
    {
        public string Roles { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var isLogged = session != null && session["UsuarioId"] != null;

            if (!isLogged)
            {
                var url = new UrlHelper(filterContext.RequestContext);
                var returnUrl = filterContext.HttpContext.Request?.Url?.PathAndQuery;
                var redirect = url.Action("InicioSesion", "Login", new { returnUrl });

                filterContext.Result = new RedirectResult(redirect);
                return;
            }

            var userRol = session["NombreRol"]?.ToString();

            if (!string.IsNullOrEmpty(Roles))
            {
                var rolesPermitidos = Roles.Split(',');
                bool tienePermiso = Array.Exists(rolesPermitidos, r => r.Trim().Equals(userRol, StringComparison.OrdinalIgnoreCase));

                if (!tienePermiso)
                {
                    filterContext.Result = new RedirectResult("~/Error/AccesoDenegado"); // O muestra ViewBag con mensaje
                    return;
                }
            }
        }
    }
}
