using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.MVC.Models.ViewModels;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace Cs.Unicam.TrashHunter.MVC.Controllers
{
    public class UserController : BaseController
    {
        private readonly IRepository<User> _repository;
        public UserController(IRepository<User> repository, IServiceScopeFactory factory) : base(factory)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index(string userId = null)
        {
            UserDTO userDTO = _user;
            var user = await _repository.Find(userId ?? userDTO.Email);
            if (user != null)
                userDTO = new UserDTO(user);
            if (userDTO == null || user == null) 
                return RedirectToHome();
            var userViewModel = new UserViewModel(userDTO, user.Post.Concat(user.PostCompleted).Select(p => new PostDTO(p, null)));
            return View(userViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return View();
            var user = await _repository.Find(email);
            if (user == null)
                return View();
            if (user.Password != password)
                return View();
            
            var principal = GetPrincipal(user);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, GetClaims(principal, user));
            return RedirectToAction("Index", "Home");
        }




        private IPrincipal GetPrincipal(User user)
        {
            var identity = new GenericIdentity(user.Email);
            var principal = new GenericPrincipal(identity, new string[] { user.Role.GetRoleString() });
            return principal;
        }

        private ClaimsPrincipal GetClaims(IPrincipal principal, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.GetRoleString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }
}
