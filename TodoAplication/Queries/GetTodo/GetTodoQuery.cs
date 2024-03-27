using Common.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;

namespace TodoAplication.Queries.GetTodo
{
    public class GetTodoQuery : IRequest<Todo>
    {
        public int TodoId { get; set; }
    }
}
