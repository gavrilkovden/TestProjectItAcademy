using MediatR;
using Todos.Domain;

namespace TodoAplication.Commands.CreateTodo
{
    public class CreateTodoCommand : IRequest<Todo>
    {
        public string? Label { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int OwnerId { get; set; }
    }
}
