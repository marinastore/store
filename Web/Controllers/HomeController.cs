using System.Web.Mvc;

namespace Marina.Store.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Content("[ Я  &mdash; заглушка будущего великого проекта ]");
        }

    }
}
