using Common.Api;
using Common.Application;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Todos.Api;
using Users.Api;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("Logs/Informations - .txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .WriteTo.File("Logs/Log-Error - .txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddTodoServices();
    builder.Services.AddUserServices();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddTodoDB(builder.Configuration);

    builder.Host.UseSerilog();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ExceptionsHandlerMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

catch(Exception ex)
{
    Log.Error(ex, messageTemplate: "Run error");
}
finally
{
    Log.Information("Application is shutting down.");
    Log.CloseAndFlush();
}