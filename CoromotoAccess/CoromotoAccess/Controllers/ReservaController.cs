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
                    IdUsuario = r.IdUsuario,
                    IdHabitacion = r.IdHabitacion,
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
                var idUsuario = int.Parse(Session["Consecutivo"].ToString());

                var habitacion = context.tHabitaciones.Find(model.IdHabitacion);
                if (habitacion == null)
                {
                    return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "La habitación seleccionada no existe." });
                }

                var usuario = context.tUsuario.Find(idUsuario);
                if (usuario == null)
                {
                    return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "El usuario seleccionado no existe." });
                }

                var reservasExistentes = context.tReservas.Where(r => r.IdHabitacion == model.IdHabitacion && ((model.CheckIn >= r.CheckIn && model.CheckIn <= r.CheckOut) || (model.CheckOut >= r.CheckIn && model.CheckOut <= r.CheckOut))).ToList();

                if (reservasExistentes.Any())
                {
                    return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "La habitación ya está reservada en las fechas seleccionadas." });
                }

                var Estado = true;
                var reserva = new tReservas
                {
                    IdUsuario = idUsuario,
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

        private void CargarListas()
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.MetodosPago = new SelectList(context.tMetodoPago.ToList(), "IdMetodoP", "NombreMetodoP");
                ViewBag.Monedas = new SelectList(context.tMonedas.ToList(), "IdMoneda", "NombreMoneda");
            }
        }

        [HttpGet]
        public ActionResult ModificarReserva(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tReservas.Where(x => x.IdReserva == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró la reserva.";
                    return RedirectToAction("AdministrarReservas", "Reserva");
                }

                var reserva = new Reserva()
                {
                    IdReserva = model.IdReserva,
                    IdUsuario = model.IdUsuario,
                    IdHabitacion = model.IdHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Estado = model.Estado,
                    IdMoneda = model.IdMoneda,
                    IdMetodoP = model.IdMetodoP
                };

                CargarListas();
                return View(reserva);
            }
        }

        [HttpPost]
        public ActionResult ModificarReserva(Reserva model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tReservas.Where(x => x.IdReserva == model.IdReserva).FirstOrDefault();
                if (datos != null)
                {
                    datos.CheckIn = model.CheckIn;
                    datos.CheckOut = model.CheckOut;
                    datos.Estado = model.Estado;
                    datos.IdMoneda = model.IdMoneda;
                    datos.IdMetodoP = model.IdMetodoP;

                    context.SaveChanges();
                    return RedirectToAction("AdministrarReservas", "Reserva");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            }
            CargarListas();
            return View(model);
        }

    }
}
