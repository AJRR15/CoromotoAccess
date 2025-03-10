using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class RecursosHumanosController : Controller
    {
        // GET: RecursosHumanos
        public ActionResult GestiondeRRHH()
        {
            return View();
        }
        public ActionResult GestionarVacaciones()
        {
            return View();
        }
        public ActionResult AutoGestion()
        {
            return View();
        }
        public ActionResult ConsultarBoletasPago()
        {
            return View();
        }
        public ActionResult AprobarVacaciones()
        {
            return View();
        }
        public ActionResult GestionNomina()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EvaluacionEmpleados()
        {
            using (var context = new BDCoromotoEntities())
            {
                var empleados = context.tEmpleados.Select(e => new Empleado
                {
                    ConsecutivoEmp = e.ConsecutivoEmp,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    IdRol = e.ConsecutivoRol,
                    HorasTrabajadas = e.HorasTrabajadas
                }).ToList();

                ViewBag.Comentarios = context.tEvaluaciones.ToDictionary(c => c.IdEmpleado, c => c.Comentario);
                ViewBag.Calificaciones = context.tEvaluaciones.ToDictionary(c => c.IdEmpleado, c => c.Calificacion);
                ViewBag.DiccionarioRoles = context.tRoles.ToDictionary(r => r.IdRol, r => r.NombreRol);
                ViewBag.DiccionarioCalificaciones = new Dictionary<int, string> { { 0, "Sin calificar" }, { 1, "Pobre" }, { 2, "Regular" }, { 3, "Bueno" }, { 4, "Excelente" } };
                ViewBag.OpcionesCalificacion = new SelectList(ViewBag.DiccionarioCalificaciones, "Key", "Value");

                ViewBag.TargetEmpleado = -1;

                return View(empleados);
            }
        }

        [HttpPost]
        public ActionResult AsignarEvaluacion(Evaluacion model)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                    var evaluacion = context.tEvaluaciones.FirstOrDefault(e => e.IdEmpleado == model.IdEmpleado);

                    if (evaluacion != null)
                    {
                        evaluacion.Calificacion = model.Calificacion;
                        evaluacion.Comentario = model.Comentario;
                        evaluacion.FechaEvaluacion = DateTime.Now;
                    }
                    else
                    {
                        var nuevaEvaluacion = new tEvaluaciones
                        {
                            IdEmpleado = model.IdEmpleado,
                            Comentario = model.Comentario,
                            Calificacion = model.Calificacion,
                            FechaEvaluacion = DateTime.Now
                        };

                        context.tEvaluaciones.Add(nuevaEvaluacion);
                    }

                    context.SaveChanges();

                    return RedirectToAction("EvaluacionEmpleados");
                }
                catch (Exception ex)
                {
                    ViewBag.MensajePantalla = $"Error: {ex.Message}";
                    return RedirectToAction("EvaluacionEmpleados");
                }
            }
        }
        public ActionResult BoletasPago()
        {
            return View();
        }
        public ActionResult HistoricoSalarios()
        {
            return View();
        }
    }


    }
