using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class HabitacionController : Controller
    {
        public ActionResult CatalogoHabitaciones()
        {
            return View();
        }
        public ActionResult AdministrarHabitaciones()
        {
            return View();
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