using System.Web.Mvc;
using System.Web.Security;

namespace CameraWebService.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}