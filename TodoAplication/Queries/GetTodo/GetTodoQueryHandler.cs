using Common.Application;
using Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Text.Json.Serialization;
using System.Text.Json;
using Todos.Domain;
using UserApplication;

namespace TodoAplication.Queries.GetTodo
{
    public class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, Todo>
    {
        private readonly IRepository<Todo> _todos;
        private readonly MemoryCache _memoryCache;

        public GetTodoQueryHandler(IRepository<Todo> todos, UsersMemoryCache memoryCache)
        {
            _todos = todos;
            _memoryCache = memoryCache.Cache;
        }
        public async Task<Todo> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            Log.Information($"Getting Todo with id: {request.TodoId}");

            var cashKey = JsonSerializer.Serialize(request, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            if (_memoryCache.TryGetValue(cashKey, out Todo? result))
            {
                return result!;
            }
            var todo = await _todos.SingleOrDefaultAsync(u => u.Id == request.TodoId, cancellationToken);

            if (todo == null)
            {
                throw new NotFoundException("Todo not found.");
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetSize(3);
            _memoryCache.Set(cashKey, result, cacheEntryOptions);
            _memoryCache.Clear();

            return todo;
        }
    }
}
