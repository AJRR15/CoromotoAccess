using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ParametrosController : Controller
    {
        // GET: Parametros
        public ActionResult HojaDeConfiguracion()
        {
            return View();
        }
        public ActionResult ModificarIdioma()
        {
            return View();
        }
        public ActionResult ModificarMetodoPago()
        {
            return View();
        }
        public ActionResult ModificarTipoHabitacion()
        {
            return View();
        }
        public ActionResult ModificarMoneda()
        {
            return View();
        }
    }
}