using AutoMapper;
using Common.Application;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using Todos.Domain;

namespace TodoAplication.Commands.UpdateTodo
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, Todo>
    {
        private readonly IRepository<Todo> _todos;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IRepository<ApplicationUser> _users;

        public UpdateTodoCommandHandler(
            IRepository<Todo> todos,
            IMapper mapper,
            IMemoryCache memoryCache,
            IRepository<ApplicationUser> users)
        {
            _todos = todos;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _users = users;
        }

        public async Task<Todo> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            Log.Information($"Updating Todo: {JsonConvert.SerializeObject(request)}");

            var user = await _users.SingleOrDefaultAsync(u => u.Id == request.OwnerId);
            if (user == null)
            {
                throw new NotFoundException("Todo with specified OwnerId not found.");
            }

            var existingTodo = await _todos.SingleOrDefaultAsync(d => d.Id == request.Id);

            if (existingTodo == null)
            {
                throw new NotFoundException("Todo with specified Id not found.");
            }
            _mapper.Map(request, existingTodo);

            return await _todos.UpdateAsync(existingTodo);
        }
    }
}
