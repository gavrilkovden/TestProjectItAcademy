using Common.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog;
using System.Text;
using Auth.Api;
using Common.Application;
using Microsoft.Extensions.Configuration;

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
    var configuration = builder.Configuration;
    builder.Services.AddTodoDB(configuration);
    builder.Services.AddAuthServices();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Authorization header using the Bearer scheme.\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: 'Bearer 1234Sabcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
            });
    });

    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    var app = builder.Build();

    app.UseMiddleware<ExceptionsHandlerMiddleware>();
    // Configure the HTTP request pipeline.
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

catch (Exception ex)
{
    Log.Error(ex, messageTemplate: "Run error");
}
finally
{
    Log.Information("Application is shutting down.");
    Log.CloseAndFlush();
}