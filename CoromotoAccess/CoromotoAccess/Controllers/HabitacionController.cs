using CoromotoAccess.Filters;
using CoromotoAccess.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    [AuthRequired]
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
                        IdTipoHabitacion = item.IdTipoHabitacion,
                        img1 = item.img1

                    });
                }
                ViewBag.Villas = context.tVillas.ToList();
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipodeHabitacion", "NombreTipodeHabitcion");
                return View(habitaciones);
            }
        }
        
        [HttpGet]
        [AuthRequired(Roles = "Administrador")]
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
                        IdTipoHabitacion = item.IdTipoHabitacion
                    });
                }
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones.ToList(), "IdTipodeHabitacion", "NombreTipodeHabitcion");
                return View(habitaciones);
            }
        }



        [HttpGet]
        [AuthRequired(Roles = "Administrador")]
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
        [AuthRequired(Roles = "Administrador")]
        public ActionResult AgregarHabitacion(Habitacion model,
     HttpPostedFileBase img1File,
     HttpPostedFileBase img2File,
     HttpPostedFileBase img3File,
     HttpPostedFileBase img4File,
     HttpPostedFileBase img5File,
     HttpPostedFileBase img6File)
        {
            using (var context = new BDCoromotoEntities())
            {
                // Método interno para guardar archivos y devolver la ruta
                string GuardarImagen(HttpPostedFileBase file)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var folderPath = Server.MapPath("~/Content/ImagenesHabitaciones");

                        // Crear carpeta si no existe
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        var fullPath = Path.Combine(folderPath, fileName);
                        file.SaveAs(fullPath);

                        return "/Content/ImagenesHabitaciones/" + fileName;
                    }
                    return null;
                }

                try
                {
                    if (!ModelState.IsValid)
                    {
                        // Recargar combos si hay error de validación
                        ViewBag.Villas = new SelectList(context.tVillas, "IdVilla", "NombreHabitacion");
                        ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones, "IdTipodeHabitacion", "NombreTipodeHabitacion");
                        return View(model);
                    }

                    var nuevaHabitacion = new tHabitaciones
                    {
                        NombreHabitacion = model.NombreHabitacion,
                        Descripcion = model.Descripcion,
                        Precio = model.Precio,
                        CheckIn = model.CheckIn,
                        CheckOut = model.CheckOut,
                        Estado = model.Estado,
                        IdVilla = model.IdVilla,
                        IdTipoHabitacion = model.IdTipoHabitacion,
                        img1 = GuardarImagen(img1File),
                        img2 = GuardarImagen(img2File),
                        img3 = GuardarImagen(img3File),
                        img4 = GuardarImagen(img4File),
                        img5 = GuardarImagen(img5File),
                        img6 = GuardarImagen(img6File)
                    };

                    context.tHabitaciones.Add(nuevaHabitacion);
                    context.SaveChanges();

                    return RedirectToAction("AdministrarHabitaciones");
                }
                catch (Exception ex)
                {
                    // Mostrar mensaje de error
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";

                    // Recargar combos para que el modal no quede vacío
                    ViewBag.Villas = new SelectList(context.tVillas, "IdVilla", "NombreHabitacion");
                    ViewBag.TiposHabitacion = new SelectList(context.tTiposHabitaciones, "IdTipodeHabitacion", "NombreTipodeHabitacion");

                    return View(model);
                }
            }
        }


        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
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
        [AuthRequired(Roles = "Administrador")]
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
                    IdTipoHabitacion = model.IdTipoHabitacion,
                    img1 = model.img1,
                    img2 = model.img2,
                    img3 = model.img3,
                    img4 = model.img4,
                    img5 = model.img5,
                    img6 = model.img6
                };


                return View(habitacion);
            }
        }


    [HttpPost]
    [AuthRequired(Roles = "Administrador")]
    public ActionResult ModificarHabitacion(Habitacion model,
                        HttpPostedFileBase img1File, HttpPostedFileBase img2File,
                        HttpPostedFileBase img3File, HttpPostedFileBase img4File,
                        HttpPostedFileBase img5File, HttpPostedFileBase img6File)
    {
        // Parsear precio manualmente desde el campo "Precio"
        var rawPrecio = Request.Form["Precio"];
        if (!decimal.TryParse(rawPrecio, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
        {
            ViewBag.MensajePantalla = "El precio ingresado no es válido.";
            return View(model);
        }
        model.Precio = precio;

        using (var context = new BDCoromotoEntities())
        {
            var datos = context.tHabitaciones.FirstOrDefault(x => x.IdHabitacion == model.IdHabitacion);
            if (datos != null)
            {
                // Método para guardar imágenes
                string GuardarImagen(HttpPostedFileBase file, string actual)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var folderPath = Server.MapPath("~/Content/ImagenesHabitaciones");

                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        var fullPath = Path.Combine(folderPath, fileName);
                        file.SaveAs(fullPath);

                        return "/Content/ImagenesHabitaciones/" + fileName;
                    }
                    return actual; // si no subieron nada, mantener imagen anterior
                }

                // Actualizar campos
                datos.NombreHabitacion = model.NombreHabitacion;
                datos.Descripcion = model.Descripcion;
                datos.Precio = model.Precio;
                datos.CheckIn = model.CheckIn;
                datos.CheckOut = model.CheckOut;
                datos.Estado = model.Estado;
                datos.IdVilla = model.IdVilla;
                datos.IdTipoHabitacion = model.IdTipoHabitacion;

                // Actualizar imágenes solo si se subió una nueva
                datos.img1 = GuardarImagen(img1File, datos.img1);
                datos.img2 = GuardarImagen(img2File, datos.img2);
                datos.img3 = GuardarImagen(img3File, datos.img3);
                datos.img4 = GuardarImagen(img4File, datos.img4);
                datos.img5 = GuardarImagen(img5File, datos.img5);
                datos.img6 = GuardarImagen(img6File, datos.img6);

                context.SaveChanges();
                return RedirectToAction("AdministrarHabitaciones", "Habitacion");
            }

            ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            return View(model);
        }
    }

    [HttpGet]
        public ActionResult DatosHabitacion(int id, string errorMessage = null)
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
                    img1 = habitacion.img1,
                    img2 = habitacion.img2,
                    img3 = habitacion.img3,
                    img4 = habitacion.img4,
                    img5 = habitacion.img5,
                    img6 = habitacion.img6,
                }
            };

                    ViewBag.Habitaciones = habitaciones;
                    ViewBag.ErrorMessage = errorMessage;

                    // Cargar reservas válidas y materializar ANTES de transformar fechas
                    var reservas = context.tReservas
                        .Where(r => r.IdHabitacion == habitacion.IdHabitacion
                                 && r.CheckIn != null
                                 && r.CheckOut != null
                                 && r.CheckOut >= r.CheckIn
                                 && r.Estado == true) // solo reservas activas
                        .Select(r => new { r.CheckIn, r.CheckOut })
                        .ToList();

                    // Generar lista de días ocupados (yyyy-MM-dd) en memoria
                    var fechasOcupadas = reservas
                        .SelectMany(r =>
                        {
                            var inicio = r.CheckIn.Date;
                            var fin = r.CheckOut.Date;
                            var dias = (fin - inicio).Days + 1; // inclusivo
                                                                // Limitar a una ventana razonable para evitar bloquear todo el calendario si hay datos malos
                            if (dias <= 0 || dias > 730) return Enumerable.Empty<string>();
                            return Enumerable.Range(0, dias)
                                .Select(offset => inicio.AddDays(offset).ToString("yyyy-MM-dd"));
                        })
                        .Distinct()
                        .ToList();

                    ViewBag.FechasOcupadas = fechasOcupadas;

                    return View(new Reserva { IdHabitacion = habitacion.IdHabitacion });
                }
            }
            catch
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar los datos de la habitación.";
                return View("Error");
            }
        }

    }
}