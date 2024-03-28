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
using System.Text.Json.Serialization;
using System.Text.Json;
using Users.Service.DTO;

namespace UserApplication.Queries.GetUserCount
{
    public class GetUserCountQueryHandler : IRequestHandler<GetUserCountQuery, int>
    {
        private readonly IRepository<ApplicationUser> _repository;
        private readonly MemoryCache _memoryCache;

        public GetUserCountQueryHandler(IRepository<ApplicationUser> repository, UsersMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache.Cache;
        }

        public async Task<int> Handle(GetUserCountQuery query, CancellationToken cancellationToken)
        {
            Log.Information("Getting user count.");

            var cashKey = JsonSerializer.Serialize(query, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            if (_memoryCache.TryGetValue(cashKey, out int result))
            {
                return result!;
            }

            var count = await _repository.CountAsync();

            if (count == 0)
            {
                throw new NotFoundException("No ApplicationUsers found.");
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetSize(3);
            _memoryCache.Set(cashKey, result, cacheEntryOptions);
            _memoryCache.Clear();
            return count;
        }
    }
}
