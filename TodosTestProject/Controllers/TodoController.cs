using Common.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoAplication.Commands.CreateTodo;
using TodoAplication.Commands.DeleteTodo;
using TodoAplication.Commands.UpdateTodo;
using TodoAplication.DTO;
using TodoAplication.Queries.GetAllTodos;
using TodoAplication.Queries.GetTodo;
using TodoAplication.Queries.GetTodoCount;
using Todos.Domain;
using UserApplication.Commands.CreateUser;
using UserApplication.Commands.DeleteUser;
using UserApplication.Queries.GetUser;

namespace TodosTestProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet("id")]
        public async Task<ActionResult<Todo>> GetTodoById([FromQuery] GetTodoQuery getTodoQuery,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserIdClaim == null)
            {
                return BadRequest("Invalid user identifier.");
            }

            var todo = await mediator.Send(getTodoQuery, cancellationToken);

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
        public async Task<ActionResult<int>> GetCount([FromQuery] GetTodoCountQuery getTodoCountQuery,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var count = await mediator.Send(getTodoCountQuery, cancellationToken);

            return Ok(count);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos([FromQuery] GetAllTodosQuery getAllTodosQuery,
            [FromQuery] GetTodoCountQuery getTodoCountQuery,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            int currentUserId;
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim, out currentUserId))
            {
                return BadRequest("Invalid user identifier.");
            }

            IEnumerable<Todo> todos;
            int totalCount;

            todos = await mediator.Send(User.IsInRole("Admin") ? getAllTodosQuery : new GetAllTodosQuery { OwnerId = currentUserId }, cancellationToken);
            totalCount = await mediator.Send(getTodoCountQuery, cancellationToken);
            Response.Headers.Add("x-Total-Count", totalCount.ToString());

            return Ok(todos);
        }

        [HttpPost]
        public async Task<ActionResult> AddTodo(CreateTodoCommand createTodoCommand,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var createdTodo = await mediator.Send(createTodoCommand, cancellationToken);

            if (createdTodo == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateTodoDTO>> UpdateTodo(UpdateTodoCommand updateTodoCommand,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null)

            {
                return BadRequest("Invalid user identifier.");
            }

            if (updateTodoCommand.OwnerId != int.Parse(currentUserIdClaim) && !User.IsInRole("Admin"))
            {
                return Forbid("The current user does not have access to update this todo.");
            }
            var existingTodo = await mediator.Send(updateTodoCommand, cancellationToken);

            if (existingTodo == null)
            {
                return NotFound();
            }

            return Ok(existingTodo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(DeleteTodoCommand deleteTodoCommand ,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null)

            {
                return BadRequest("Invalid user identifier.");
            }

            if (deleteTodoCommand.OwnerId != int.Parse(currentUserIdClaim) && !User.IsInRole("Admin"))
            {
                return Forbid("The current user does not have access to delete this todo.");
            }

            await mediator.Send(deleteTodoCommand, cancellationToken);
            return Ok("Todo deleted successfully.");
        }
    }
}