using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class LoginController : Controller
 {
    // GET: Login/InicioSesion
        [HttpGet]
        public ActionResult InicioSesion() {
            return View();
        }

        [HttpGet]
        public ActionResult Registro() {
            return View();
        }
        [HttpGet]
        public ActionResult RecuperarAcceso()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ActualizarPerfil()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MiPerfil()
        {
            return View();
        }
    }
}




