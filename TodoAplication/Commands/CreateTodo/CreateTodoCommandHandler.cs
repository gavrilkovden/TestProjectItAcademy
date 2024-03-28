using AutoMapper;
using Common.Application;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using Todos.Domain;

namespace TodoAplication.Commands.CreateTodo
{
    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Todo>
    {
        private readonly IRepository<Todo> _todos;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _users;

        public CreateTodoCommandHandler(
            IRepository<Todo> todos,
            IMapper mapper,
            IRepository<ApplicationUser> users)
        {
            _todos = todos;
            _mapper = mapper;
            _users = users;
        }

        public async Task<Todo> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            Log.Information($"Creating Todo: {JsonConvert.SerializeObject(request)}");

            var user = await _users.SingleOrDefaultAsync(u => u.Id == request.OwnerId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Todo with specified OwnerId not found.");
            }

            var todo = _mapper.Map<Todo>(request);

            return await _todos.AddAsync(todo);
        }
    }
}
