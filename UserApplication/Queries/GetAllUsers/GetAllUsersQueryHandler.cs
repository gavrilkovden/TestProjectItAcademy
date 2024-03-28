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
using System.Text.Json.Serialization;
using System.Text.Json;

namespace UserApplication.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyCollection<GetUserDTO>>
    {
        private readonly IRepository<ApplicationUser> _users;
        private readonly IMapper _mapper;
        private readonly MemoryCache _memoryCache;

        public GetAllUsersQueryHandler(IRepository<ApplicationUser> users,
            IMapper mapper,
            UsersMemoryCache memoryCache)
        {
            _users = users;
            _mapper = mapper;
            _memoryCache = memoryCache.Cache;
        }

        public async Task<IReadOnlyCollection<GetUserDTO>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            if (_memoryCache.TryGetValue(cashKey, out IReadOnlyCollection<GetUserDTO>? result))
            {
                return result!;
            }

            var users = await _users.GetListAsync();

            if (!users.Any())
            {
                throw new NotFoundException("No ApplicationUsers found.");
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                          .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                          .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                          .SetSize(3);
            _memoryCache.Set(cashKey, result, cacheEntryOptions);
            _memoryCache.Clear();
            return _mapper.Map<IReadOnlyCollection<GetUserDTO>>(users);

        }
    }
}
