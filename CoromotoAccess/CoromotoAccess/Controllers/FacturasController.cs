using CoromotoAccess.Filters;
using CoromotoAccess.Models; // Asegúrate de tener el namespace correcto para los modelos
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;


    namespace CoromotoAccess.Controllers
    {
        [AuthRequired]
        public class FacturasController : Controller
        {
            // GET: Facturas/Historico
            [HttpGet]
            [AuthRequired(Roles = "Administrador, Empleado")]
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
            [AuthRequired(Roles = "Administrador")]
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
                        Document document = new Document(PageSize.A4, 36, 36, 36, 36);
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        document.Open();

                        // ===== Estilos =====
                        var negro = BaseColor.BLACK;
                        var gris = new BaseColor(80, 80, 80);
                        Font fTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, negro);
                        Font fSub = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, negro);
                        Font fText = FontFactory.GetFont(FontFactory.HELVETICA, 10, negro);
                        Font fTiny = FontFactory.GetFont(FontFactory.HELVETICA, 8, gris);
                        Font fPaid = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 36, new BaseColor(5, 135, 60));
                        Font fPend = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 36, new BaseColor(200, 0, 0));

                        // ===== Encabezado con logo + empresa =====
                        PdfPTable header = new PdfPTable(2) { WidthPercentage = 100 };
                        header.SetWidths(new float[] { 1.2f, 2f });

                        // LOGO (pon tu imagen en /Content/img/logo.png)
                        var rutaLogo = Server.MapPath("~/Content/img/logo.png");
                        PdfPCell cLogo;
                        if (System.IO.File.Exists(rutaLogo))
                        {
                            var img = iTextSharp.text.Image.GetInstance(rutaLogo);
                            img.ScaleToFit(120f, 60f);
                            cLogo = new PdfPCell(img) { Border = Rectangle.NO_BORDER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        }
                        else
                        {
                            cLogo = new PdfPCell(new Phrase("TU EMPRESA", fSub)) { Border = Rectangle.NO_BORDER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        }
                        header.AddCell(cLogo);

                        // Datos de la empresa (EDITA A TU GUSTO)
                        var datosEmpresa =
                            "Coromoto Access S.A.\n" +
                            "Ced. Jurídica: 3-101-000000\n" +
                            "Dirección: San José, Costa Rica\n" +
                            "Tel: +506 2222-2222  •  soporte@coromoto.com";
                        header.AddCell(new PdfPCell(new Phrase(datosEmpresa, fText))
                        { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                        document.Add(header);

                        document.Add(new Paragraph(" "));
                        var line = new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -2);
                        document.Add(new Chunk(line));
                        document.Add(new Paragraph(" "));

                        // ===== Título + estado =====
                        var estadoTexto = facturaData.Factura.Estado ? "PAGADA" : "PENDIENTE";
                        PdfPTable meta = new PdfPTable(2) { WidthPercentage = 100 };
                        meta.SetWidths(new float[] { 1f, 1f });
                        meta.AddCell(new PdfPCell(new Phrase("Factura Electrónica", fTitulo)) { Border = Rectangle.NO_BORDER });
                        meta.AddCell(new PdfPCell(new Phrase(estadoTexto, facturaData.Factura.Estado ? fPaid : fPend))
                        { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                        document.Add(meta);

                        // ===== Datos generales =====
                        PdfPTable info = new PdfPTable(2) { WidthPercentage = 100 };
                        info.SetWidths(new float[] { 1f, 2f });
                        info.AddCell(new PdfPCell(new Phrase("Número:", fSub)) { Border = Rectangle.NO_BORDER });
                        info.AddCell(new PdfPCell(new Phrase(facturaData.Factura.IdFactura.ToString(), fText)) { Border = Rectangle.NO_BORDER });
                        info.AddCell(new PdfPCell(new Phrase("Fecha de Emisión:", fSub)) { Border = Rectangle.NO_BORDER });
                        info.AddCell(new PdfPCell(new Phrase($"{facturaData.Factura.FechaEmision:dd/MM/yyyy}", fText)) { Border = Rectangle.NO_BORDER });
                        document.Add(info);

                        document.Add(new Paragraph(" "));
                        document.Add(new Paragraph("Datos del Cliente", fSub));
                        PdfPTable cliente = new PdfPTable(2) { WidthPercentage = 100 };
                        cliente.SetWidths(new float[] { 1f, 2f });
                        cliente.AddCell(new PdfPCell(new Phrase("Nombre:", fText)));
                        cliente.AddCell(new PdfPCell(new Phrase($"{facturaData.Usuario.Nombre} {facturaData.Usuario.Apellido}", fText)));
                        cliente.AddCell(new PdfPCell(new Phrase("Cédula:", fText)));
                        cliente.AddCell(new PdfPCell(new Phrase(facturaData.Usuario.Identificacion ?? "-", fText)));
                        cliente.SpacingAfter = 8f;
                        document.Add(cliente);

                        // ===== Total desde BD (cadena) – formateo SE PUEDE y si no, crudo =====
                        var cultura = new System.Globalization.CultureInfo("es-CR");
                        string totalStrOriginal = Convert.ToString(facturaData.Factura.Total);
                        string totalFormateado = totalStrOriginal;  // por defecto, tal cual viene de BD

                        decimal totalDecimal;
                        if (decimal.TryParse(totalStrOriginal, System.Globalization.NumberStyles.Any,
                                             System.Globalization.CultureInfo.InvariantCulture, out totalDecimal) ||
                            decimal.TryParse(totalStrOriginal, out totalDecimal))
                        {
                            // si puede convertirse, lo mostramos bonito (₡ y separadores)
                            totalFormateado = totalDecimal.ToString("C", cultura);
                        }

                        // ===== Tabla de ítems simple (sin tocar BD) =====
                        PdfPTable items = new PdfPTable(4) { WidthPercentage = 100 };
                        items.SetWidths(new float[] { 3f, 1f, 1f, 1f });
                        PdfPCell H(string t) => new PdfPCell(new Phrase(t, fSub))
                        { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(245, 245, 245) };

                        items.AddCell(H("Descripción"));
                        items.AddCell(H("Cant."));
                        items.AddCell(H("P. Unitario"));
                        items.AddCell(H("Importe"));

                        // Línea genérica (si más adelante tienes detalle real, aquí iteras)
                        items.AddCell(new PdfPCell(new Phrase("Servicio/Reserva", fText)));
                        items.AddCell(new PdfPCell(new Phrase("1", fText)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        items.AddCell(new PdfPCell(new Phrase(totalFormateado, fText)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                        items.AddCell(new PdfPCell(new Phrase(totalFormateado, fText)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                        items.SpacingAfter = 6f;
                        document.Add(items);

                        // ===== Totales (solo muestra TOTAL; sin cálculos si el Total es string) =====
                        PdfPTable tot = new PdfPTable(2) { WidthPercentage = 40, HorizontalAlignment = Element.ALIGN_RIGHT };
                        tot.SetWidths(new float[] { 1f, 1f });
                        tot.AddCell(new PdfPCell(new Phrase("TOTAL", fSub)) { Border = Rectangle.TOP_BORDER });
                        tot.AddCell(new PdfPCell(new Phrase(totalFormateado, fSub))
                        { Border = Rectangle.TOP_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                        document.Add(tot);

                        // ===== Pie =====
                        document.Add(new Paragraph(" "));
                        document.Add(new Paragraph("Observaciones:", fSub));
                        document.Add(new Paragraph("Gracias por su compra.", fTiny));

                        document.Close();
                        byte[] bytes = ms.ToArray();
                        return File(bytes, "application/pdf", $"Factura_{facturaData.Factura.IdFactura}.pdf");
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
            [AuthRequired(Roles = "Administrador, Empleado")]
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



        [HttpGet]
        public ActionResult MisFacturas(long? idUsuario)
        {
            if (idUsuario == null)
            {
                ViewBag.MensajePantalla = "Por favor, inicie sesión.";
                return RedirectToAction("Index", "Home");
            }

            using (var context = new BDCoromotoEntities())
            {
                var facturas = context.tFacturas
                    .Where(r => r.IdUsuario == idUsuario)
                    .Select(r => new Factura
                    {
                        IdFactura = r.IdFactura,
                        IdUsuario = r.IdUsuario,
                        FechaEmision = r.FechaEmision,
                        imagen = r.Imagen,
                        total = r.Total,
                        Estado = r.Estado
                    }).ToList();

                if (!facturas.Any())
                {
                    ViewBag.MensajePantalla = "No se encontraron facturas.";
                    return View(facturas);
                }

                var usuario = context.tUsuario.FirstOrDefault(u => u.ConsecutivoCliente == idUsuario);
                ViewBag.Usuario = usuario?.Nombre + " " + usuario?.Apellido;

                return View(facturas);
            }
        }
    }
    }