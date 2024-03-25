using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;

namespace UserApplication.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<GetUserDTO>
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
