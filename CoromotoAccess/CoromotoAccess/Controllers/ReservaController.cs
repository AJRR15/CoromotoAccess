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
                var usuarios = context.tUsuario.ToList();
                var tiposHabitacion = context.tTiposHabitaciones.ToList();

                ViewBag.Usuarios = usuarios;
                ViewBag.TiposHabitacion = tiposHabitacion;

                var listaReservas = new List<Reserva>();

                foreach (var item in datos)
                {
                    listaReservas.Add(new Reserva
                    {
                        IdReserva = item.IdReserva,
                        IdUsuario = item.IdUsuario,
                        IdHabitacion = item.IdHabitacion,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut,
                        Estado = item.Estado,
                        IdMoneda = item.IdMoneda,
                        IdMetodoP = item.IdMetodoP
                    });
                }

                return View(listaReservas);
            }
        }

        [HttpPost]
        public ActionResult AgregarReserva(Reserva model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var id = int.Parse(Session["Consecutivo"].ToString());

                // Verificar si la habitación existe en la base de datos
                var habitacion = context.tHabitaciones.Find(model.IdHabitacion);
                if (habitacion == null)
                {
                    ModelState.AddModelError("", "La habitación seleccionada no existe.");
                    return RedirectToAction("DatosHabitacion", new { id = model.IdHabitacion });
                }

                var reservasExistentes = context.tReservas.Where(r => r.IdHabitacion == model.IdHabitacion && ((model.CheckIn >= r.CheckIn && model.CheckIn <= r.CheckOut) || (model.CheckOut >= r.CheckIn && model.CheckOut <= r.CheckOut))).ToList();

                if (reservasExistentes.Any())
                {
                    ModelState.AddModelError("", "La habitación ya está reservada en las fechas seleccionadas.");
                    return RedirectToAction("DatosHabitacion", new { id = model.IdHabitacion });
                }

                var reserva = new tReservas
                {
                    IdUsuario = id,
                    IdHabitacion = model.IdHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Estado = model.Estado,
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
