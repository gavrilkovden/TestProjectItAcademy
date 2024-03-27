using Auth.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Commands.CreateJwtToken
{
    public class CreateJwtTokenCommand : IRequest<JwtTokenDto>
    {
        public string? UserName { get; set; }
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
