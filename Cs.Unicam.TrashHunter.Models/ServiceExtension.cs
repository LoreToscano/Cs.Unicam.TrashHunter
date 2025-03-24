using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.DB;
using Cs.Unicam.TrashHunter.Models.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models
{
    public static class ServiceExtension
    {
        public static IServiceCollection SetUpModel(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrashHunterContext>(options => options.UseSqlServer(configuration.GetConnectionString("TrashHunterContext")));
            services.AddScoped<TrashHunterContext>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            return services;
        }

    }
}
