using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;

namespace UserApplication.Queries.GetUserCount
{
    public class GetUserCountQuery : IRequest<int>
    {

    }
}
