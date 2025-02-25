using CoromotoAccess.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Xml.Linq;


namespace CoromotoAccess.Controllers
{
    public class ReservaController : Controller
    {
        [HttpGet]
        public ActionResult AdministrarReservas()
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tReservas.ToList();
                var usuarios = context.tUsuario.Select(u => new Usuario
                {
                    ConsecutivoCliente = u.ConsecutivoCliente,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Telefono = u.Telefono
                }).ToList();
                var habitaciones = context.tHabitaciones.Select(h => new Habitacion
                {
                    IdHabitacion = h.IdHabitacion,
                    NombreHabitacion = h.NombreHabitacion
                }).ToList();

                ViewBag.Usuarios = usuarios;
                ViewBag.Habitaciones = habitaciones;

                var listaReservas = datos.Select(r => new Reserva
                {
                    IdReserva = r.IdReserva,
                    IdUsuario = r.IdUsuario,
                    IdHabitacion = r.IdHabitacion,
                    CheckIn = r.CheckIn,
                    CheckOut = r.CheckOut,
                    Estado = r.Estado,
                    IdMoneda = r.IdMoneda,
                    IdMetodoP = r.IdMetodoP
                }).ToList();

                return View(listaReservas);
            }
        }
        /*
        [HttpPost]
        public ActionResult AgregarReserva(Reserva model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var idUsuario = int.Parse(Session["Consecutivo"].ToString());

                var habitacion = context.tHabitaciones.Find(model.IdHabitacion);
                if (habitacion == null)
                {
                    return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "La habitación seleccionada no existe." });
                }

                var usuario = context.tUsuario.Find(idUsuario);
                if (usuario == null)
                {
                    return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "El usuario seleccionado no existe." });
                }

                var reservasExistentes = context.tReservas.Where(r => r.IdHabitacion == model.IdHabitacion && ((model.CheckIn >= r.CheckIn && model.CheckIn <= r.CheckOut) || (model.CheckOut >= r.CheckIn && model.CheckOut <= r.CheckOut))).ToList();

                if (reservasExistentes.Any())
                {
                    return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "La habitación ya está reservada en las fechas seleccionadas." });
                }

                var Estado = true;
                var reserva = new tReservas
                {
                    IdUsuario = idUsuario,
                    IdHabitacion = model.IdHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Comentario = model.Comentario,
                    PersonasHospedados = model.PersonasHospedados,
                    Estado = Estado,
                    IdMoneda = model.IdMoneda,
                    IdMetodoP = model.IdMetodoP,
                };

                context.tReservas.Add(reserva);
                context.SaveChanges();

                return RedirectToAction("CatalogoHabitaciones", "Habitacion");
            }
        }
        */
        private void CargarListas()
        {
            using (var context = new BDCoromotoEntities())
            {
                ViewBag.MetodosPago = new SelectList(context.tMetodoPago.ToList(), "IdMetodoP", "NombreMetodoP");
                ViewBag.Monedas = new SelectList(context.tMonedas.ToList(), "IdMoneda", "NombreMoneda");
            }
        }

        [HttpGet]
        public ActionResult ModificarReserva(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tReservas.Where(x => x.IdReserva == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró la reserva.";
                    return RedirectToAction("AdministrarReservas", "Reserva");
                }

                var reserva = new Reserva()
                {
                    IdReserva = model.IdReserva,
                    IdUsuario = model.IdUsuario,
                    IdHabitacion = model.IdHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Estado = model.Estado,
                    IdMoneda = model.IdMoneda,
                    IdMetodoP = model.IdMetodoP
                };

                CargarListas();
                return View(reserva);
            }
        }

        [HttpPost]
        public ActionResult ModificarReserva(Reserva model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var datos = context.tReservas.Where(x => x.IdReserva == model.IdReserva).FirstOrDefault();
                if (datos != null)
                {
                    datos.CheckIn = model.CheckIn;
                    datos.CheckOut = model.CheckOut;
                    datos.Estado = model.Estado;
                    datos.IdMoneda = model.IdMoneda;
                    datos.IdMetodoP = model.IdMetodoP;

                    context.SaveChanges();
                    return RedirectToAction("AdministrarReservas", "Reserva");
                }

                ViewBag.MensajePantalla = "La información no se ha podido actualizar correctamente";
            }
            CargarListas();
            return View(model);
        }


        [HttpPost]
        public ActionResult CancelarReserva(long Id)
        {
            try
            {
                using (var context = new BDCoromotoEntities())
                {
                    var reserva = context.tReservas.Find(Id);

                    if (reserva == null)
                    {
                        TempData["Mensaje"] = "La reserva no existe.";
                        return RedirectToAction("AdministrarReservas");
                    }

                    reserva.Estado = false;
                    context.SaveChanges();

                    TempData["Mensaje"] = "Reserva cancelada exitosamente.";
                    return RedirectToAction("AdministrarReservas");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al cancelar la reserva.";
                return RedirectToAction("AdministrarReservas");
            }
        }

        [HttpGet]
        public ActionResult DetallesReserva(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tReservas.Where(x => x.IdReserva == id).FirstOrDefault();

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró la reserva.";
                    return RedirectToAction("AdministrarReservas");
                }

                var moneda = context.tMonedas.FirstOrDefault(m => m.IdMoneda == model.IdMoneda);
                var metodoPago = context.tMetodoPago.FirstOrDefault(mp => mp.IdMetodoP == model.IdMetodoP);
                var usuario = context.tUsuario.FirstOrDefault(u => u.ConsecutivoCliente == model.IdUsuario);
                var habitacion = context.tHabitaciones.FirstOrDefault(h => h.IdHabitacion == model.IdHabitacion);

                var reserva = new Reserva()
                {
                    IdReserva = model.IdReserva,
                    IdUsuario = model.IdUsuario,
                    NombreUsuario = usuario?.Nombre + " " + usuario?.Apellido,
                    IdHabitacion = model.IdHabitacion,
                    NombreHabitacion = habitacion?.NombreHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Estado = model.Estado,
                    IdMoneda = model.IdMoneda,
                    NombreMoneda = moneda?.NombreMoneda,
                    IdMetodoP = model.IdMetodoP,
                    NombreMetodoPago = metodoPago?.NombreMetodoP,
                };

                return View(reserva);
            }
        }

        [HttpGet]
        public ActionResult MisReservas(long? idUsuario)
        {
            if (idUsuario == null)
            {
                ViewBag.MensajePantalla = "Por favor, inicie sesión.";
                return RedirectToAction("Index", "Home");
            }

            using (var context = new BDCoromotoEntities())
            {
                var reservas = context.tReservas
                    .Where(r => r.IdUsuario == idUsuario)
                    .Select(r => new Reserva
                    {
                        IdReserva = r.IdReserva,
                        IdUsuario = r.IdUsuario,
                        IdHabitacion = r.IdHabitacion,
                        CheckIn = r.CheckIn,
                        CheckOut = r.CheckOut,
                        Estado = r.Estado,
                        IdMoneda = r.IdMoneda,
                        IdMetodoP = r.IdMetodoP
                    }).ToList();

                if (!reservas.Any())
                {
                    ViewBag.MensajePantalla = "No se encontraron reservaciones.";
                    return View(reservas); // Se pasa un modelo vacío con el mensaje de error
                }

                var usuario = context.tUsuario.FirstOrDefault(u => u.ConsecutivoCliente == idUsuario);
                ViewBag.Usuario = usuario?.Nombre + " " + usuario?.Apellido;

                return View(reservas);
            }
        }


        [HttpGet]
        public ActionResult DescargarReserva(long id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tReservas.FirstOrDefault(x => x.IdReserva == id);

                if (model == null)
                {
                    ViewBag.MensajePantalla = "No se encontró la reserva.";
                    return RedirectToAction("MisReservas", new { idUsuario = Session["Consecutivo"] });
                }

                var moneda = context.tMonedas.FirstOrDefault(m => m.IdMoneda == model.IdMoneda);
                var metodoPago = context.tMetodoPago.FirstOrDefault(mp => mp.IdMetodoP == model.IdMetodoP);
                var usuario = context.tUsuario.FirstOrDefault(u => u.ConsecutivoCliente == model.IdUsuario);
                var habitacion = context.tHabitaciones.FirstOrDefault(h => h.IdHabitacion == model.IdHabitacion);

                var reserva = new Reserva()
                {
                    IdReserva = model.IdReserva,
                    IdUsuario = model.IdUsuario,
                    NombreUsuario = usuario?.Nombre + " " + usuario?.Apellido,
                    IdHabitacion = model.IdHabitacion,
                    NombreHabitacion = habitacion?.NombreHabitacion,
                    CheckIn = model.CheckIn,
                    CheckOut = model.CheckOut,
                    Estado = model.Estado,
                    IdMoneda = model.IdMoneda,
                    NombreMoneda = moneda?.NombreMoneda,
                    IdMetodoP = model.IdMetodoP,
                    NombreMetodoPago = metodoPago?.NombreMetodoP
                };

                byte[] pdfContent = GenerarPDFReserva(reserva);
                return File(pdfContent, "application/pdf", $"Reserva_{reserva.IdReserva}.pdf");
            }
        }
        private byte[] GenerarPDFReserva(Reserva reserva)
        {
            using (var memoryStream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph("Detalles de la Reserva"));
                document.Add(new Paragraph(" ")); 

                document.Add(new Paragraph($"ID Reserva: {reserva.IdReserva}"));
                document.Add(new Paragraph($"Usuario: {reserva.NombreUsuario}"));
                document.Add(new Paragraph($"Habitación: {reserva.NombreHabitacion}"));
                document.Add(new Paragraph($"CheckIn: {reserva.CheckIn:dd/MM/yyyy HH:mm}"));
                document.Add(new Paragraph($"CheckOut: {reserva.CheckOut:dd/MM/yyyy HH:mm}"));
                document.Add(new Paragraph($"Estado: {(reserva.Estado ? "Confirmada" : "Cancelada")}"));
                document.Add(new Paragraph($"Moneda: {reserva.NombreMoneda}"));
                document.Add(new Paragraph($"Método de Pago: {reserva.NombreMetodoPago}"));

                document.Close();
                writer.Close();
                return memoryStream.ToArray();
            }
        }


        //Metodos a implementar proximamente:

        
         [HttpPost]
