using Common.Domain;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserAsync(u => u.Id == id);

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
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var totalCount = await _userService.GetUserCountAsync();
            Response.Headers.Add("x-Total-Count", totalCount.ToString());

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(CreateUserDTO createUserDTO)
        {
            var createdUser = await _userService.CreateUserAsync(createUserDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateUserDTO>> UpdateUser(UpdateUserDTO updatedUser)
        {
            var existingUser = await _userService.UpdateUserAsync(updatedUser);

            if (existingUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
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
