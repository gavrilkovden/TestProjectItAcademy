using AutoMapper;
using Common.Domain;
using Common.Domain.Exceptions;
using Common.Repositories;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Linq.Expressions;
using System.Security.Claims;
using Todos.Domain;
using Todos.Service.DTO;
using Users.Service;
using Users.Service.DTO;

namespace Todos.Service
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<Todo> _repository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public TodoService(IRepository<Todo> repository, IMapper mapper, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IEnumerable<Todo>> GetAllTodos(int? offset = null, int? limit = null, int? ownerId = null, string? labelFreeText = null)
        {
                if (ownerId.HasValue && (await _userService.GetUserAsync(u => u.Id == ownerId.Value)) == null)
                {
                    throw new NotFoundException($"User with Id {ownerId} not found.");
                }

                Log.Information($"Getting Todos. Offset: {offset}, Limit: {limit}, OwnerId: {ownerId}, LabelFreeText: {labelFreeText}");

                var todos = await _repository.GetListAsync(offset, limit, t =>
                    (string.IsNullOrWhiteSpace(labelFreeText) ||
                    t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase)) &&
                    (ownerId == null || t.OwnerId == ownerId.Value));

                if (!todos.Any())
                {
                    throw new NotFoundException("No Todos found.");
                }

                return todos;
        }

        public async Task<Todo> GetTodoAsync(Expression<Func<Todo, bool>>? predicate = null)
        {
                Log.Information($"Getting Todo. Predicate: {predicate?.ToString()}");

                var todo = await _repository.SingleOrDefaultAsync(predicate);

                if (todo == null)
                {
                    throw new NotFoundException("Todo not found.");
                }

                return todo;
        }

        public async Task<int> GetTodoCountAsync()
        {
                Log.Information($"Getting Todo Count.");

                var count = await _repository.CountAsync();

                if (count == 0)
                {
                    throw new NotFoundException("No Todos found.");
                }

                return count;
        }

        public async Task<Todo> GreateTodoAsync(CreateTodoDTO todoDTO)
        {
                Log.Information($"Creating Todo: {JsonConvert.SerializeObject(todoDTO)}");

                var user = await _userService.GetUserAsync(u => u.Id == todoDTO.OwnerId);
                if (user == null)
                {
                    throw new NotFoundException("Todo with specified OwnerId not found.");
                }

                var todo = _mapper.Map<Todo>(todoDTO);
                return await _repository.AddAsync(todo);
        }

        public async Task<Todo> UpdateTodoAsync(UpdateTodoDTO updateTodoDTO)
        {
                Log.Information($"Updating Todo: {JsonConvert.SerializeObject(updateTodoDTO)}");

            var user = await _userService.GetUserAsync(u => u.Id == updateTodoDTO.OwnerId);
                if (user == null)
                {
                    throw new NotFoundException("Todo with specified OwnerId not found.");
                }

                var existingTodo = await GetTodoAsync(d => d.Id == updateTodoDTO.Id);

                if (existingTodo == null)
                {
                    throw new NotFoundException("Todo with specified Id not found.");
                }
                _mapper.Map(updateTodoDTO, existingTodo);

                return await _repository.UpdateAsync(existingTodo);
        }

        public async Task<bool> DeleteTodoAsync(UpdateTodoDTO updateTodoDTO)
        {
                Log.Information($"Deleting Todo: {JsonConvert.SerializeObject(updateTodoDTO)}");

                var existingTodo = await GetTodoAsync(d => d.Id == updateTodoDTO.Id);

                if (existingTodo == null)
                {
                    throw new NotFoundException("Todo with specified Id not found.");
                }


                return await _repository.DeleteAsync(existingTodo);
        }
    }
}