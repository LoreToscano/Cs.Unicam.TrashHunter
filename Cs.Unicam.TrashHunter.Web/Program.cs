using Cs.Unicam.TrashHunter.Models;
using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.DB;
using Cs.Unicam.TrashHunter.Services;
using Cs.Unicam.TrashHunter.Web.Models.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.SetUpModel(builder.Configuration)
    .SetUpServices(builder.Configuration);
var app = builder.Build();
app.UseMiddleware<FakeUserMiddleware>();

using var context = app.Services.GetService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetRequiredService<TrashHunterContext>();
if (context.Database.EnsureCreated())
{
    context.SeedGenerico();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseMvc();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
