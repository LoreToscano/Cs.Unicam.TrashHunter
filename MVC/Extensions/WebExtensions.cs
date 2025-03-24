using Cs.Unicam.TrashHunter.Models.Entities;
using System.Security.Claims;

namespace Cs.Unicam.TrashHunter.MVC.Extensions
{
    public static class WebExtensions
    {
        public static bool IsInRole(this ClaimsPrincipal user, Role role)
        {
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userRole) || !Enum.TryParse<Role>(userRole, out var parsedRole))
                return false;
            return role == parsedRole;
        }
    }
}
