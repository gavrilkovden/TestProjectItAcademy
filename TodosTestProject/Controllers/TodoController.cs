using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
using Todos.Domain;
using Todos.Service;
using Todos.Service.DTO;

namespace TodosTestProject.Controllers
{
     [Authorize]
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
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserIdClaim == null) 
            { 
                return BadRequest("Invalid user identifier."); 
            }

            var todo = await _todoService.GetTodoAsync(u => u.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && todo.OwnerId != int.Parse(currentUserIdClaim))
            {
                return Forbid("You do not have access to this todo.");
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
            int currentUserId;
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim, out currentUserId))
            {
                return BadRequest("Invalid user identifier.");
            }

            IEnumerable<Todo> todos;
            int totalCount;

            if (User.IsInRole("Admin"))
            {
                todos = await _todoService.GetAllTodos(offset, limit, ownerId, labelFreeText);
                totalCount = await _todoService.GetTodoCountAsync();
            }
            else
            {
                todos = await _todoService.GetAllTodos(offset, limit, currentUserId, labelFreeText);
                totalCount = await _todoService.GetTodoCountAsync();
            }

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
        public async Task<ActionResult<UpdateTodoDTO>> UpdateTodo(UpdateTodoDTO updatedTodo)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null) 
            
            { 
                return BadRequest("Invalid user identifier."); 
            }

            if (updatedTodo.OwnerId != int.Parse(currentUserIdClaim) && !User.IsInRole("Admin"))
            {
                return Forbid("The current user does not have access to update this todo.");
            }
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
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null) 
            
            { 
                return BadRequest("Invalid user identifier."); 
            }

            var existingTodo = await _todoService.GetTodoAsync(u => u.Id == id);

            if (existingTodo.OwnerId != int.Parse(currentUserIdClaim) && !User.IsInRole("Admin"))
            {
                return Forbid("The current user does not have access to delete this todo.");
            }

            var todoDTO = new UpdateTodoDTO { Id = id };
            await _todoService.DeleteTodoAsync(todoDTO);
            return Ok("Todo deleted successfully.");
        }
    }
}