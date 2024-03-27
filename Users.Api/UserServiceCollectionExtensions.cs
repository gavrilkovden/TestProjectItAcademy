using Common.Domain;
using Common.Application;
using FluentValidation;
using MediatR;
using System.Reflection;
using Users.Service;
using UserApplication.Commands.CreateUser;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Api
{
    public static class UserServiceCollectionExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
  //          services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepository<ApplicationUser>, EntityRepository<ApplicationUser>>();
            services.AddTransient<IRepository<RefreshToken>, EntityRepository<RefreshToken>>();
            services.AddTransient<IRepository<ApplicationUserApplicationRole>, EntityRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IRepository<ApplicationUserRole>, EntityRepository<ApplicationUserRole>>();
 //           services.AddTransient<IAuthService, AuthService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly(), typeof(UserServiceCollectionExtensions).Assembly }, includeInternalTypes: true);
           services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));
            services.AddMemoryCache();
            //  services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);


            return services;
        }
    }
}
