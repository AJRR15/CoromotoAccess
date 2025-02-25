using CoromotoAccess.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class LimpiezaController : Controller
    {
        [HttpGet]
        public ActionResult ControlDeLimpieza()
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tLimpiezas.ToList();
                var limpiezas = new List<Limpieza>();

                foreach (var d in datos)
                {
                    limpiezas.Add(new Limpieza
                    {
                        IdLimpieza = d.IdLimpieza,
                        IdEmpleado = d.IdEmpleado,
                        IdHabitacion = d.IdHabitacion,
                        FechaLimpieza = d.FechaLimpieza,
                        Estado = d.Estado
                    });
                }

                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");
                ViewBag.DiccionarioEmpleados = context.tEmpleados.ToDictionary(e => e.ConsecutivoEmp, e => e.Nombre);
                ViewBag.DiccionarioHabitaciones = context.tHabitaciones.ToDictionary(h => h.IdHabitacion, h => h.NombreHabitacion);

                return View(limpiezas);
            }
        }

        [HttpGet]
        public ActionResult AsignarLimpieza()
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");
            }
            return RedirectToAction("ControlDeLimpieza");
        }

        [HttpPost]
        public ActionResult AsignarLimpieza(Limpieza model)
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");
                ViewBag.Habitaciones = new SelectList(context.tHabitaciones.ToList(), "IdHabitacion", "NombreHabitacion");
                try
                {
                    var nuevaLimpieza = new tLimpiezas
                    {
                        IdLimpieza = model.IdLimpieza,
                        IdEmpleado = model.IdEmpleado,
                        IdHabitacion = model.IdHabitacion,
                        FechaLimpieza = model.FechaLimpieza,
                        Estado = model.Estado
                    };

                    context.tLimpiezas.Add(nuevaLimpieza);
                    context.SaveChanges();

                    return RedirectToAction("ControlDeLimpieza");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return RedirectToAction("ControlDeLimpieza");
                }
            }
        }

        [HttpPost]
        public ActionResult ConfirmarLimpieza(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                    var limpieza = context.tLimpiezas.FirstOrDefault(l => l.IdLimpieza == id);

                    limpieza.Estado = true;
                    context.SaveChanges();

                    return RedirectToAction("ControlDeLimpieza");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return RedirectToAction("ControlDeLimpieza");
                }
            }
        }

        public ActionResult GenerarReportePDF(bool incluirDetalles = true)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    // Si se solicita un reporte detallado
                    if (incluirDetalles)
                    {
                        var limpiezas = context.tLimpiezas.ToList();
                        var empleados = context.tEmpleados.ToDictionary(e => e.ConsecutivoEmp, e => e.Nombre);
                        var habitaciones = context.tHabitaciones.ToDictionary(h => h.IdHabitacion, h => h.NombreHabitacion);

                        MemoryStream workStream = new MemoryStream();
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter.GetInstance(document, workStream).CloseStream = false;

                        document.Open();
                        Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                        Font textoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                        document.Add(new Paragraph("Reporte de Control de Limpieza", tituloFont));
                        document.Add(new Paragraph($"Fecha de Generación: {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n\n", textoFont));

                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 25f, 25f, 25f, 25f });

                        table.AddCell(new PdfPCell(new Phrase("Empleado", tituloFont)));
                        table.AddCell(new PdfPCell(new Phrase("Fecha de Limpieza", tituloFont)));
                        table.AddCell(new PdfPCell(new Phrase("Habitación", tituloFont)));
                        table.AddCell(new PdfPCell(new Phrase("Estado", tituloFont)));

                        foreach (var l in limpiezas)
                        {
                            table.AddCell(new PdfPCell(new Phrase(empleados[l.IdEmpleado], textoFont)));
                            table.AddCell(new PdfPCell(new Phrase(l.FechaLimpieza.ToString("dd/MM/yyyy"), textoFont)));
                            table.AddCell(new PdfPCell(new Phrase(habitaciones[l.IdHabitacion], textoFont)));
                            table.AddCell(new PdfPCell(new Phrase(l.Estado ? "Finalizada" : "Pendiente", textoFont)));
                        }

                        document.Add(table);
                        document.Close();

                        byte[] byteInfo = workStream.ToArray();
                        workStream.Write(byteInfo, 0, byteInfo.Length);
                        workStream.Position = 0;

                        // Establecer mensaje de éxito en TempData
                        TempData["Message"] = "Se descargó exitosamente el reporte.";

                        return File(workStream, "application/pdf", "Reporte_Limpieza.pdf");
                    }
                    // Si solo se quiere un reporte básico
                    else
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Document document = new Document();
                            PdfWriter writer = PdfWriter.GetInstance(document, ms);
                            document.Open();

                            document.Add(new Paragraph("Reporte de Limpieza"));
                            document.Add(new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm")));

                            document.Close();
                            writer.Close();

                            // Establecer mensaje de éxito en TempData
                            TempData["Message"] = "Se descargó exitosamente el reporte.";

                            return File(ms.ToArray(), "application/pdf", "Reporte_Limpieza.pdf");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = $"Error al generar el PDF: {ex.Message}";
                return RedirectToAction("ControlDeLimpieza");
            }
        }

    }
}