using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ActivosController : Controller
    {
        // GET: Activos
        public ActionResult AdministrarActivos()
        {
            return View();
        }
        public ActionResult ModificarActivo()
        {
            return View();
        }
    }
}