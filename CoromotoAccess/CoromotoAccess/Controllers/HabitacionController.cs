using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class HabitacionController : Controller
    {
        [HttpGet]
        public ActionResult CatalogoHabitaciones()
        {
            using (var context = new BDCoromotoEntities()) //Cambiar conección
            {
                var datos = context.tHabitaciones.ToList();

                var habitaciones = new List<Habitacion>();
                foreach (var item in datos)
                {
                    habitaciones.Add(new Habitacion
                    {
                        IdHabitacion = item.IdHabitacion,
                        NombreHabitacion = item.NombreHabitacion,
                        Descripcion = item.Descripcion,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut,
                        Precio = item.Precio,
                        Estado = item.Estado,
                        IdVilla = item.IdVilla,
                    });
                }
                ViewBag.Villas = context.tVillas.ToList();
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipodeHabitacion", "NombreTipodeHabitcion");
                return View(habitaciones);
            }
        }

        [HttpGet]
        public ActionResult AdministrarHabitaciones()
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tHabitaciones.ToList();

                var habitaciones = new List<Habitacion>();
                foreach (var item in datos)
                {
                    habitaciones.Add(new Habitacion
                    {
                        IdHabitacion = item.IdHabitacion,
                        NombreHabitacion = item.NombreHabitacion,
                        Descripcion = item.Descripcion,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut,
                        Precio = item.Precio,
                        Estado = item.Estado,
                        IdVilla = item.IdVilla,
                    });
                }
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipodeHabitacion", "NombreTipodeHabitcion");
                return View(habitaciones);
            }
        }



        [HttpGet]
        public ActionResult AgregarHabitaciones()
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipodeHabitacion", "NombreTipodeHabitcion");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AgregarHabitacion(Habitacion model)
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipodeHabitacion", "NombreTipodeHabitcion");
                try
                {
                    var nuevaHabitacion = new tHabitaciones
                    {
                        NombreHabitacion = model.NombreHabitacion,
                        Descripcion = model.Descripcion,
                        Precio = model.Precio,
                        CheckIn = model.CheckIn,
                        CheckOut = model.CheckOut,
                        Estado = model.Estado,
                        IdVilla = model.IdVilla,
                        IdTipoHabitacion = model.IdTipoHabitacion
                    };

                    context.tHabitaciones.Add(nuevaHabitacion);
                    context.SaveChanges();

                    return RedirectToAction("AdministrarHabitaciones");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return View(model);
                }
            }
        }
        [HttpPost]
        public ActionResult EliminarHabitacion(long IdHabitacion)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    var habitacion = context.tHabitaciones.Find(IdHabitacion);

                    if (habitacion == null)
                    {
                        TempData["Mensaje"] = "La habitacion no existe.";
                        return RedirectToAction("AdministrarHabitaciones");
                    }

                    context.tHabitaciones.Remove(habitacion);
                    context.SaveChanges();

                    TempData["Mensaje"] = "Habitación eliminada exitosamente.";
                    return RedirectToAction("AdministrarHabitaciones");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar la habitacion.";
                return RedirectToAction("AdministrarHabitaciones");
            }
        }



        [HttpGet]
        public ActionResult ModificarHabitacion(long id)
        {

            using (var context = new BDCoromotoEntities())
            {
                var model = context.tHabitaciones.Where(x => x.IdHabitacion == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró el producto.";
                    return RedirectToAction("AdministrarHabitaciones", "Habitacion");
                }

                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipodeHabitacion", "NombreTipodeHabitcion");

                var habitacion = new Habitacion()
                {
                    IdHabitacion = model.IdHabitacion,
                    NombreHabitacion = model.NombreHabitacion,
                    Descripcion = model.Descripcion,
                    Precio = model.Precio,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Estado = model.Estado,
                    IdVilla = model.IdVilla,
                    IdTipoHabitacion = model.IdTipoHabitacion
                };

                return View(habitacion);
            }
        }
        [HttpPost]
        public ActionResult ModificarHabitacion(Habitacion model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tHabitaciones.Where(x => x.IdHabitacion == model.IdHabitacion).FirstOrDefault();
                if (datos != null)
                {
                    datos.NombreHabitacion = model.NombreHabitacion;
                    datos.Descripcion = model.Descripcion;
                    datos.Precio = model.Precio;
                    datos.CheckIn = model.CheckIn;
                    datos.CheckOut = model.CheckOut;
                    datos.Estado = model.Estado;
                    datos.IdVilla = model.IdVilla;
                    datos.IdTipoHabitacion = model.IdTipoHabitacion;

                    context.SaveChanges();
                    return RedirectToAction("AdministrarHabitaciones", "Habitacion");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DatosHabitacion(int id)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    ViewBag.Monedas = new SelectList(context.tMonedas.ToList(), "IdMoneda", "NombreMoneda");
                    ViewBag.MetodosPago = new SelectList(context.tMetodoPago.ToList(), "IdMetodoP", "NombreMetodoP");
                    var habitacion = context.tHabitaciones.Find(id);

                    if (habitacion == null)
                    {
                        ViewBag.ErrorMessage = "Habitación no encontrada.";
                        return View("Error");
                    }

                    var habitaciones = new List<Habitacion>
            {
                new Habitacion
                {
                    IdHabitacion = habitacion.IdHabitacion,
                    NombreHabitacion = habitacion.NombreHabitacion,
                    Descripcion = habitacion.Descripcion,
                    Precio = habitacion.Precio,
                    CheckIn = habitacion.CheckIn,
                    CheckOut = habitacion.CheckOut,
                    Estado = habitacion.Estado,
                    IdVilla = habitacion.IdVilla,
                }
            };

                    ViewBag.Habitaciones = habitaciones;
                    return View(new Reserva { IdHabitacion = habitacion.IdHabitacion });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar los datos de la habitación.";
                return View("Error");
            }
        }

    }
}