using Auth.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Commands.CreateJwtTokenByRefreshToken
{
    public class CreateJwtTokenByRefreshTokenCommand : IRequest<JwtTokenDto>
    {
        public string? RefreshToken { get; set; }
    }
}
