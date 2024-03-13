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
        public async Task<ActionResult<Todo>> GetTodoById(int id)
        {
            var todo = await _todoService.GetTodoAsync(u => u.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            var count = await _todoService.GetTodoCountAsync();

            return Ok(count);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos(int? offset = null, int? limit = null, int? ownerId = null, string? labelFreeText = null)
        {
            var todos = await _todoService.GetAllTodos(offset, limit, ownerId, labelFreeText);
            var totalCount = await _todoService.GetTodoCountAsync();
            Response.Headers.Add("x-Total-Count", totalCount.ToString());
            return Ok(todos);
        }

        [HttpPost]
        public async Task<ActionResult> AddTodo(CreateTodoDTO todoDTO)
        {
            var createdTodo = await _todoService.GreateTodoAsync(todoDTO);

            if (createdTodo == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodo(UpdateTodoDTO updatedTodo)
        {
            var existingTodo = await _todoService.UpdateTodoAsync(updatedTodo);

            if (existingTodo == null)
            {
                return NotFound();
            }

            return Ok(updatedTodo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var todoDTO = new UpdateTodoDTO { Id = id };
            await _todoService.DeleteTodoAsync(todoDTO);
            return Ok("Todo deleted successfully.");
        }
    }
}