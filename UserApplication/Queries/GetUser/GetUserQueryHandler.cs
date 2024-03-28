using AutoMapper;
using Common.Application.Exceptions;
using Common.Domain;
using Common.Application;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UserApplication.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserDTO>
    {
        private readonly IRepository<ApplicationUser> _repository;
        private readonly IMapper _mapper;
        private readonly MemoryCache _memoryCache;

        public GetUserQueryHandler(IRepository<ApplicationUser> repository, IMapper mapper, UsersMemoryCache memoryCache)
        {
            _repository = repository;
            _mapper = mapper;
            _memoryCache = memoryCache.Cache;
        }

        public async Task<GetUserDTO> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            Log.Information($"Getting user with predicate: {query.UserId.ToString() ?? "null"}.");

            var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            if (_memoryCache.TryGetValue(cashKey, out GetUserDTO? result))
            {
                return result!;
            }
            var user = await _repository.SingleOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetSize(3);
            _memoryCache.Set(cashKey, result, cacheEntryOptions);
            _memoryCache.Clear();
            return _mapper.Map<GetUserDTO>(user);
        }
    }
}
