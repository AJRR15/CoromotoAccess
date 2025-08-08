using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public ActionResult AccesoDenegado()
        {
            Response.StatusCode = 403;
            return View();
        }

        [AllowAnonymous]
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
