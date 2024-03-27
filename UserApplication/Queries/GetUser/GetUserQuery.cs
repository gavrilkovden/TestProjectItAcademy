using Common.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;

namespace UserApplication.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserDTO>
    {
        public int UserId { get; set; }
    }
}
