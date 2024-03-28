using AutoMapper;
using Common.Application;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using TodoAplication.Commands.CreateTodo;
using Todos.Domain;
using UserApplication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Users.Service.DTO;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TodoAplication.Queries.GetAllTodos
{
    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, IEnumerable<Todo>>
    {
        private readonly IRepository<Todo> _todos;
        private readonly MemoryCache _memoryCache;
        private readonly IRepository<ApplicationUser> _users;

        public GetAllTodosQueryHandler(
            IRepository<Todo> todos,
            UsersMemoryCache memoryCache,
            IRepository<ApplicationUser> users)
        {
            _todos = todos;
            _memoryCache = memoryCache.Cache;
            _users = users;
        }

        public async Task<IEnumerable<Todo>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            var cashKey = JsonSerializer.Serialize(request, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            if (_memoryCache.TryGetValue(cashKey, out IEnumerable<Todo>? result))
            {
                return result!;
            }

            if (request.OwnerId.HasValue && (await _users.SingleOrDefaultAsync(u => u.Id == request.OwnerId.Value, cancellationToken)) == null)
            {
                throw new NotFoundException($"User with Id {request.OwnerId} not found.");
            }

            Log.Information($"Getting Todos. Offset: {request.Offset}, Limit: {request.Limit}, OwnerId: {request.OwnerId}, LabelFreeText: {request.LabelFreeText}");

            var todos = await _todos.GetListAsync(request.Offset, request.Limit, t =>
                (string.IsNullOrWhiteSpace(request.LabelFreeText) ||
                t.Label.Contains(request.LabelFreeText, StringComparison.InvariantCultureIgnoreCase)) &&
                (request.OwnerId == null || t.OwnerId == request.OwnerId.Value));

            if (!todos.Any())
            {
                throw new NotFoundException("No Todos found.");
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetSize(3);
            _memoryCache.Set(cashKey, result, cacheEntryOptions);
            _memoryCache.Clear();

            return todos;

        }
    }
}
