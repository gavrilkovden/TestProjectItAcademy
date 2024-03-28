using Common.Application;
using Common.Application.Abstractions.Transaction;
using Common.Domain;
using FluentValidation;
using MediatR;
using System.Reflection;
using TodoAplication.Commands.CreateTodo;
using Todos.Domain;
using UserApplication;
using UserApplication.Commands.CreateUser;
using Users.Api;

namespace Todos.Api
{
    public static class TodoServiceCollectionExtensions
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Todo>, EntityRepository<Todo>>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<IRepository<ApplicationUserApplicationRole>, EntityRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IRepository<ApplicationUserRole>, EntityRepository<ApplicationUserRole>>();
            services.AddTransient<IRepository<RefreshToken>, EntityRepository<RefreshToken>>();
            services.AddTransient<IContextTransactionCreator, ContextTransactionCreator>();
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly(), typeof(TodoServiceCollectionExtensions).Assembly }, includeInternalTypes: true);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTodoCommand).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ContextTransactionBehavior<,>));
            services.AddSingleton<UsersMemoryCache>();
            return services;
        }
    }
}
