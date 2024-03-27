using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.DTO
{
    public class AuthDTO
    {
        public string? UserName { get; set; }
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
