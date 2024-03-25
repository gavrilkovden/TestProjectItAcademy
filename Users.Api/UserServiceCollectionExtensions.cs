using Common.Domain;
using Common.Repositories;
using FluentValidation;
using MediatR;
using System.Reflection;
using Users.Service;

namespace Users.Api
{
    public static class UserServiceCollectionExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepository<ApplicationUser>, EntityRepository<ApplicationUser>>();
            services.AddTransient<IRepository<RefreshToken>, EntityRepository<RefreshToken>>();
            services.AddTransient<IRepository<ApplicationUserApplicationRole>, EntityRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IRepository<ApplicationUserRole>, EntityRepository<ApplicationUserRole>>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly(), typeof(UserService).Assembly }, includeInternalTypes: true);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
