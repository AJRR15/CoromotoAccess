using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        [HttpGet]
        public ActionResult GestionClientes()
        {
            using (var context = new BDCoromotoEntities())
            {
                var usuarios = context.tUsuario.Select(u => new Usuario
                {
                    ConsecutivoCliente = u.ConsecutivoCliente,
                    Identificacion = u.Identificacion,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    CorreoElectronico = u.CorreoElectronico,
                    Telefono = u.Telefono
                }).ToList();

                ViewBag.MensajePantalla = TempData["MensajePantalla"];
                return View(usuarios);
            }
        }

        [HttpGet]
        public ActionResult ModificarCliente(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tUsuario.Where(x => x.ConsecutivoCliente == id).FirstOrDefault();

                if (model == null)
                {
                    TempData["MensajePantalla"] = "No se encontró al cliente.";
                    return RedirectToAction("GestionClientes", "Clientes");
                }
                var usuario = new Usuario()
                {
                    ConsecutivoCliente = model.ConsecutivoCliente,
                    Identificacion = model.Identificacion,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    CorreoElectronico = model.CorreoElectronico,
                    Telefono = model.Telefono
                };
                return View(usuario);
            }
        }

        [HttpPost]
        public ActionResult ModificarCliente(Usuario model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tUsuario.Where(x => x.ConsecutivoCliente == model.ConsecutivoCliente).FirstOrDefault();
                if (datos != null)
                {
                    datos.CorreoElectronico = model.CorreoElectronico;
                    datos.Telefono = model.Telefono;

                    context.SaveChanges();
                    TempData["MensajePantalla"] = "La información se ha actualizado correctamente.";
                    return RedirectToAction("GestionClientes", "Clientes");
                }

                TempData["MensajePantalla"] = "La información no se ha podido actualizar correctamente.";
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult AgregarCliente()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AgregarCliente(Usuario model)
        {
            if (model == null || !ModelState.IsValid)
            {
                TempData["MensajePantalla"] = "Los datos ingresados no son válidos.";
                return RedirectToAction("GestionClientes");
            }

            using (var context = new BDCoromotoEntities())
            {
                bool identificacionExiste = context.tUsuario.Any(u => u.Identificacion == model.Identificacion);
                bool correoExiste = context.tUsuario.Any(u => u.CorreoElectronico == model.CorreoElectronico);
                bool telefonoExiste = context.tUsuario.Any(u => u.Telefono == model.Telefono);

                if (identificacionExiste || correoExiste)
                {
                    if (identificacionExiste && correoExiste)
                    {
                        TempData["MensajePantalla"] = "La identificación y el correo ya existen.";
                    }
                    else if (identificacionExiste)
                    {
                        TempData["MensajePantalla"] = "La identificación ya existe.";
                    }
                    else if (correoExiste)
                    {
                        TempData["MensajePantalla"] = "El correo ya existe.";
                    }
                    else if (telefonoExiste)
                    {
                        TempData["MensajePantalla"] = "El número de teléfono ya existe.";
                    }
                    return RedirectToAction("GestionClientes");
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
                    return RedirectToAction("InicioSesion", "Login");
                }
                else
                {
                    TempData["MensajePantalla"] = "No se ha podido registrar el usuario.";
                    return RedirectToAction("GestionClientes");
                }
            }
        }

        [HttpGet]
        public ActionResult HistorialCliente(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var usuario = context.tUsuario.Find(id);

                if (usuario == null)
                {
                    ViewBag.MensajePantalla = "No se encontró el cliente.";
                    return RedirectToAction("GestionClientes");
                }

                var reservas = context.tReservas
                    .Where(r => r.IdUsuario == id)
                    .Select(r => new Reserva
                    {
                        IdReserva = r.IdReserva,
                        IdHabitacion = r.IdHabitacion,
                        CheckIn = r.CheckIn,
                        CheckOut = r.CheckOut,
                        Estado = r.Estado
                    }).ToList();

                ViewBag.Usuario = usuario.Nombre + " " + usuario.Apellido;
                return View(reservas);
            }
        }

        [HttpGet]
        public ActionResult DetallesReservaCliente(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tReservas.Where(x => x.IdReserva == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró la reserva.";
                    return RedirectToAction("HistorialCliente");
                }

                var moneda = context.tMonedas.FirstOrDefault(m => m.IdMoneda == model.IdMoneda);
                var metodoPago = context.tMetodoPago.FirstOrDefault(mp => mp.IdMetodoP == model.IdMetodoP);
                var usuario = context.tUsuario.FirstOrDefault(u => u.ConsecutivoCliente == model.IdUsuario);
                var habitacion = context.tHabitaciones.FirstOrDefault(h => h.IdHabitacion == model.IdHabitacion);

                var reserva = new Reserva()
                {
                    IdReserva = model.IdReserva,
                    IdUsuario = model.IdUsuario,
                    NombreUsuario = usuario?.Nombre + " " + usuario?.Apellido,
                    IdHabitacion = model.IdHabitacion,
                    NombreHabitacion = habitacion?.NombreHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Estado = model.Estado,
                    IdMoneda = model.IdMoneda,
                    NombreMoneda = moneda?.NombreMoneda,
                    IdMetodoP = model.IdMetodoP,
                    NombreMetodoPago = metodoPago?.NombreMetodoP,
                };

                return View(reserva);
            }
        }
    }
}