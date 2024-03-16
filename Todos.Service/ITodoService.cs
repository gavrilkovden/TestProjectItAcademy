using Common.Domain.Exceptions;
using Common.Repositories;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;
using Todos.Service.DTO;

namespace Todos.Service
{
    public interface ITodoService
    {
        public  Task<IEnumerable<Todo>> GetAllTodos(int? offset = null, int? limit = null, int? ownerId = null, string? labelFreeText = null);

        public  Task<Todo> GetTodoAsync(Expression<Func<Todo, bool>>? predicate = null);

        public  Task<int> GetTodoCountAsync();

        public  Task<Todo> GreateTodoAsync(CreateTodoDTO todoDTO);

        public  Task<Todo> UpdateTodoAsync(UpdateTodoDTO updateTodoDTO);

        public  Task<bool> DeleteTodoAsync(UpdateTodoDTO updateTodoDTO);

    }

}
