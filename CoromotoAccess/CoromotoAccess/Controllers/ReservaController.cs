using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ReservaController : Controller
    {
        public ActionResult AdministrarReservas()
        {
            return View();
        }
    
    [HttpPost]
        public ActionResult AgregarReserva(Reserva model)
        {
            using (var context = new BDCoromotoEntities())
            {       
                var id = int.Parse(Session["Consecutivo"].ToString());


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

                    return RedirectToAction("AdministrarHabitaciones");
                }
        }
    }
}