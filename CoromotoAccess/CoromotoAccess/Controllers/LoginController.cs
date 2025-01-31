using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class LoginController : Controller
 {
        [HttpGet]
        public ActionResult InicioSesion()
        {
            return View();
        }
        // GET: Login/InicioSesion
        [HttpPost]
        public ActionResult InicioSesion(Usuario model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var usuario = context.tUsuario
                    .FirstOrDefault(u => u.Identificacion == model.Identificacion
                                      && u.Contrasenna == model.Contrasenna);

                if (usuario != null)
                {
                    if (usuario.TieneContrasennaTemp && usuario.FechaVencimientoTemp < DateTime.Now)
                    {
                        ViewBag.MensajePantalla = "Credenciales Expiradas";
                        return View();
                    }

                    Session["Consecutivo"] = usuario.ConsecutivoCliente;
                    Session["IdUsuario"] = usuario.Identificacion;
                    Session["NombreUsuario"] = usuario.Nombre;
                    Session["Rol"] = usuario.ConsecutivoRol;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.MensajePantalla = "Credenciales incorrectas";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Registro() {
            return View();
        }
        [HttpPost]
        public ActionResult Registro(Usuario model)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                    // Validar existencia previa
                    bool identificacionExiste = context.tUsuario.Any(u => u.Identificacion == model.Identificacion);
                    bool correoExiste = context.tUsuario.Any(u => u.CorreoElectronico == model.CorreoElectronico);

                    if (identificacionExiste || correoExiste)
                    {
                        if (identificacionExiste && correoExiste)
                        {
                            ViewBag.MensajePantalla = "La identificación y el correo ya existen.";
                        }
                        else if (identificacionExiste)
                        {
                            ViewBag.MensajePantalla = "La identificación ya existe.";
                        }
                        else if (correoExiste)
                        {
                            ViewBag.MensajePantalla = "El correo ya existe.";
                        }
                        return View(model);
                    } 

                    //model.Contrasenna = BCrypt.Net.BCrypt.HashPassword(model.Contrasenna);

                        var nuevoCliente = new tUsuario
                        {
                            ConsecutivoCliente = 0,
                            Identificacion = model.Identificacion,
                            Nombre = model.Nombre,
                            Apellido = model.Apellido,
                            CorreoElectronico = model.CorreoElectronico,
                            Telefono = model.Telefono,
                            FotoPerfil = model.FotoPerfil,
                            Contrasenna = model.Contrasenna,
                            ConsecutivoRol = 3,
                            Activo = true

                        };
                        context.tUsuario.Add(nuevoCliente);
                        context.SaveChanges();
                        return RedirectToAction("InicioSesion", "Login");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error registrando usuario: {ex.Message}";
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




