using AutoMapper;
using Common.Domain;
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

            if (_repository.Count() == 0)
            {
                _repository.Add(new Todo { Id = 1, Label = "LabelName1", CreatedDate = DateTime.UtcNow, OwnerId = 2 });
                _repository.Add(new Todo { Id = 2, Label = "LabelName2", CreatedDate = DateTime.UtcNow, OwnerId = 3 });
                _repository.Add(new Todo { Id = 3, Label = "LabelName3", CreatedDate = DateTime.UtcNow, OwnerId = 4 });
                _repository.Add(new Todo { Id = 4, Label = "LabelName4", CreatedDate = DateTime.UtcNow, OwnerId = 5 });
                _repository.Add(new Todo { Id = 5, Label = "LabelName5", CreatedDate = DateTime.UtcNow, OwnerId = 1 });
            }
        }

        public IEnumerable<Todo> GetAllTodos(int? offset = null, int? limit = null, int? ownerId = null, string? labelFreeText = null)
        {
            try
            {
                Log.Information($"Getting Todos. Offset: {offset}, Limit: {limit}, OwnerId: {ownerId}, LabelFreeText: {labelFreeText}");

                return _repository.GetList(offset, limit, t => (string.IsNullOrWhiteSpace(labelFreeText) ||
                    t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase)) && (ownerId == null || t.OwnerId == ownerId.Value));
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todos.");
                throw;
            }
        }

        public Todo GetTodo(Expression<Func<Todo, bool>>? predicate = null)
        {
            try
            {
                Log.Information($"Getting Todo. Predicate: {predicate?.ToString()}");

                return _repository.SingleOrDefault(predicate);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todo.");
                throw;
            }

        }

        public int GetTodoCount()
        {
            try
            {
                Log.Information($"Getting Todo Count.");

                return _repository.Count();
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting Todo Count.");
                throw;
            }
        }

        public Todo GreateTodo(CreateTodoDTO todoDTO)
        {
            try
            {
                Log.Information($"Creating Todo: {JsonConvert.SerializeObject(todoDTO)}");

                var user = _userService.GetUser(u => u.Id == todoDTO.OwnerId);
                if (user == null)
                {
                    throw new Exception("Todo with specified OwnerId not found.");
                }

                var todo = _mapper.Map<Todo>(todoDTO);
                return _repository.Add(todo);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error creating Todo.");
                throw; 
            }
        }

        public Todo UpdateTodo(UpdateTodoDTO updateTodoDTO)
        {
            try
            {
                Log.Information($"Updating Todo: {JsonConvert.SerializeObject(updateTodoDTO)}");

                var user = _userService.GetUser(u => u.Id == updateTodoDTO.OwnerId);
                if (user == null)
                {
                    throw new Exception("Todo with specified OwnerId not found.");
                }

                var existingTodo = GetTodo(d => d.Id == updateTodoDTO.Id);

                if (existingTodo == null)
                {
                    throw new Exception("Todo with specified Id not found.");
                }
                _mapper.Map(updateTodoDTO, existingTodo);

                return _repository.Update(existingTodo);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error updating Todo.");
                throw;
            }

        }

        public bool DeleteTodo(UpdateTodoDTO updateTodoDTO)
        {
            try
            {
                Log.Information($"Deleting Todo: {JsonConvert.SerializeObject(updateTodoDTO)}");

                var existingTodo = GetTodo(d => d.Id == updateTodoDTO.Id);

                if (existingTodo == null)
                {
                    throw new Exception("Todo with specified Id not found.");
                }
                return _repository.Delete(existingTodo);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error deleting Todo.");
                throw;
            }
        }
    }
}