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
                var datos = context.tActivos.Join(context.tCategorias,
                a => a.IdCategoria,
                c => c.IdCategoria,
                (a, c) => new { Activo = a, CategoriaNombre = c.Nombre }).ToList();

                var listaActivos = new List<Activo>();

                foreach (var item in datos)
                {
                    listaActivos.Add(new Activo
                    {
                        IdHabitacion = item.Activo.IdHabitacion,
                        Nombre = item.Activo.Nombre,
                        Modelo = item.Activo.Modelo,
                        NumeroSerie = item.Activo.NumeroSerie,
                        Descripcion = item.Activo.Descripcion,
                        IdCategoria =item.Activo.IdCategoria,
                        CategoriaNombre = item.CategoriaNombre,
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

        //Eliminar Actvo

        [HttpPost]
        public ActionResult EliminarActivo(long IdActivo)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    var activo = context.tActivos.Find(IdActivo);

                    if (activo == null)
                    {
                        TempData["Mensaje"] = "El activo no existe.";
                        return RedirectToAction("AdministrarActivos");
                    }

                    context.tActivos.Remove(activo);
                    context.SaveChanges();

                    TempData["Mensaje"] = "Activo eliminado exitosamente.";
                    return RedirectToAction("AdministrarActivos");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el activo.";
                return RedirectToAction("AdministrarActivos");
            }
        }

    //Modificar Activo
        
        [HttpGet]
        public ActionResult ModificarActivo(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tActivos.Where(x => x.IdActivo == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró el activo.";
                    return RedirectToAction("AdministrarActivos");
                }

                // Cargar las listas desplegables
               ViewBag.Categorias = new SelectList(context.tCategorias.ToList(), "IdCategoria", "Nombre");
               ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");

                var activo = new Activo()
                {
                    IdActivo = model.IdActivo,
                    IdHabitacion = model.IdHabitacion,
                    Nombre = model.Nombre,
                    Modelo = model.Modelo,
                    NumeroSerie = model.NumeroSerie,
                    Descripcion = model.Descripcion,
                    IdCategoria = model.IdCategoria
                };

                return View(activo);
            }
        }

        [HttpPost]
        public ActionResult ModificarActivo(Activo model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tActivos.Where(x => x.IdActivo == model.IdActivo).FirstOrDefault();

                if (datos != null)
                {
                    datos.IdHabitacion = model.IdHabitacion;
                    datos.Nombre = model.Nombre;
                    datos.Modelo = model.Modelo;
                    datos.NumeroSerie = model.NumeroSerie;
                    datos.Descripcion = model.Descripcion;
                    datos.IdCategoria = model.IdCategoria;

                    context.SaveChanges();

                    TempData["Mensaje"] = "Activo modificado exitosamente.";
                    return RedirectToAction("AdministrarActivos");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente.";

                // Recargar listas desplegables para la vista
                ViewBag.Categorias = new SelectList(context.tCategorias.ToList(), "IdCategoria", "Nombre");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");
            }
            return View(model);
        }
    }
}