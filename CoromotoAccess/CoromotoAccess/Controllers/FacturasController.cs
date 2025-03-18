    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using CoromotoAccess.Models; // Asegúrate de tener el namespace correcto para los modelos
    using iTextSharp.text;
    using iTextSharp.text.pdf;


    namespace CoromotoAccess.Controllers
    {
        public class FacturasController : Controller
        {
            // GET: Facturas/Historico
            [HttpGet]
            public ActionResult Historico(DateTime? filtroFechaInicio, DateTime? filtroFechaFin, string cedulaCliente)
            {
                using (var context = new BDCoromotoEntities())
                {
                    var query = context.tFacturas.AsQueryable();

                    // Filtrar por rango de fechas
                    if (filtroFechaInicio != null && filtroFechaFin != null)
                    {
                        query = query.Where(f => f.FechaEmision >= filtroFechaInicio && f.FechaEmision <= filtroFechaFin);
                    }

                    // Filtrar por cédula del cliente
                    if (!string.IsNullOrEmpty(cedulaCliente))
                    {
                        var idsUsuarios = context.tUsuario
                                                .Where(u => u.Identificacion == cedulaCliente)
                                                .Select(u => u.ConsecutivoCliente)
                                                .ToList();
                        query = query.Where(f => idsUsuarios.Contains(f.IdUsuario));
                    }

                    var datos = query.ToList();

                    var facturas = new List<Factura>();
                    foreach (var item in datos)
                    {
                        facturas.Add(new Factura
                        {
                            IdFactura = item.IdFactura,
                            IdUsuario = item.IdUsuario,
                            CedulaCliente = item.tUsuario.Identificacion,
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

            public ActionResult DescargarFactura(int id)
            {
                using (var context = new BDCoromotoEntities())
                {
                    try
                    {
                        // Obtener datos de la factura con join a usuarios
                        var facturaData = context.tFacturas
                            .Join(context.tUsuario,
                                f => f.IdUsuario,
                                u => u.ConsecutivoCliente,
                                (f, u) => new { Factura = f, Usuario = u })
                            .FirstOrDefault(f => f.Factura.IdFactura == id);

                        if (facturaData == null)
                        {
                            TempData["Error"] = "Factura no encontrada";
                            return RedirectToAction("Historico");
                        }

                        // Configurar documento PDF
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                            PdfWriter writer = PdfWriter.GetInstance(document, ms);

                            document.Open();

                            // Estilos
                            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                            Font subtituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                            Font textoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                            // Encabezado
                            document.Add(new Paragraph("Factura Electrónica", tituloFont));
                            document.Add(new Paragraph($"Número: {facturaData.Factura.IdFactura}", textoFont));
                            document.Add(new Paragraph($"Fecha de Emisión: {facturaData.Factura.FechaEmision:dd/MM/yyyy}", textoFont));
                            document.Add(new Paragraph($"Estado: {facturaData.Factura.Estado}\n\n", textoFont));

                            // Datos del Cliente
                            document.Add(new Paragraph("Datos del Cliente:", subtituloFont));
                            document.Add(new Paragraph($"Nombre: {facturaData.Usuario.Nombre} {facturaData.Usuario.Apellido}", textoFont));
                            document.Add(new Paragraph($"Cédula: {facturaData.Usuario.Identificacion}\n\n", textoFont));

                            // Detalles de Pago
                            document.Add(new Paragraph("Detalles de Pago:", subtituloFont));
                            document.Add(new Paragraph($"Total: {facturaData.Factura.Total}", textoFont));

                            document.Close();

                            return File(ms.ToArray(), "application/pdf", $"Factura_{id}.pdf");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Error al generar PDF: {ex.Message}";
                        return RedirectToAction("Historico");
                    }
                }
            }
            [HttpPost]
            public ActionResult MarcarComoPagado(int id)
            {
                using (var context = new BDCoromotoEntities())
                {
                    try
                    {
                        var factura = context.tFacturas.Find(id);
                        if (factura != null)
                        {
                            factura.Estado = true;
                            context.SaveChanges();
                            TempData["Mensaje"] = "Factura marcada como pagada exitosamente.";
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Error al actualizar el estado: {ex.Message}";
                    }
                    return RedirectToAction("Historico");
                }
            }


        }
    }