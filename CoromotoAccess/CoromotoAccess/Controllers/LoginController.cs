using CoromotoAccess.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult InicioSesion(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InicioSesion(Usuario model, string returnUrl)
        {
            using (var context = new BDCoromotoEntities())
            {
                var resultadoUsuario = context.InicioSesionUsuario(model.Identificacion, model.Contrasenna).FirstOrDefault();
                var resultadoEmpleado = context.InicioSesionEmpleado(model.Identificacion, model.Contrasenna).FirstOrDefault();

                if (resultadoUsuario != null)
                {
                    var rol = context.tRoles.FirstOrDefault(r => r.IdRol == resultadoUsuario.ConsecutivoRol);

                    if (resultadoUsuario.TieneContrasennaTemp && resultadoUsuario.FechaVencimientoTemp < DateTime.Now)
                    {
                        ViewBag.MensajePantalla = "Credenciales Expiradas";
                        return View();
                    }

                    // Opcional: regenerar SessionID para mitigar fijación de sesión
                    Session.RemoveAll();

                    Session["UsuarioId"] = resultadoUsuario.ConsecutivoCliente;
                    Session["Consecutivo"] = resultadoUsuario.ConsecutivoCliente;
                    Session["IdUsuario"] = resultadoUsuario.Identificacion;
                    Session["NombreUsuario"] = resultadoUsuario.Nombre;
                    Session["Rol"] = resultadoUsuario.ConsecutivoRol;
                    Session["NombreRol"] = rol != null ? rol.NombreRol : "Rol desconocido";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
                else if (resultadoEmpleado != null)
                {
                    var rol = context.tRoles.FirstOrDefault(r => r.IdRol == resultadoEmpleado.ConsecutivoRol);

                    if (resultadoEmpleado.TieneContrasennaTemp && resultadoEmpleado.FechaVencimientoTemp < DateTime.Now)
                    {
                        ViewBag.MensajePantalla = "Credenciales Expiradas";
                        return View();
                    }

                    Session.RemoveAll();

                    Session["UsuarioId"] = resultadoEmpleado.ConsecutivoEmp;
                    Session["Consecutivo"] = resultadoEmpleado.ConsecutivoEmp;
                    Session["IdUsuario"] = resultadoEmpleado.Identificacion;
                    Session["NombreUsuario"] = resultadoEmpleado.Nombre;
                    Session["Rol"] = resultadoEmpleado.ConsecutivoRol;
                    Session["NombreRol"] = rol != null ? rol.NombreRol : "Rol desconocido";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.MensajePantalla = "Correo o Contraseña incorrecta";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuario model)
        {
            if (model == null || !ModelState.IsValid)
            {
                ViewBag.MensajePantalla = "Los datos ingresados no son válidos.";
                return View(model);
            }

            using (var context = new BDCoromotoEntities())
            {
                bool identificacionExiste = context.tUsuario.Any(u => u.Identificacion == model.Identificacion);
                bool correoExiste = context.tUsuario.Any(u => u.CorreoElectronico == model.CorreoElectronico);
                bool telefonoExiste = context.tUsuario.Any(u => u.Telefono == model.Telefono);

                if (identificacionExiste || correoExiste || telefonoExiste)
                {
                    if (identificacionExiste && correoExiste)
                        ViewBag.MensajePantalla = "La identificación y el correo ya existen.";
                    else if (identificacionExiste)
                        ViewBag.MensajePantalla = "La identificación ya existe.";
                    else if (correoExiste)
                        ViewBag.MensajePantalla = "El correo ya existe.";
                    else if (telefonoExiste)
                        ViewBag.MensajePantalla = "El número de teléfono ya existe.";

                    return View(model);
                }

                if (string.IsNullOrEmpty(model.FotoPerfil))
                {
                    model.FotoPerfil = "default.jpg";
                }

                var respuesta = context.RegistroUsuario(
                    model.Identificacion,
                    model.Nombre,
                    model.Apellido,
                    model.CorreoElectronico,
                    model.Telefono,
                    model.FotoPerfil,
                    model.Contrasenna
                );

                if (respuesta > 0)
                {
                    TempData["Mensaje"] = "Registro exitoso. Ahora puede iniciar sesión.";
                    return RedirectToAction("InicioSesion", "Login");
                }
                else
                {
                    TempData["Error"] = "No se ha podido registrar el usuario.";
                    ViewBag.MensajePantalla = "No se ha podido registrar el usuario.";
                    return View(model);
                }
            }
        }

        [HttpGet]
        public ActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ActualizarPerfil()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MiPerfil()
        {
            return View();
        }

        public ActionResult MiEvaluacion()
        {
            return View();
        }
    }
}





