﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;

namespace TodoAplication.Commands.UpdateTodo
{
    public class UpdateTodoCommand : IRequest<Todo>
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int OwnerId { get; set; }
    }
}
