using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class SeguridadController : Controller
    {
        // GET: Seguridad
        public ActionResult GestionRoles()
        {
            return View();
        }
        public ActionResult GestionPermisos()
        {
            return View();
        }
        public ActionResult Seguridad()
        {
            return View();
        }
        public ActionResult Configuracion()
        {
            return View();
        }
    }
}