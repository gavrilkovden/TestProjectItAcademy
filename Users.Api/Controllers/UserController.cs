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
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userService.GetUser(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("count")]
        public ActionResult<int> GetCount()
        {
            var count = _userService.GetUserCount();

            return Ok(count);
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            var totalCount = users.Count();
            Response.Headers.Add("x-Total-Count", totalCount.ToString());

            return Ok(users);
        }

        [HttpPost]
        public ActionResult AddUser(CreateUserDTO ñreateUserDTO)
        {
            var ñreateduser = _userService.GreateUser(ñreateUserDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = ñreateduser.Id }, ñreateduser);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(UpdateUserDTO updatedUser)
        {
            var existingUser = _userService.UpdateUser(updatedUser);

            if (existingUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(UpdateUserDTO updatedUser)
        {
            _userService.DeleteUser(updatedUser);
            return Ok("User deleted successfully.");
        }
    }
}
