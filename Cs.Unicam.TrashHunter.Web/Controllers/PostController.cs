using Microsoft.AspNetCore.Mvc;

namespace Cs.Unicam.TrashHunter.Web.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
