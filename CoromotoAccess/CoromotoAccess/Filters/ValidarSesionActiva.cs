using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using CoromotoAccess.Models; // Ajusta el namespace para tu contexto EF

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
                // 🔹 Cerrar sesión en BD antes de redirigir
                try
                {
                    string identificacion = session["IdUsuario"]?.ToString();
                    string tipoUsuario = session["TipoUsuario"]?.ToString();

                    if (!string.IsNullOrEmpty(identificacion) && !string.IsNullOrEmpty(tipoUsuario))
                    {
                        using (var context = new BDCoromotoEntities())
                        {
                            if (tipoUsuario == "Cliente")
                            {
                                var usuario = context.tUsuario.FirstOrDefault(u => u.Identificacion == identificacion);
                                if (usuario != null)
                                {
                                    usuario.SesionActiva = false;
                                    context.SaveChanges();
                                }
                            }
                            else if (tipoUsuario == "Empleado")
                            {
                                var empleado = context.tEmpleados.FirstOrDefault(e => e.Identificacion == identificacion);
                                if (empleado != null)
                                {
                                    empleado.SesionActiva = false;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Manejo de errores opcional (log, etc.)
                }

                // Limpiar sesión en memoria
                session.Clear();

                // Redirigir a pantalla de sesión expirada
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "controller", "Login" },
                        { "action", "SesionExpirada" },
                        { "expired", "1" }
                    }
                );
                return;
            }

            // Actualizar última actividad
            session["UltimaActividad"] = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }
    }
}
