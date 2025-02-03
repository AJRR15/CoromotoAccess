using CoromotoAccess.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ParametrosController : Controller
    {
        //Vistas
        public ActionResult HojaDeConfiguracion()
        {
            using (var context = new BDCoromotoEntities())
            {
                var metodosPago = context.tMetodoPago.ToList();
                var tiposHabitacion = context.tTiposHabitaciones.ToList();
                var tiposMoneda = context.tMonedas.ToList();
                var villas = context.tVillas.ToList();

                var parametros = new Parametros
                {
                    MetodosPago = metodosPago,
                    TiposHabitacion = tiposHabitacion,
                    TiposMoneda = tiposMoneda,
                    Villas = villas
                };

                return View(parametros);
            }
        }


        //Villas
        [HttpPost]
        public ActionResult AgregarVilla(Villa model)
        {
            using (var context = new BDCoromotoEntities())
                try
                {
                    {
                        var nuevaVilla = new tVillas
                        {
                            NombreHabitacion = model.NombreHabitacion
                        };
                        context.tVillas.Add(nuevaVilla);
                        context.SaveChanges();
                    }
                    return RedirectToAction("HojaDeConfiguracion");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return View(model);
                }
        }
        [HttpPost]
        public ActionResult EliminarVilla(long IdVilla)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    var villa = context.tVillas.Find(IdVilla);

                    if (IdVilla == null)
                    {
                        TempData["Mensaje"] = "La Villa no existe.";
                        return RedirectToAction("HojaDeConfiguracion");
                    }

                    context.tVillas.Remove(villa);
                    context.SaveChanges();

                    TempData["Mensaje"] = "villa eliminada exitosamente.";
                    return RedirectToAction("HojaDeConfiguracion");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar la villa.";
                return RedirectToAction("HojaDeConfiguracion");
            }
        }
        [HttpGet]
        public ActionResult ModificarVilla(long id)
        {

            using (var context = new BDCoromotoEntities())
            {
                var model = context.tVillas.Where(x => x.IdVilla == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró la villa.";
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                var villa = new Villa()
                {
                    IdVilla = model.IdVilla,
                    NombreHabitacion = model.NombreHabitacion,

                };

                return View(villa);
            }
        }

        [HttpPost]
        public ActionResult ModificarVilla(Villa model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tVillas.Where(x => x.IdVilla == model.IdVilla).FirstOrDefault();
                if (datos != null)
                {
                    datos.NombreHabitacion = model.NombreHabitacion;

                    context.SaveChanges();
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            }
            return View(model);
        }


        //Metodos de Pago
        [HttpPost]
        public ActionResult AgregarMetodoP(MetodoPago model)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                    var metodoP = new tMetodoPago
                    {
                        NombreMetodoP = model.NombreMetodoP
                    };
                    context.tMetodoPago.Add(metodoP);
                    context.SaveChanges();
                    return RedirectToAction("HojaDeConfiguracion");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return View(model);
                }
            }
        }

        [HttpPost]
        public ActionResult EliminarMetodoP(long IdMetodoP)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    var metodoP = context.tMetodoPago.Find(IdMetodoP);

                    if (IdMetodoP == null)
                    {
                        TempData["Mensaje"] = "El metodo no existe.";
                        return RedirectToAction("HojaDeConfiguracion");
                    }

                    context.tMetodoPago.Remove(metodoP);
                    context.SaveChanges();

                    TempData["Mensaje"] = "Metodo de pago eliminado eliminada exitosamente.";
                    return RedirectToAction("HojaDeConfiguracion");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el metodo de pago.";
                return RedirectToAction("HojaDeConfiguracion");
            }
        }

        [HttpGet]
        public ActionResult ModificarMetodoPago(long id)
        {

            using (var context = new BDCoromotoEntities())
            {
                var model = context.tMetodoPago.Where(x => x.IdMetodoP == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró el metodo de pago.";
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                var metodoP = new MetodoPago()
                {
                    IdMetodoP = model.IdMetodoP,
                    NombreMetodoP = model.NombreMetodoP,

                };

                return View(metodoP);
            }
        }

        [HttpPost]
        public ActionResult ModificarMetodoPago(MetodoPago model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tMetodoPago.Where(x => x.IdMetodoP == model.IdMetodoP).FirstOrDefault();
                if (datos != null)
                {
                    datos.NombreMetodoP = model.NombreMetodoP;

                    context.SaveChanges();
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            }
            return View(model);
        }



        //Tipos de Habitaciones
        [HttpPost]
        public ActionResult AgregarTipoHabitacion(TiposHabitaciones model)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                    var tipoHabitacion = new tTiposHabitaciones
                    {
                        NombreTipodeHabitcion = model.NombreTipodeHabitacion
                    };
                    context.tTiposHabitaciones.Add(tipoHabitacion);
                    context.SaveChanges();
                    return RedirectToAction("HojaDeConfiguracion");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return View(model);
                }
            }
        }

        [HttpPost]
        public ActionResult EliminarTipoHabitacion(long IdTipodeHabitacion)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    var tipoHabitacion = context.tTiposHabitaciones.Find(IdTipodeHabitacion);

                    if (IdTipodeHabitacion == null)
                    {
                        TempData["Mensaje"] = "El tipo de habitación no existe.";
                        return RedirectToAction("HojaDeConfiguracion");
                    }

                    context.tTiposHabitaciones.Remove(tipoHabitacion);
                    context.SaveChanges();

                    TempData["Mensaje"] = "Tipo de habitación eliminado exitosamente.";
                    return RedirectToAction("HojaDeConfiguracion");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el tipo de habitación.";
                return RedirectToAction("HojaDeConfiguracion");
            }
        }

        [HttpGet]
        public ActionResult ModificarTipoHabitacion(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tTiposHabitaciones.Where(x => x.IdTipodeHabitacion == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró el tipo de habitación.";
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                var tipoHabitacion = new TiposHabitaciones()
                {
                    IdTipodeHabitacion = model.IdTipodeHabitacion,
                    NombreTipodeHabitacion = model.NombreTipodeHabitcion,
                };

                return View(tipoHabitacion);
            }
        }

        [HttpPost]
        public ActionResult ModificarTipoHabitacion(TiposHabitaciones model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tTiposHabitaciones.Where(x => x.IdTipodeHabitacion == model.IdTipodeHabitacion).FirstOrDefault();
                if (datos != null)
                {
                    datos.NombreTipodeHabitcion = model.NombreTipodeHabitacion;

                    context.SaveChanges();
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            }
            return View(model);
        }



        [HttpPost]
        public ActionResult AgregarTipoMoneda(TipoMoneda model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var tipoMoneda = new tMonedas
                {
                    NombreMoneda = model.NombreMoneda
                };
                context.tMonedas.Add(tipoMoneda);
                context.SaveChanges();
                return RedirectToAction("HojaDeConfiguracion");
            }
        }

        [HttpPost]
        public ActionResult EliminarTipoMoneda(long IdMoneda)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    var tipoMoneda = context.tMonedas.Find(IdMoneda);

                    if (tipoMoneda == null)
                    {
                        TempData["Mensaje"] = "El tipo de moneda no existe.";
                        return RedirectToAction("HojaDeConfiguracion");
                    }

                    context.tMonedas.Remove(tipoMoneda);
                    context.SaveChanges();

                    TempData["Mensaje"] = "Tipo de moneda eliminado exitosamente.";
                    return RedirectToAction("HojaDeConfiguracion");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el tipo de moneda.";
                return RedirectToAction("HojaDeConfiguracion");
            }
        }

        [HttpGet]
        public ActionResult ModificarMoneda(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tMonedas.Where(x => x.IdMoneda == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró el tipo de moneda.";
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                var tipoMoneda = new TipoMoneda()
                {
                    IdMoneda = model.IdMoneda,
                    NombreMoneda = model.NombreMoneda,
                };

                return View(tipoMoneda);
            }
        }

        [HttpPost]
        public ActionResult ModificarMoneda(TipoMoneda model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tMonedas.Where(x => x.IdMoneda == model.IdMoneda).FirstOrDefault();
                if (datos != null)
                {
                    datos.NombreMoneda = model.NombreMoneda;

                    context.SaveChanges();
                    return RedirectToAction("HojaDeConfiguracion", "Parametros");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            }
            return View(model);
        }
    }
}
