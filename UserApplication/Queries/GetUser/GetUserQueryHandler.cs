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

namespace UserApplication.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserDTO>
    {
        private readonly IRepository<ApplicationUser> _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public GetUserQueryHandler(IRepository<ApplicationUser> repository, IMapper mapper, IMemoryCache memoryCache)
        {
            _repository = repository;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<GetUserDTO> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            Log.Information($"Getting user with predicate: {query.UserId.ToString() ?? "null"}.");

            var user = await _repository.SingleOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
    //        _memoryCache.Cache.Clear();
            return _mapper.Map<GetUserDTO>(user);
        }
    }
}
