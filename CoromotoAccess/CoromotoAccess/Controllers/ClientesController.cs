using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult GestionClientes()
        {
            return View();
        }
        public ActionResult ModificarCliente()
        {
            return View();
        }
        public ActionResult HistorialCliente()
        {
            return View();
        }
    }
}