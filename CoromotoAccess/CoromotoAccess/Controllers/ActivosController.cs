using Antlr.Runtime.Misc;
using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ActivosController : Controller
    {
        [HttpGet]
        public ActionResult AdministrarActivos()
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tActivos.ToList();

                var listaActivos = new List<Activo>();

                foreach (var item in datos)
                {
                    listaActivos.Add(new Activo
                    {
                        IdActivo = item.IdActivo,
                        IdHabitacion = item.IdHabitacion,
                        Nombre = item.Nombre,
                        Modelo = item.Modelo,
                        NumeroSerie = item.NumeroSerie,
                        Descripcion = item.Descripcion,
                        IdCategoria = item.IdCategoria,
                    });
                }
                ViewBag.Categorias = new SelectList(context.tCategorias.ToList(), "IdCategoria", "Nombre");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");
                return View(listaActivos);
            }
            
        }


        [HttpGet]
        public ActionResult CrearActivo()
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.Categorias = new SelectList(context.tCategorias.ToList(), "IdCategoria", "Nombre");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");
            }
            return View();
        }



        [HttpPost]
        public ActionResult CrearActivo(Activo activo)
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.Categorias = new SelectList(context.tCategorias.ToList(), "IdCategoria", "Nombre");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");

                try
                {
                    var nuevoActivo = new tActivos
                    {
                        IdHabitacion = activo.IdHabitacion,
                        Nombre = activo.Nombre,
                        Modelo = activo.Modelo,
                        NumeroSerie = activo.NumeroSerie,
                        Descripcion = activo.Descripcion,
                        IdCategoria = activo.IdCategoria
                    };

                    context.tActivos.Add(nuevoActivo);
                    context.SaveChanges();

                    return RedirectToAction("AdministrarActivos");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return View(activo);
                }
            }
        }



        public ActionResult ModificarActivo()
        {
            return View();
        }

        public ActionResult EliminarActivo()
        {
            return View();
        }

    }
}