using Common.Domain;
using Common.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users.Service;
using Users.Service.DTO;
using UserApplication.Queries.GetUser;
using UserApplication.Queries.GetUserCount;
using UserApplication.Queries.GetAllUsers;
using UserApplication.Commands.CreateUser;
using MediatR;
using UserApplication.Commands.UpdateUser;
using UserApplication.Commands.ChangePassword;
using UserApplication.Commands.DeleteUser;

namespace Users.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet("id")]
        public async Task<ActionResult<ApplicationUser>> GetUserById([FromQuery] GetUserQuery getUserQuery,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var user = await mediator.Send(getUserQuery, cancellationToken);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount([FromQuery] GetUserCountQuery getUserCountQuery,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var count = await mediator.Send(getUserCountQuery, cancellationToken);

            return Ok(count);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers([FromQuery]GetAllUsersQuery getAllUsersQuery,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var users = await mediator.Send(getAllUsersQuery, cancellationToken);
            var totalCount = await mediator.Send(new GetUserCountQuery(), cancellationToken);
            Response.Headers.Add("x-Total-Count", totalCount.ToString());

            return Ok(users);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> AddUser(CreateUserCommand createUserCommand,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var createdUser = await mediator.Send(createUserCommand, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UpdateUserDTO>> UpdateUser(UpdateUserCommand updateUserCommand,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null) { return BadRequest("Invalid user identifier."); }

            if (!User.IsInRole("Admin") && updateUserCommand.Id != int.Parse(currentUserIdClaim))
            {
                return Forbid("The current user does not have access to this operation.");
            }
            var existingUser = await mediator.Send(updateUserCommand, cancellationToken);

            if (existingUser == null)
            {
                return NotFound();
            }

            return Ok(updateUserCommand);
        }

        [Authorize]
        [HttpPut("{id}/Password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand,
           IMediator mediator,
           CancellationToken cancellationToken = default)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null) { return BadRequest("Invalid user identifier."); }

            if (!User.IsInRole("Admin") && changePasswordCommand.UserId != int.Parse(currentUserIdClaim))
            {
                return Forbid();
            }

            await mediator.Send(changePasswordCommand, cancellationToken);

            return Ok("Password changed successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(DeleteUserCommand deleteUserCommand,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            await mediator.Send(deleteUserCommand, cancellationToken);
            return Ok("User deleted successfully.");
        }
    }
}
