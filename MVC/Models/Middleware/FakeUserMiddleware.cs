using System.Security.Claims;

namespace Cs.Unicam.TrashHunter.Web.Models.Middleware
{
    public class FakeUserMiddleware
    {
        private readonly RequestDelegate _next;

        public FakeUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var fakeUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, "lorenzo"),
            new Claim(ClaimTypes.Email, "lorenzo.toscano@hunter.com")
        }, "FakeAuthentication"));

            context.User = fakeUser;

            await _next(context);
        }
    }
}
