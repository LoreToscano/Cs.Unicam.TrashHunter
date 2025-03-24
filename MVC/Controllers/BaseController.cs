using Cs.Unicam.TrashHunter.Models.DB;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cs.Unicam.TrashHunter.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected UserDTO _user;
        private readonly IServiceScopeFactory _factory;
        public BaseController(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (User.Identity.IsAuthenticated)
            {
                using var scope = _factory.CreateScope();
                using var dbContext = scope.ServiceProvider.GetRequiredService<TrashHunterContext>();
                _user = new UserDTO(dbContext.Users.FirstOrDefault(u => u.Email == User.Identity.Name));
                ViewBag.User = _user;
            }

            base.OnActionExecuting(context);
        }


        public static string GetControllerName<T>() where T : Controller
        {
            return typeof(T).Name.Replace("Controller", "");
        }

        protected IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
