using AutoMapper;
using Common.Application.Exceptions;
using Common.Domain;
using Common.Application;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApplication.Commands.UpdateUser;
using Users.Service.DTO;
using MediatR;
using UserApplication.Queries.GetUser;

namespace UserApplication.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyCollection<GetUserDTO>>
    {
        private readonly IRepository<ApplicationUser> _users;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public GetAllUsersQueryHandler(IRepository<ApplicationUser> users,
            IMapper mapper,
            IMemoryCache memoryCache)
        {
            _users = users;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<IReadOnlyCollection<GetUserDTO>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _users.GetListAsync();

            if (!users.Any())
            {
                throw new NotFoundException("No ApplicationUsers found.");
            }
          //  _memoryCache.Cache.Clear();
            return _mapper.Map<IReadOnlyCollection<GetUserDTO>>(users);

        }
    }
}
