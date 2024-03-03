using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Todos.Domain;
using Todos.Service;
using Todos.Service.DTO;

namespace TodosTestProject.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> GetTodoById(int id)
        {
            var todo = _todoService.GetTodo(u => u.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpGet("count")]
        public ActionResult<int> GetCount()
        {
            var count = _todoService.GetTodoCount();

            return Ok(count);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Todo>> GetAllTodos(int? offset = null, int? limit = null,
 int? ownerId = null, string? labelFreeText = null)
        {
            var todos = _todoService.GetAllTodos(offset, limit, ownerId, labelFreeText);
            var totalCount = todos.Count();
            Response.Headers.Add("x-Total-Count", totalCount.ToString());
            return Ok(todos);
        }

        [HttpPost]
        public ActionResult AddTodo(TodoDTO todoDTO)
        {
            _todoService.GreateTodo(todoDTO);
            return CreatedAtAction(nameof(GetTodoById), new { id = todoDTO.Id }, todoDTO);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateTodo(TodoDTO updatedTodo)
        {
            var existingTodo = _todoService.UpdateTodo(updatedTodo);

            if (existingTodo == null)
            {
                return NotFound();
            }

            return Ok(updatedTodo);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTodo(TodoDTO todoDTO)
        {
            var existingTodo = _todoService.DeleteTodo(todoDTO);

            if (existingTodo == null)
            {
                return NotFound();
            }
            return Ok("Todo deleted successfully.");
        }
    }
}