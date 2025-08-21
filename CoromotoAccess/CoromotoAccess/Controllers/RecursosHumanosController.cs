using CoromotoAccess.Filters;
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
        [AuthRequired]
        // GET: RecursosHumanos
        public ActionResult GestiondeRRHH()
        {
            return View();
        }
        public ActionResult GestionarVacaciones()
        {
            using (var context = new BDCoromotoEntities())
            {
                var vacaciones = context.tVacaciones.Select(e => new Vacaciones
                {
                    IdVacacion = e.IdVacacion,
                    IdEmpleado = e.IdEmpleado,
                    DiasSolicitados = e.DiasSolicitados,
                    FechaInicio = e.fechaInicio,
                    FechaFin = e.fechaFin,
                    Estado = e.Estado
                }).ToList();

                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");
                ViewBag.DiccionarioEmpleados = context.tEmpleados.ToDictionary(e => e.ConsecutivoEmp, e => e);

                return View(vacaciones);
            }
        }

        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult CrearVacaciones(Vacaciones model)
        {
            using (var context = new BDCoromotoEntities())
            {
                if (ModelState.IsValid)
                {
                    var nuevaVacacion = new tVacaciones
                    {
                        IdVacacion = model.IdVacacion,
                        IdEmpleado = model.IdEmpleado,
                        DiasSolicitados = (model.FechaFin - model.FechaInicio).TotalDays.ToString(),
                        fechaInicio = model.FechaInicio,
                        fechaFin = model.FechaFin,
                        Estado = model.Estado
                    };

                    context.tVacaciones.Add(nuevaVacacion);
                    context.SaveChanges();

                    return RedirectToAction("GestionarVacaciones");
                }

                return RedirectToAction("GestionarVacaciones");
            }
        }
        [HttpGet]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult AprobarVacaciones()
        {
            using (var context = new BDCoromotoEntities())
            {
                var vacaciones = context.tVacaciones.Select(e => new Vacaciones
                {
                    IdVacacion = e.IdVacacion,
                    IdEmpleado = e.IdEmpleado,
                    DiasSolicitados = e.DiasSolicitados,
                    FechaInicio = e.fechaInicio,
                    FechaFin = e.fechaFin,
                    Estado = e.Estado
                }).ToList();

                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");
                ViewBag.DiccionarioEmpleados = context.tEmpleados.ToDictionary(e => e.ConsecutivoEmp, e => e);

                return View(vacaciones);
            }
        }

        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult AprobarVacaciones(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var vacacion = context.tVacaciones.Find(id);

                if (vacacion != null)
                {
                    vacacion.Estado = 1;
                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Vacaciones aprobadas.";
                }

                return RedirectToAction("GestionarVacaciones");
            }
        }

        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult RechazarVacaciones(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var vacacion = context.tVacaciones.Find(id);

                if (vacacion != null)
                {
                    vacacion.Estado = 2;
                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Vacaciones rechazadas.";
                }

                return RedirectToAction("GestionarVacaciones");
            }
        }
        public ActionResult AutoGestion()
        {
            return View();
        }
        public ActionResult ConsultarBoletasPago()
        {
            return View();
        }
        [HttpGet]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult GestionNomina()
        {
            using (var context = new BDCoromotoEntities())
            {
                var nominas = context.tNominas.Select(e => new Nomina
                {
                    IdNomina = e.IdNomina,
                    IdEmpleado = e.IdEmpleado,
                    Salario = e.Salario,
                    Bono = e.Bono,
                    Multa = e.Multa,
                    Total = e.Total
                }).ToList();

                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");
                ViewBag.DiccionarioEmpleados = context.tEmpleados.ToDictionary(e => e.ConsecutivoEmp, e => e);
                ViewBag.DiccionarioRoles = context.tRoles.ToDictionary(r => r.IdRol, r => r.NombreRol);

                return View(nominas);
            }
        }

        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult CrearNomina(Nomina model)
        {
            using (var context = new BDCoromotoEntities())
            {
                if (ModelState.IsValid)
                {
                    var nuevaNomina = new tNominas
                    {
                        IdNomina = model.IdNomina,
                        IdEmpleado = model.IdEmpleado,
                        Salario = model.Salario,
                        Bono = model.Bono,
                        Multa = model.Multa,
                        Total = model.Total
                    };

                    context.tNominas.Add(nuevaNomina);
                    context.SaveChanges();

                    return RedirectToAction("GestionNomina");
                }

                return RedirectToAction("GestionNomina");
            }
        }

        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult ModificarNomina(Nomina model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var nomina = context.tNominas.Find(model.IdNomina);

                if (nomina != null)
                {
                    nomina.Salario = model.Salario;
                    nomina.Bono = model.Bono;
                    nomina.Multa = model.Multa;

                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Nomina modificada correctamente.";
                }

                return RedirectToAction("GestionNomina");
            }
        }

        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult EliminarNomina(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var nomina = context.tNominas.Find(id);

                if (nomina != null)
                {
                    context.tNominas.Remove(nomina);
                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Nomina eliminada correctamente.";
                }

                return RedirectToAction("GestionNomina");
            }
        }
        [HttpGet]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult EvaluacionEmpleados()
        {
            using (var context = new BDCoromotoEntities())
            {
                var evaluaciones = context.tEvaluaciones
                    .Join(context.tEmpleados,
                        e => e.IdEmpleado,
                        emp => emp.ConsecutivoEmp,
                        (e, emp) => new { Evaluacion = e, Empleado = emp })
                    .Join(context.tRoles,
                        combined => combined.Empleado.ConsecutivoRol,
                        r => r.IdRol,
                        (combined, r) => new Evaluacion
                        {
                            IdEvaluacion = combined.Evaluacion.IdEvaluacion,
                            IdEmpleado = combined.Evaluacion.IdEmpleado,
                            Comentario = combined.Evaluacion.Comentario,
                            Calificacion = combined.Evaluacion.Calificacion,
                            FechaEvaluacion = combined.Evaluacion.FechaEvaluacion,
                            NombreEmpleado = combined.Empleado.Nombre,
                            ApellidoEmpleado = combined.Empleado.Apellido,
                            RolEmpleado = r.NombreRol,
                            HorasTrabajadas = combined.Empleado.HorasTrabajadas
                        })
                    .OrderByDescending(e => e.FechaEvaluacion)
                    .ToList();

                ViewBag.DiccionarioCalificaciones = new Dictionary<int, string> {
            { 1, "Pobre" },
            { 2, "Regular" },
            { 3, "Bueno" },
            { 4, "Excelente" }
        };

                ViewBag.OpcionesCalificacion = new SelectList(ViewBag.DiccionarioCalificaciones, "Key", "Value");
                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");

                return View(evaluaciones);
            }
        }

        [HttpPost]
        [AuthRequired(Roles = "Administrador")]
        public ActionResult AsignarEvaluacion(Evaluacion model)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                        var nuevaEvaluacion = new tEvaluaciones
                        {
                            IdEmpleado = model.IdEmpleado,
                            Comentario = model.Comentario,
                            Calificacion = model.Calificacion,
                            FechaEvaluacion = DateTime.Now
                        };

                        context.tEvaluaciones.Add(nuevaEvaluacion);
                    

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
