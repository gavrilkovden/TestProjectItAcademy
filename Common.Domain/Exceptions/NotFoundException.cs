﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, string filter = null)
    : base($"{message} Filter: {filter}") { }
    }
}
