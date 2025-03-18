using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CoromotoAccess.Models; // Asegúrate de tener el namespace correcto para los modelos

namespace CoromotoAccess.Controllers
{
    public class FacturasController : Controller
    {
        // GET: Facturas/Historico
        [HttpGet]
        public ActionResult Historico()
        {
            using (var context = new BDCoromotoEntities()) //Cambiar conección
            {
                var datos = context.tFacturas.ToList();

                var facturas = new List<Factura>();
                foreach (var item in datos)
                {
                    facturas.Add(new Factura
                    {
                        IdFactura = item.IdFactura,
                        IdUsuario = item.IdUsuario,
                        FechaEmision = item.FechaEmision,
                        imagen = item.Imagen,
                        total = item.Total,
                        Estado = item.Estado

                    });
                }
                ViewBag.Usuarios = new SelectList(context.tUsuario.ToList(), "ConsecutivoCliente", "Nombre");
                return View(facturas);
            }
        }

        // POST: Facturas/AgregarFactura
        [HttpPost]
        public ActionResult AgregarFactura(Factura factura)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                    var nuevaFactura = new tFacturas
                    {
                        IdUsuario = factura.IdUsuario,
                        FechaEmision = factura.FechaEmision,
                        Imagen = "Sin Imagen",
                        Total = factura.total,
                        Estado = factura.Estado
                        
                    };
                    context.tFacturas.Add(nuevaFactura); // Agrega la factura
                    context.SaveChanges();
                    TempData["Mensaje"] = "Factura agregada exitosamente.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al agregar la factura: {ex.Message}";
                }
                return RedirectToAction("Historico");
            }
        }
    }
}