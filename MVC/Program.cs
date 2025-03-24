using Cs.Unicam.TrashHunter.Web.Models.Middleware;
using Cs.Unicam.TrashHunter.Models;
using Cs.Unicam.TrashHunter.Services;
using Cs.Unicam.TrashHunter.Models.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.SetUpModel(builder.Configuration)
    .SetUpServices(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Logout";
    });

var app = builder.Build();
//app.UseMiddleware<FakeUserMiddleware>();

using var context = app.Services.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetRequiredService<TrashHunterContext>();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
context.SeedGenerico();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
