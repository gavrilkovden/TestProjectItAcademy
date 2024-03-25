using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;

namespace UserApplication.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<GetUserDTO>
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = default!;
    }
}
