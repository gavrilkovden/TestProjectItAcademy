using Common.Domain;
using Common.Repositories;
using FluentValidation;
using System.Reflection;
using Todos.Domain;
using Todos.Service;
using Users.Service;

namespace Todos.Api
{
    public static class TodoServiceCollectionExtensions
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services)
        {
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IRepository<Todo>, BaseRepository<Todo>>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
            return services;
        }
    }
}
