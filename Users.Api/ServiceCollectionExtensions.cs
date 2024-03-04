using Common.Domain;
using Common.Repositories;
using Users.Service;

namespace Users.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepository<User>, BaseRepository<User>>();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
