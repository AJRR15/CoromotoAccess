using CoromotoAccess.Models;
using System.Linq;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class PerfilController : Controller
    {
        [HttpGet]
        public ActionResult MiPerfil()
        {
            if (Session["UsuarioId"] == null)
                return RedirectToAction("InicioSesion", "Login");


            int idUsuario = (int)Session["UsuarioId"];

            using (var context = new BDCoromotoEntities())
            {
                var usuario = context.tUsuario.FirstOrDefault(u => u.ConsecutivoCliente == idUsuario);

                if (usuario == null)
                {
                    TempData["MensajePantalla"] = "Usuario no encontrado.";
                    return RedirectToAction("Login", "Cuenta");
                }

                var model = new Usuario
                {
                    ConsecutivoCliente = usuario.ConsecutivoCliente,
                    Identificacion = usuario.Identificacion,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    CorreoElectronico = usuario.CorreoElectronico,
                    Telefono = usuario.Telefono,
                    FotoPerfil = usuario.FotoPerfil,
                    ConsecutivoRol = usuario.ConsecutivoRol
                };

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult MiPerfil(Usuario model)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensajePantalla"] = "Datos inválidos.";
                return RedirectToAction("MiPerfil");
            }

            using (var context = new BDCoromotoEntities())
            {
                var usuario = context.tUsuario.FirstOrDefault(u => u.ConsecutivoCliente == model.ConsecutivoCliente);

                if (usuario == null)
                {
                    TempData["MensajePantalla"] = "Usuario no encontrado.";
                    return RedirectToAction("MiPerfil");
                }

                usuario.Nombre = model.Nombre;
                usuario.Apellido = model.Apellido;
                usuario.CorreoElectronico = model.CorreoElectronico;
                usuario.Telefono = model.Telefono;

                context.SaveChanges();

                TempData["MensajePantalla"] = "Información actualizada correctamente.";
                return RedirectToAction("MiPerfil");
            }
        }
    }
}
