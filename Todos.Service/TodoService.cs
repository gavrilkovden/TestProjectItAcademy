using AutoMapper;
using Common.Repositories;
using System.Linq.Expressions;
using Todos.Domain;
using Todos.Service.DTO;
using Users.Service;

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

        public IEnumerable<Todo> GetAllTodos(int? offset = null, int? limit = null, 
            Expression<Func<Todo, object>>? orderBy = null,
            bool? descending = null, int? ownerId = null, string? labelFreeText = null)
        {
            return _repository.GetList(offset, limit, t => (string.IsNullOrWhiteSpace(labelFreeText) ||
            t.Label.Contains(labelFreeText, StringComparison.InvariantCultureIgnoreCase)) && (ownerId == null || t.OwnerId == ownerId.Value), orderBy, descending);
        }

        public Todo GetTodo(Expression<Func<Todo, bool>>? predicate = null)
        {
            return _repository.SingleOrDefault(predicate);
        }

        public int GetTodoCount()
        {
            return _repository.Count();
        }

        public Todo GreateTodo(TodoDTO todoDTO)
        {
            var user = _userService.GetUser(u => u.Id == todoDTO.OwnerId);
            if (user == null)
            {
                throw new Exception("User with specified OwnerId not found.");
            }

            var todo = _mapper.Map<Todo>(todoDTO);
            return _repository.Add(todo);
        }

        public Todo UpdateTodo(TodoDTO todoDTO)
        {
            var user = _userService.GetUser(u => u.Id == todoDTO.OwnerId);
            if (user == null)
            {
                throw new Exception("User with specified OwnerId not found.");
            }

            var todo = _mapper.Map<Todo>(todoDTO);
            return _repository.Update(todo);
        }

        public bool DeleteTodo(TodoDTO todoDTO)
        {
            var todo = _mapper.Map<Todo>(todoDTO);
            return _repository.Delete(todo);
        }
    }
}