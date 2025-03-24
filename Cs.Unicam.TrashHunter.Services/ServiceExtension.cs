using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.Options;
using Cs.Unicam.TrashHunter.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Cs.Unicam.TrashHunter.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection SetUpServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AttachmentOptions>(configuration.GetSection(nameof(AttachmentOptions)));

            // Add services
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICuponServices, CuponServices>();

            // Configure options
            return services;
        }
    }
}
