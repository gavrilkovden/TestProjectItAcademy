using Common.Domain;
using Common.Repositories;
using Todos.Domain;
using Todos.Repositories;
using Todos.Service;
using Users.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ITodoService, TodoService>();
builder.Services.AddSingleton<IRepository<Todo>, BaseRepository<Todo>>();
builder.Services.AddAutoMapper(typeof(TodoService).Assembly);
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IRepository<User>, BaseRepository<User>>();
builder.Services.AddAutoMapper(typeof(UserService).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
