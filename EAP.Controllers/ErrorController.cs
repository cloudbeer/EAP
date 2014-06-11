using System.Web.Mvc;

namespace EAP.Controllers
{
    public class ErrorController : Zippy.SaaS.Mvc.Controller
    {

        public ActionResult NoPermission()
        {
            return View();
        }

        public ActionResult E404()
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            return View();
        }
    }
}
