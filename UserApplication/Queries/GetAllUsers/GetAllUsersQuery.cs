using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApplication.DTO;
using Users.Service.DTO;

namespace UserApplication.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IReadOnlyCollection<GetUserDTO>>
    {
    }
}
