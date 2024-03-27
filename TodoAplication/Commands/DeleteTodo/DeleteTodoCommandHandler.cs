using Common.Application;
using Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using Todos.Domain;

namespace TodoAplication.Commands.DeleteTodo
{
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
    {
        private readonly IRepository<Todo> _todos;
        private readonly IMemoryCache _memoryCache;

        public DeleteTodoCommandHandler(IRepository<Todo> todos, IMemoryCache memoryCache)
        {
            _todos = todos;
            _memoryCache = memoryCache;
        }
        public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            Log.Information($"Deleting Todo: {JsonConvert.SerializeObject(request)}");

            var existingTodo = await _todos.SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (existingTodo == null)
            {
                throw new NotFoundException("Todo with specified Id not found.");
            }

            //     _memoryCache.Cache.Clear();
            return await _todos.DeleteAsync(existingTodo);

        }
    }
}
