using Common.Domain;
using Common.Repositories;
using FluentValidation;
using System.Reflection;
using Users.Service;
using Users.Service.DTO;
using Users.Service.Validators;

namespace Users.Api
{
    public static class UserServiceCollectionExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepository<User>, EntityRepository<User>>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly(), typeof(UserService).Assembly }, includeInternalTypes: true);

            return services;
        }
    }
}
