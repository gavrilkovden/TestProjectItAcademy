using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.DTO
{
    public class JwtTokenDto
    {
        public string JwtToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime Expires { get; set; }
    }
}
