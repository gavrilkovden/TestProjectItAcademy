﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;

namespace TodoAplication.Queries.GetTodoCount
{
    public class GetTodoCountQuery : IRequest<int>
    {
    }
}
