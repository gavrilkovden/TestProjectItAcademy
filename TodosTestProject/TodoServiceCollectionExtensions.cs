using Common.Application;
using Common.Domain;
using FluentValidation;
using System.Reflection;
using TodoAplication.Commands.CreateTodo;
using Todos.Domain;
using UserApplication.Commands.CreateUser;

namespace Todos.Api
{
    public static class TodoServiceCollectionExtensions
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services)
        {
        //    services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IRepository<Todo>, EntityRepository<Todo>>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<IRepository<ApplicationUserApplicationRole>, EntityRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IRepository<ApplicationUserRole>, EntityRepository<ApplicationUserRole>>();
            services.AddTransient<IRepository<RefreshToken>, EntityRepository<RefreshToken>>();
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly(), typeof(TodoServiceCollectionExtensions).Assembly }, includeInternalTypes: true);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTodoCommand).Assembly));
            services.AddMemoryCache();
            return services;
        }
    }
}
