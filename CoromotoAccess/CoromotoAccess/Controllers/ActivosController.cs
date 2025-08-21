using Antlr.Runtime.Misc;
using CoromotoAccess.Filters;
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
    [AuthRequired]

    public class ActivosController : Controller
    {

        [AuthRequired(Roles = "Administrador, Empleado")]
        [HttpGet]
        public ActionResult AdministrarActivos()
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tActivos
                    .Join(context.tHabitaciones,
                        a => a.IdHabitacion,
                        h => h.IdHabitacion,
                        (a, h) => new { Activo = a, Habitacion = h })
                    .Join(context.tCategorias,
                        ah => ah.Activo.IdCategoria,
                        c => c.IdCategoria,
                        (ah, c) => new { ah.Activo, ah.Habitacion, CategoriaNombre = c.Nombre })
                    .Join(context.tVillas,
                        ahc => ahc.Habitacion.IdVilla,
                        v => v.IdVilla,
                        (ahc, v) => new { ahc.Activo, ahc.Habitacion, ahc.CategoriaNombre, Villa = v })
                    .ToList();

                var listaActivos = new List<Activo>();

                foreach (var item in datos)
                {
                    listaActivos.Add(new Activo
                    {
                        IdActivo = item.Activo.IdActivo, // AÑADIR ESTO
                        IdHabitacion = item.Activo.IdHabitacion,
                        Nombre = item.Activo.Nombre,
                        Modelo = item.Activo.Modelo,
                        NumeroSerie = item.Activo.NumeroSerie,
                        Descripcion = item.Activo.Descripcion,
                        IdCategoria = item.Activo.IdCategoria,
                        CategoriaNombre = item.CategoriaNombre,
                        IdVilla = item.Villa.IdVilla,
                        VillaNombre = item.Villa.NombreHabitacion
                    });
                }
                ViewBag.Categorias = new SelectList(context.tCategorias.ToList(), "IdCategoria", "Nombre");
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");
                return View(listaActivos);
            }
            
        }


        [HttpGet]
        [AuthRequired(Roles = "Administrador, Empleado")]
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
        [AuthRequired(Roles = "Administrador, Empleado")]
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
        [AuthRequired(Roles = "Administrador")]
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
        [AuthRequired(Roles = "Administrador, Empleado")]
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
        [ValidateAntiForgeryToken]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult ModificarActivo(Activo model)
        {
            // Validación de campos vacíos
            if (string.IsNullOrWhiteSpace(model.Nombre) ||
                string.IsNullOrWhiteSpace(model.Modelo) ||
                string.IsNullOrWhiteSpace(model.NumeroSerie) ||
                string.IsNullOrWhiteSpace(model.Descripcion))
            {
                ModelState.AddModelError("", "Todos los campos son obligatorios");
            }

            // Validación específica para número de serie (solo números)
            if (!long.TryParse(model.NumeroSerie, out _))
            {
                ModelState.AddModelError("NumeroSerie", "El número de serie solo puede contener dígitos");
            }

            if (!ModelState.IsValid)
            {
                // Recargar listas si hay error
                using (var context = new BDCoromotoEntities())
                {
                    ViewBag.Categorias = new SelectList(
                        context.tCategorias.ToList(),
                        "IdCategoria",
                        "Nombre",
                        model.IdCategoria
                    );

                    ViewBag.Habitaciones = new SelectList(
                        context.tHabitaciones.ToList(),
                        "IdHabitacion",
                        "NombreHabitacion",
                        model.IdHabitacion
                    );
                }
                return View(model);
            }

            using (var context = new BDCoromotoEntities())
            {
                var activo = context.tActivos.Find(model.IdActivo);

                if (activo == null)
                {
                    TempData["MensajePantalla"] = "Activo no encontrado";
                    return RedirectToAction("AdministrarActivos");
                }

                // Actualizar propiedades con trim
                activo.IdHabitacion = model.IdHabitacion;
                activo.Nombre = model.Nombre.Trim();
                activo.Modelo = model.Modelo.Trim();
                activo.NumeroSerie = model.NumeroSerie.Trim();
                activo.Descripcion = model.Descripcion.Trim();
                activo.IdCategoria = model.IdCategoria;

                context.SaveChanges();

                TempData["MensajeExito"] = "Activo modificado exitosamente.";
                return RedirectToAction("AdministrarActivos");
            }
        }
    }
}