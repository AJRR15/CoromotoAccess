using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace CoromotoAccess.Filters
{
    public class ValidarSesionActivaAttribute : ActionFilterAttribute
    {
        private const int InactividadMinutos = 30;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            var ruta = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" +
                       filterContext.ActionDescriptor.ActionName;

            var rutasExcluidas = new[] {
                "Login/InicioSesion",
                "Login/SesionExpirada",
                "Login/RecuperarContraseña"
            };

            if (rutasExcluidas.Contains(ruta))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (session["UsuarioId"] == null)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            DateTime? ultimaActividad = session["UltimaActividad"] as DateTime?;
            if (ultimaActividad.HasValue &&
                DateTime.Now.Subtract(ultimaActividad.Value).TotalMinutes > InactividadMinutos)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "controller", "Login" },
                        { "action", "SesionExpirada" },
                        { "expired", "1" }
                    }
                );
                return;
            }

            session["UltimaActividad"] = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }
    }
}