using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;

namespace TodoAplication.Queries.GetAllTodos
{
    public class GetAllTodosQuery : IRequest<IEnumerable<Todo>>

    {
        public int? Offset { get; set;} = null;
        public int? Limit { get; set; } = null;
        public int? OwnerId { get; set; } = null;
        public string? LabelFreeText { get; set; } = null;
    }
}

