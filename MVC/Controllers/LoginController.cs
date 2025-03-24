using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.MVC.Models.ViewModels;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cs.Unicam.TrashHunter.MVC.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IRepository<User> _userRepository;
        public LoginController(IServiceScopeFactory factory, IRepository<User> repository) : base(factory)
        {
            _userRepository = repository;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToHome();
            var vm = new LoginViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToHome();
            if (vm.Email == null || vm.Password == null)
            {
                vm.ErrorMessage = "Inserisci email e password";
                return View(vm);
            }

            var user = await _userRepository.Find(vm.Email);
            if (user == null)
            {
                vm.ErrorMessage = "Utente non esistente";
            } else if (user.Password != vm.Password)
            {
                vm.ErrorMessage = "Password errata";
            } else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                _user = new UserDTO(user);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToHome();
            }
            vm.Password = null;
            return View(vm);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }

    }
}
