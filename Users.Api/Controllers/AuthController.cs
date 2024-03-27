//using Common.Domain;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq.Expressions;
//using System.Security.Claims;
//using Users.Service;
//using Users.Service.DTO;

//namespace Users.Api.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/users/auth")]
//    public class AuthController : ControllerBase
//    {
//        private readonly IAuthService _authService;
//        private readonly IUserService _userService;

//        public AuthController(IAuthService authService, IUserService userService )
//        {
//            _authService = authService;
//            _userService = userService;
//        }

//        [AllowAnonymous]
//        [HttpPost("CreateJwtToken")]
//        public async Task<ActionResult> CreateJwtToken(AuthDTO authDTO, CancellationToken cancellationToken = default)
//        {
//            var createdUser = await _authService.GetJwtTokenAsync(authDTO, cancellationToken);
//            return Ok(createdUser);
//        }

//        [AllowAnonymous]
//        [HttpPost("CreateJwtTokenByRefreshToken")]
//        public async Task<ActionResult> CreateJwtTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken = default)
//        {
//            var createdUser = await _authService.GetJwtTokenByRefreshTokenAsync(refreshToken, cancellationToken);
//            return Ok(createdUser);
//        }

//        [HttpGet("GetMyInfo")]
//        public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken)
//        {
//            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (currentUserId == null)
//            {
//                return BadRequest("User not authenticated.");
//            }
          
//            var user = await _userService.GetUserAsync(u => u.Id == int.Parse(currentUserId), cancellationToken);
//            return Ok(user);
//        }
//        [Authorize(Roles = "Admin")]
//        [HttpGet("IAmAdmin")]
//        public async Task<IActionResult> IAmAdmin(CancellationToken cancellationToken)
//        {
//            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (currentUserId == null)
//            {
//                return BadRequest("User not authenticated.");
//            }

//            var user = await _userService.GetUserAsync(u => u.Id == int.Parse(currentUserId), cancellationToken);
//            return Ok(user);
//        }

//    }
//}
