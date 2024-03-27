using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAplication.Commands.CreateTodo;
using Todos.Domain;

namespace TodoAplication.Commands.UpdateTodo
{
    public class UpdateTodoCommandValidator : AbstractValidator<Todo>
    {
        public UpdateTodoCommandValidator()
        {
             // RuleFor(e => e.OwnerId).GreaterThan(2).NotEmpty().WithMessage("OwnerId error");
              //RuleFor(e => e.Label).MinimumLength(3).MaximumLength(10).Must(e => e.StartsWith("todo"));
        }
    }
}
