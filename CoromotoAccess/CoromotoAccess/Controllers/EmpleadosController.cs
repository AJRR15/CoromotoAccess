using CoromotoAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class EmpleadosController : Controller
    {
        // GET: Empleados
        [HttpGet]
        public ActionResult GestionEmpleados()
        {
            using (var context = new BDCoromotoEntities())
            {
                var empleados = context.tEmpleados.Select(e => new Empleado
                {
                    ConsecutivoEmp = e.ConsecutivoEmp,
                    Identificacion = e.Identificacion,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    CorreoElectronico = e.CorreoElectronico,
                    Telefono = e.Telefono,
                    IdRol = e.ConsecutivoRol,
                    IdVilla = e.IdVilla,
                    IdTurno = e.IdTurno,
                    Activo = e.Activo
                }).ToList();
                ViewBag.Roles = new SelectList(context.tRoles.ToList(), "IdRol", "NombreRol");
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.Turnos = new SelectList(context.tTurnos.ToList(), "IdTurno", "Nombre");
                ViewBag.MensajePantalla = TempData["MensajePantalla"];
                return View(empleados);
            }
        }

        public ActionResult EmpleadosMain()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ModificarEmpleado(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var model = context.tEmpleados.FirstOrDefault(e => e.ConsecutivoEmp == id);

                if (model == null)
                {
                    TempData["MensajePantalla"] = "No se encontró al empleado.";
                    return RedirectToAction("GestionEmpleados");
                }
                ViewBag.Roles = new SelectList(context.tRoles.ToList(), "IdRol", "NombreRol");
                ViewBag.Villas = new SelectList(context.tVillas.ToList(), "IdVilla", "NombreHabitacion");
                ViewBag.Turnos = new SelectList(context.tTurnos.ToList(), "IdTurno", "Nombre");
                var empleado = new Empleado()
                {
                    ConsecutivoEmp = model.ConsecutivoEmp,
                    Identificacion = model.Identificacion,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    CorreoElectronico = model.CorreoElectronico,
                    Telefono = model.Telefono,
                    IdRol = model.ConsecutivoRol,
                    IdVilla = model.IdVilla,
                    IdTurno = model.IdTurno,
                    Vacaciones = model.Vacaciones,
                    HorasTrabajadas = model.HorasTrabajadas

                };

                return View(empleado);
            }
        }

        [HttpPost]
        public ActionResult ModificarEmpleado(Empleado model)
        {
            using (var context = new BDCoromotoEntities())
            {
                try
                {
                    var empleado = context.tEmpleados.FirstOrDefault(e => e.ConsecutivoEmp == model.ConsecutivoEmp);

                    if (empleado == null)
                    {
                        TempData["MensajePantalla"] = "Empleado no encontrado.";
                        return RedirectToAction("GestionEmpleados");
                    }

                    // Validar duplicados
                    if (context.tEmpleados.Any(e => e.Identificacion == model.Identificacion && e.ConsecutivoEmp != model.ConsecutivoEmp))
                    {
                        TempData["MensajePantalla"] = "La identificación ya está en uso.";
                        return RedirectToAction("GestionEmpleados");
                    }

                    // Actualizar propiedades
                    empleado.CorreoElectronico = model.CorreoElectronico;
                    empleado.Telefono = model.Telefono;
                    empleado.ConsecutivoRol = model.IdRol;
                    empleado.IdVilla = model.IdVilla;
                    empleado.IdTurno = model.IdTurno;

                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Empleado actualizado correctamente.";
                    return RedirectToAction("GestionEmpleados");
                }
                catch (DbEntityValidationException ex)
                {
                    // Capturar errores de validación
                    var errores = ex.EntityValidationErrors
                        .SelectMany(e => e.ValidationErrors)
                        .Select(v => $"{v.PropertyName}: {v.ErrorMessage}");

                    TempData["MensajePantalla"] = "Error de validación: " + string.Join("; ", errores);
                    return RedirectToAction("GestionEmpleados");
                }
            }
        }

        [HttpGet]
        public ActionResult AgregarEmpleado()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarEmpleado(Empleado model)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensajePantalla"] = "Datos inválidos.";
                return RedirectToAction("GestionEmpleados");
            }

            using (var context = new BDCoromotoEntities())
            {
                // Validar duplicados
                if (context.tEmpleados.Any(e => e.Identificacion == model.Identificacion))
                {
                    TempData["MensajePantalla"] = "La identificación ya está registrada.";
                    return RedirectToAction("GestionEmpleados");
                }

                var nuevoEmpleado = new tEmpleados
                {
                    Identificacion = model.Identificacion,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    CorreoElectronico = model.CorreoElectronico,
                    Telefono = model.Telefono,
                    FotoPerfil = "default.jpg",
                    Contrasenna = "Temp123",
                    ConsecutivoRol = model.IdRol,
                    IdVilla = model.IdVilla,
                    IdTurno = model.IdTurno,
                    Activo = true,
                    TieneContrasennaTemp = true,
                    FechaVencimientoTemp = DateTime.Now.AddDays(30),
                    Vacaciones = 0,
                    HorasTrabajadas = 0
                };

                context.tEmpleados.Add(nuevoEmpleado);
                context.SaveChanges();

                TempData["MensajePantalla"] = "Empleado registrado exitosamente.";
                return RedirectToAction("GestionEmpleados");
            }
        }

        [HttpGet]
        public ActionResult EliminarEmpleado(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var empleado = context.tEmpleados.FirstOrDefault(e => e.ConsecutivoEmp == id);

                if (empleado != null)
                {
                    context.tEmpleados.Remove(empleado);
                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Empleado eliminado correctamente.";
                }
                else
                {
                    TempData["MensajePantalla"] = "Empleado no encontrado.";
                }

                return RedirectToAction("GestionEmpleados");
            }
        }


        [HttpGet]
        public ActionResult GestionTareas()
        {
            using (var context = new BDCoromotoEntities())
            {
                var tareas = context.tTareas.Select(t => new Tarea
                {
                    IdTarea = t.IdTarea,
                    Descripcion = t.Descripcion,
                    Estado = t.Estado,
                    IdEmpleado = t.IdEmpleado // Asegúrate de incluir este campo
                }).ToList();

                // Cargar la lista de empleados en ViewBag
                ViewBag.Empleados = new SelectList(context.tEmpleados.ToList(), "ConsecutivoEmp", "Nombre");

                ViewBag.MensajePantalla = TempData["MensajePantalla"];
                return View(tareas);
            }
        }




        [HttpPost]
        public ActionResult ModificarTarea(Tarea model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var tarea = context.tTareas.Find(model.IdTarea);

                if (tarea != null)
                {
                    tarea.Descripcion = model.Descripcion;
                    tarea.Estado = model.Estado;
                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Tarea actualizada correctamente.";
                }

                return RedirectToAction("GestionTareas", new { id = model.IdEmpleado });
            }
        }

        [HttpGet]
        public ActionResult EliminarTarea(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var tarea = context.tTareas.Find(id);

                if (tarea != null)
                {
                    context.tTareas.Remove(tarea);
                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Tarea eliminada correctamente.";
                }

                return RedirectToAction("GestionTareas", new { id = tarea.IdEmpleado });
            }
        }

        [HttpGet]
        public ActionResult AgregarTarea(int ConsecutivoEmp)
        {
            ViewBag.IdEmpleado = ConsecutivoEmp;
            return View();
        }


        [HttpPost]
        public ActionResult AgregarTarea(Tarea model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var empleadoExiste = context.tEmpleados.Any(e => e.ConsecutivoEmp == model.IdEmpleado);
                if (!empleadoExiste)
                {
                    ModelState.AddModelError("", "El empleado no existe.");
                    // Devolver la vista original con el modelo y mensaje de error
                    return View(model);
                }

                var nuevaTarea = new tTareas
                {
                    Descripcion = model.Descripcion,
                    Estado = model.Estado,
                    IdEmpleado = model.IdEmpleado
                };

                context.tTareas.Add(nuevaTarea);
                context.SaveChanges();

                TempData["MensajePantalla"] = "Tarea registrada exitosamente.";
                return RedirectToAction("GestionEmpleados");
            }
        }


        public ActionResult EmpleadoTurno()
        {
            using (var context = new BDCoromotoEntities())
            {
                var turnos = context.tTurnos.ToList();
                return View(turnos);
            }
        }

        [HttpGet]
        public ActionResult CrearTurno()
        {
            using (var context = new BDCoromotoEntities())
            {
                var turnos = context.tTurnos.Select(t => new Turnos
                {
                    IdTurno = t.IdTurno,
                    Nombre = t.Nombre,
                    HoraInicio = t.HoraInicio,
                    HoraSalida = t.HoraSalida
                }).ToList();

                ViewBag.Empleados = context.tEmpleados.ToList();
                return View(turnos);
            }
        }

        [HttpPost]
        public ActionResult CrearTurno(Turnos turno)
        {
            using (var context = new BDCoromotoEntities())
            {
                if (ModelState.IsValid)
                {
                    var nuevoTurno = new tTurnos
                    {
                        IdTurno = turno.IdTurno,
                        Nombre = turno.Nombre,
                        HoraInicio = turno.HoraInicio,
                        HoraSalida = turno.HoraSalida
                    };

                    context.tTurnos.Add(nuevoTurno);
                    context.SaveChanges();
                    return RedirectToAction("CrearTurno");
                }
                return RedirectToAction("CrearTurno");
            }
        }

        public ActionResult AsignarTurno()
        {
            using (var context = new BDCoromotoEntities())
            {
                var empleados = context.tEmpleados.Select(e => new Empleado
                {
                    ConsecutivoEmp = e.ConsecutivoEmp,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    IdTurno = e.IdTurno,
                    Activo = e.Activo
                }).ToList();

                ViewBag.Turnos = new SelectList(context.tTurnos.ToList(), "IdTurno", "Nombre");
                ViewBag.DiccionarioTurnos = context.tTurnos.ToDictionary(t => t.IdTurno, t => t.Nombre);

                return View(empleados);
            }
        }

        [HttpPost]
        public ActionResult AsignarTurno(Empleado model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var empleado = context.tEmpleados.Find(model.ConsecutivoEmp);

                if (empleado != null)
                {
                    empleado.IdTurno = model.IdTurno;
                    empleado.Activo = true;

                    context.SaveChanges();

                    TempData["MensajePantalla"] = "Turno asignado correctamente.";
                }

                return RedirectToAction("AsignarTurno");
            }
        }

        [HttpPost]
        public ActionResult DesasignarTurno(Empleado model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var empleado = context.tEmpleados.Find(model.ConsecutivoEmp);

                if (empleado != null)
                {
                    empleado.Activo = false;

                    context.SaveChanges();

                    TempData["MensajePantalla"] = "Turno desasignado correctamente.";
                }

                return RedirectToAction("AsignarTurno");
            }
        }

        [HttpPost]
        public ActionResult EliminarTurno(int id)
        {
            using (var context = new BDCoromotoEntities())
            {
                var turno = context.tTurnos.Find(id);

                if (turno != null)
                {
                    context.tTurnos.Remove(turno);
                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Turno eliminado correctamente.";
                }

                return RedirectToAction("CrearTurno");
            }
        }

        [HttpPost]
        public ActionResult ModificarTurno(Turnos model)
        {
            using (var context = new BDCoromotoEntities())
            {
                var turno = context.tTurnos.Find(model.IdTurno);

                if (turno != null)
                {
                    turno.Nombre = model.Nombre;
                    turno.HoraInicio = model.HoraInicio;
                    turno.HoraSalida = model.HoraSalida;

                    context.SaveChanges();
                    TempData["MensajePantalla"] = "Turno modificado correctamente.";
                }

                return RedirectToAction("CrearTurno");
            }
        }

    }
}