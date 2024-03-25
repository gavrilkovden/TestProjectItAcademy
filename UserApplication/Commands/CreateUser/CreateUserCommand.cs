using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;

namespace UserApplication.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<GetUserDTO>
    {
        public string? UserName { get; set; }
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
