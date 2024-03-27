using Common.Application;
using Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAplication.Queries.GetTodo;
using Todos.Domain;

namespace TodoAplication.Queries.GetTodoCount
{
    public class GetTodoCountQueryHandler : IRequestHandler<GetTodoCountQuery, int>
    {
        private readonly IRepository<Todo> _todos;

        public GetTodoCountQueryHandler(IRepository<Todo> todos)
        {
            _todos = todos;
        }
        public async Task<int> Handle(GetTodoCountQuery request, CancellationToken cancellationToken)
        {

            Log.Information($"Getting Todo Count.");

            var count = await _todos.CountAsync();

            if (count == 0)
            {
                throw new NotFoundException("No Todos found.");
            }

            return count;

        }
    }
}
