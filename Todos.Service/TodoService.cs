using AutoMapper;
using Common.Domain;
using Common.Domain.Exceptions;
using Common.Repositories;
using Newtonsoft.Json;
using Serilog;
using System.Linq.Expressions;
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
            try
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
            catch (BadRequestException ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todos - BadRequestException.");
                throw;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todos - NotFoundException.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todos.");
                throw;
            }
        }

        public async Task<Todo> GetTodoAsync(Expression<Func<Todo, bool>>? predicate = null)
        {
            try
            {
                Log.Information($"Getting Todo. Predicate: {predicate?.ToString()}");

                var todo = await _repository.SingleOrDefaultAsync(predicate);

                if (todo == null)
                {
                    throw new NotFoundException("Todo not found.");
                }

                return todo;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todo - NotFoundException.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todo.");
                throw;
            }
        }

        public async Task<int> GetTodoCountAsync()
        {
            try
            {
                Log.Information($"Getting Todo Count.");

                var count = await _repository.CountAsync();

                if (count == 0)
                {
                    throw new NotFoundException("No Todos found.");
                }

                return count;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todo Count - NotFoundException.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todo Count.");
                throw;
            }
        }

        public async Task<Todo> GreateTodoAsync(CreateTodoDTO todoDTO)
        {
            try
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
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error creating Todo - NotFoundException.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error creating Todo.");
                throw;
            }
        }

        public async Task<Todo> UpdateTodoAsync(UpdateTodoDTO updateTodoDTO)
        {
            try
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
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error updating Todo - NotFoundException.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error updating Todo.");
                throw;
            }
        }

        public async Task<bool> DeleteTodoAsync(UpdateTodoDTO updateTodoDTO)
        {
            try
            {
                Log.Information($"Deleting Todo: {JsonConvert.SerializeObject(updateTodoDTO)}");

                var existingTodo = await GetTodoAsync(d => d.Id == updateTodoDTO.Id);

                if (existingTodo == null)
                {
                    throw new NotFoundException("Todo with specified Id not found.");
                }

                return await _repository.DeleteAsync(existingTodo);
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error deleting Todo - NotFoundException.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error deleting Todo.");
                throw;
            }
        }
    }
}