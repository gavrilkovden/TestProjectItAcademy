using Common.Application;
using Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using TodoAplication.Queries.GetTodo;
using Todos.Domain;
using UserApplication;

namespace TodoAplication.Queries.GetTodoCount
{
    public class GetTodoCountQueryHandler : IRequestHandler<GetTodoCountQuery, int>
    {
        private readonly IRepository<Todo> _todos;
        private readonly MemoryCache _memoryCache;

        public GetTodoCountQueryHandler(IRepository<Todo> todos, UsersMemoryCache memoryCache)
        {
            _todos = todos;
            _memoryCache = memoryCache.Cache;
        }
        public async Task<int> Handle(GetTodoCountQuery request, CancellationToken cancellationToken)
        {
            Log.Information($"Getting Todo Count.");

            var cashKey = JsonSerializer.Serialize(request, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            if (_memoryCache.TryGetValue(cashKey, out int result))
            {
                return result!;
            }

            var count = await _todos.CountAsync();

            if (count == 0)
            {
                throw new NotFoundException("No Todos found.");
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
              .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
              .SetSlidingExpiration(TimeSpan.FromMinutes(5))
              .SetSize(3);
            _memoryCache.Set(cashKey, result, cacheEntryOptions);
            _memoryCache.Clear();

            return count;

        }
    }
}
