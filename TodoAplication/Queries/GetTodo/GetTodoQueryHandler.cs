using Common.Application;
using Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Todos.Domain;

namespace TodoAplication.Queries.GetTodo
{
    public class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, Todo>
    {
        private readonly IRepository<Todo> _todos;
        private readonly IMemoryCache _memoryCache;

        public GetTodoQueryHandler(IRepository<Todo> todos, IMemoryCache memoryCache)
        {
            _todos = todos;
            _memoryCache = memoryCache;
        }
        public async Task<Todo> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            Log.Information($"Getting Todo with id: {request.TodoId}");

            var todo = await _todos.SingleOrDefaultAsync(u => u.Id == request.TodoId, cancellationToken);

            if (todo == null)
            {
                throw new NotFoundException("Todo not found.");
            }
            //     _memoryCache.Cache.Clear();
            return todo;
        }
    }
}
