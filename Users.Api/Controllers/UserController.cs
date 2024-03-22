using Common.Domain;
using Common.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users.Service;
using Users.Service.DTO;

namespace Users.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetUserAsync(u => u.Id == id, cancellationToken);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            var count = await _userService.GetUserCountAsync();

            return Ok(count);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var totalCount = await _userService.GetUserCountAsync();
            Response.Headers.Add("x-Total-Count", totalCount.ToString());

            return Ok(users);
        }


        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> AddUser(CreateUserDTO createUserDTO, CancellationToken cancellationToken = default)
        {
            var createdUser = await _userService.CreateUserAsync(createUserDTO, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UpdateUserDTO>> UpdateUser(UpdateUserDTO updatedUser)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null) { return BadRequest("Invalid user identifier."); }

            if (!User.IsInRole("Admin") && updatedUser.Id != int.Parse(currentUserIdClaim))
            {
                return Forbid("The current user does not have access to this operation.");
            }
            var existingUser = await _userService.UpdateUserAsync(updatedUser);

            if (existingUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [Authorize]
        [HttpPut("{id}/Password")]
        public async Task<ActionResult> ChangePassword(int id, ChangePasswordDTO changePasswordDTO)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null) { return BadRequest("Invalid user identifier."); }

            if (!User.IsInRole("Admin") && id != int.Parse(currentUserIdClaim))
            {
                return Forbid();
            }

            await _userService.ChangePasswordAsync(id, changePasswordDTO.NewPassword);

            return Ok("Password changed successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var updateUserDTO = new UpdateUserDTO { Id = id };
            await _userService.DeleteUserAsync(updateUserDTO);
            return Ok("User deleted successfully.");
        }
    }
}