public ActionResult AgregarReserva(Reserva model)
{
    using (var context = new BDCoromotoEntities())
    {
        var idUsuario = int.Parse(Session["Consecutivo"].ToString());

        var habitacion = context.tHabitaciones.Find(model.IdHabitacion);
        if (habitacion == null)
        {
            return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "La habitación seleccionada no existe." });
        }

        var usuario = context.tUsuario.Find(idUsuario);
        if (usuario == null)
        {
            return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "El usuario seleccionado no existe." });
        }

        var reservasExistentes = context.tReservas.Where(r => r.IdHabitacion == model.IdHabitacion && ((model.CheckIn >= r.CheckIn && model.CheckIn <= r.CheckOut) || (model.CheckOut >= r.CheckIn && model.CheckOut <= r.CheckOut))).ToList();

        if (reservasExistentes.Any())
        {
            return RedirectToAction("DatosHabitacion", "Habitacion", new { id = model.IdHabitacion, errorMessage = "La habitación ya está reservada en las fechas seleccionadas." });
        }

        var Estado = true;
        var reserva = new tReservas
        {
            IdUsuario = idUsuario,
            IdHabitacion = model.IdHabitacion,
            CheckIn = model.CheckIn,
            CheckOut = model.CheckOut,
           // Comentario = model.Comentario,
           // PersonasHospedados = model.PersonasHospedados,
            Estado = Estado,
            IdMoneda = model.IdMoneda,
            IdMetodoP = model.IdMetodoP,
        };

        context.tReservas.Add(reserva);
        context.SaveChanges();

        // Enviar correo de confirmación
        string asunto = "Confirmación de Reserva";
        string contenido = GenerarContenidoCorreo(usuario.Nombre, reserva, habitacion.NombreHabitacion);

        EnviarCorreo(usuario.CorreoElectronico, asunto, contenido);

        return RedirectToAction("CatalogoHabitaciones", "Habitacion");
    }
}


        private string GenerarContenidoCorreo(string nombre, tReservas reserva, string nombreHabitacion)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\Styles\\TemplateCorreoProyecto.html";
            string contenido = System.IO.File.ReadAllText(ruta);

            contenido = contenido.Replace("@@Nombre", nombre);
            contenido = contenido.Replace("@@IdReserva", reserva.IdReserva.ToString());
            contenido = contenido.Replace("@@NombreHabitacion", nombreHabitacion);
            contenido = contenido.Replace("@@CheckIn", reserva.CheckIn.ToString("dd/MM/yyyy HH:mm"));
            contenido = contenido.Replace("@@CheckOut", reserva.CheckOut.ToString("dd/MM/yyyy HH:mm"));
           // contenido = contenido.Replace("@@PersonasHospedadas", reserva.PersonasHospedados.ToString());

            return contenido;
        }

        private void EnviarCorreo(string destino, string asunto, string contenido)
        {
            string cuenta = "julloa60694@ufide.ac.cr";
            string contrasenna = "#1DEGT89";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(cuenta);
            message.To.Add(new MailAddress(destino));
            message.Subject = asunto;
            message.Body = contenido;
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.Credentials = new System.Net.NetworkCredential(cuenta, contrasenna);
            client.EnableSsl = true;
            client.Send(message);
        }

        /*
        [HttpPost]
        public ActionResult ReenviarConfirmacion(long idReserva)
        {
            using (var context = new BDCoromotoEntities())
            {
                var reserva = context.tReservas.Find(idReserva);
                if (reserva == null)
                {
                    ViewBag.MensajePantalla = "La reserva no existe.";
                    return RedirectToAction("MisReservas", new { idUsuario = Session["Consecutivo"] });
                }

                var usuario = context.tUsuario.Find(reserva.IdUsuario);
                if (usuario == null)
                {
                    ViewBag.MensajePantalla = "El usuario no existe.";
                    return RedirectToAction("MisReservas", new { idUsuario = Session["Consecutivo"] });
                }

                var habitacion = context.tHabitaciones.FirstOrDefault(h => h.IdHabitacion == reserva.IdHabitacion);
                string asunto = "Reenvío de Confirmación de Reserva";
                string contenido = GenerarContenidoCorreo(usuario.Nombre, reserva, habitacion.NombreHabitacion);

                EnviarCorreo(usuario.CorreoElectronico, asunto, contenido);

                ViewBag.MensajePantalla = "La confirmación ha sido reenviada.";
                return RedirectToAction("MisReservas", new { idUsuario = Session["Consecutivo"] });
            }

        */
        


        }
    }