using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application
{
    public static class DbContextDi
    {
        public static IServiceCollection AddTodoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    
            });

            return services;
        }
    }
}
