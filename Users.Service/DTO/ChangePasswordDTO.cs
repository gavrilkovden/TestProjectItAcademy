﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Service.DTO
{
    public class ChangePasswordDTO
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = default!;
    }
}
