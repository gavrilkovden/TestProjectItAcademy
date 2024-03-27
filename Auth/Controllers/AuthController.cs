using Auth.Application.Commands.CreateJwtToken;
using Auth.Application.Commands.CreateJwtTokenByRefreshToken;
using Common.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Users.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("CreateJwtToken")]
        public async Task<ActionResult> CreateJwtToken(CreateJwtTokenCommand createJwtTokenCommand, 
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var createdUser = await mediator.Send(createJwtTokenCommand, cancellationToken);
            return Ok(createdUser);
        }

        [AllowAnonymous]
        [HttpPost("CreateJwtTokenByRefreshToken")]
        public async Task<ActionResult> CreateJwtTokenByRefreshToken(CreateJwtTokenByRefreshTokenCommand createJwtTokenByRefreshTokenCommand,
            IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var createdUser = await mediator.Send(createJwtTokenByRefreshTokenCommand, cancellationToken);
            return Ok(createdUser);
        }
    }
}
