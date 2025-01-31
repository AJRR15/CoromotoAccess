﻿using CoromotoAccess.Models;
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
        

        [HttpGet]
        public ActionResult Registro() {
            return View();
        }
        [HttpPost]
        public ActionResult Registro(Usuario model)
        {
            // Validar si el modelo es nulo o contiene errores de validación
            if (model == null || !ModelState.IsValid)
            {
                ViewBag.MensajePantalla = "Los datos ingresados no son válidos.";
                return View(model);
            }

            using (var context = new BDCoromotoEntities())
            {
                // Verificar si la identificación o el correo ya existen en la base de datos
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

                
                    // Validar que FotoPerfil no sea nulo (si la base de datos no acepta nulos)
                    if (string.IsNullOrEmpty(model.FotoPerfil))
                    {
                        model.FotoPerfil = "default.jpg"; // O asigna una imagen por defecto
                    }

                    // Llamar al procedimiento almacenado `RegistroUsuario`
                    var respuesta = context.RegistroUsuario(
                        model.Identificacion,
                        model.Nombre,
                        model.Apellido,
                        model.CorreoElectronico,
                        model.Telefono,
                        model.FotoPerfil,
                        model.Contrasenna
                    );

                    // Verificar si el registro fue exitoso
                    if (respuesta > 0)
                    {
                        return RedirectToAction("InicioSesion", "Login");
                    }
                    else
                    {
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




