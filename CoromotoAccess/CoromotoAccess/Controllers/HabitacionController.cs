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
                        Estado = item.Estado,
                        IdVilla = item.IdVilla,
                    });
                }
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
                        CheckIn = item.CheckIn,  // Asegúrate de que sea tipo DateTime
                        CheckOut = item.CheckOut, // Asegúrate de que sea tipo DateTime
                        Precio = item.Precio,     // Campo crítico agregado
                        Estado = item.Estado,
                        IdVilla = item.IdVilla,
                    });
                }
                return View(habitaciones); // Ahora la vista recibe una lista
            }
        }


        [HttpGet]
        public ActionResult AgregarHabitaciones()
        {
            using (var context = new BDCoromotoEntities())
            {
                // Cargar datos para los dropdowns
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreVilla");
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipoHabitacion", "NombreTipo");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AgregarHabitacion(Habitacion model)
        {
            using (var context = new BDCoromotoEntities())
            {
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
        public ActionResult ModificarHabitacion()
        {
            return View();
        }
        public ActionResult DatosHabitacion()
        {
            return View();
        }
    }
}