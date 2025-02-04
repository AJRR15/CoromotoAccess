using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ReservaController : Controller
    {
        [HttpGet]
        public ActionResult AdministrarReservas()
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tReservas.ToList();
                var usuarios = context.tUsuario.Select(u => new Usuario
                {
                    ConsecutivoCliente = u.ConsecutivoCliente,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Telefono = u.Telefono
                }).ToList();
                var habitaciones = context.tHabitaciones.Select(h => new Habitacion
                {
                    IdHabitacion = h.IdHabitacion,
                    NombreHabitacion = h.NombreHabitacion
                }).ToList();

                ViewBag.Usuarios = usuarios;
                ViewBag.Habitaciones = habitaciones;

                var listaReservas = datos.Select(r => new Reserva
                {
                    IdReserva = r.IdReserva,
                    IdUsuario = r.IdUsuario,         // Asegúrate que coincida con tUsuario.ConsecutivoCliente
                    IdHabitacion = r.IdHabitacion,   // Asegúrate que coincida con tHabitaciones.IdHabitacion
                    CheckIn = r.CheckIn,
                    CheckOut = r.CheckOut,
                    Estado = r.Estado,
                    IdMoneda = r.IdMoneda,
                    IdMetodoP = r.IdMetodoP
                }).ToList();

                

                return View(listaReservas);
            }
        }
        [HttpPost]
        public ActionResult AgregarReserva(Reserva model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var idUsuario = int.Parse(Session["Consecutivo"].ToString());  // Asegúrate de que este valor sea correcto

                // Verificar si la habitación existe en la base de datos
                var habitacion = context.tHabitaciones.Find(model.IdHabitacion);
                if (habitacion == null)
                {
                    ModelState.AddModelError("", "La habitación seleccionada no existe.");
                    return RedirectToAction("DatosHabitacion", new { id = model.IdHabitacion });
                }

                // Verificar si el usuario existe en la base de datos
                var usuario = context.tUsuario.Find(idUsuario);
                if (usuario == null)
                {
                    ModelState.AddModelError("", "El usuario seleccionado no existe.");
                    return RedirectToAction("DatosHabitacion", new { id = model.IdHabitacion });
                }

                // Verificar disponibilidad de la habitación en las fechas seleccionadas
                var reservasExistentes = context.tReservas.Where(r => r.IdHabitacion == model.IdHabitacion && ((model.CheckIn >= r.CheckIn && model.CheckIn <= r.CheckOut) || (model.CheckOut >= r.CheckIn && model.CheckOut <= r.CheckOut))).ToList();

                if (reservasExistentes.Any())
                {
                    ModelState.AddModelError("", "La habitación ya está reservada en las fechas seleccionadas.");
                    return RedirectToAction("DatosHabitacion", new { id = model.IdHabitacion });
                }

                var Estado = true;
                var reserva = new tReservas
                {
                    IdUsuario = idUsuario,  // Asegúrate de asignar el IdUsuario correcto
                    IdHabitacion = model.IdHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Comentario = model.Comentario,
                    PersonasHospedados = model.PersonasHospedados,
                    Estado = Estado,
                    IdMoneda = model.IdMoneda,
                    IdMetodoP = model.IdMetodoP,
                };

                context.tReservas.Add(reserva);
                context.SaveChanges();

                return RedirectToAction("CatalogoHabitaciones", "Habitacion");
            }
        }

    }
}
