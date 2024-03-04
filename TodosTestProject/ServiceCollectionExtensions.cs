using Common.Domain;
using Common.Repositories;
using Todos.Domain;
using Todos.Service;
using Users.Service;

namespace Todos.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services)
        {
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IRepository<Todo>, BaseRepository<Todo>>();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepository<User>, BaseRepository<User>>();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
