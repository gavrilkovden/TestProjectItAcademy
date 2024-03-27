using Auth.Application.Commands.CreateJwtToken;
using Common.Application;
using Common.Domain;
using FluentValidation;
using System.Reflection;

namespace Auth.Api
{
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddTransient<IRepository<ApplicationUser>, EntityRepository<ApplicationUser>>();
            services.AddTransient<IRepository<RefreshToken>, EntityRepository<RefreshToken>>();
            services.AddTransient<IRepository<ApplicationUserApplicationRole>, EntityRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IRepository<ApplicationUserRole>, EntityRepository<ApplicationUserRole>>();
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly(), typeof(AuthServiceCollectionExtensions).Assembly }, includeInternalTypes: true);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateJwtTokenCommand).Assembly));

            return services;
        }
    }
}
