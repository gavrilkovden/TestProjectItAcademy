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

namespace UserApplication.Queries.GetUserCount
{
    public class GetUserCountQueryHandler : IRequestHandler<GetUserCountQuery, int>
    {
        private readonly IRepository<ApplicationUser> _repository;
        private readonly IMemoryCache _memoryCache;

        public GetUserCountQueryHandler(IRepository<ApplicationUser> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public async Task<int> Handle(GetUserCountQuery query, CancellationToken cancellationToken)
        {
            Log.Information("Getting user count.");

            var count = await _repository.CountAsync();

            if (count == 0)
            {
                throw new NotFoundException("No ApplicationUsers found.");
            }
        //    _memoryCache.Cache.Clear();
            return count;
        }
    }
}
