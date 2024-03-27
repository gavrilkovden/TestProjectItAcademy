using AutoMapper;
using Common.Application.Exceptions;
using Common.Domain;
using Common.Application;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Users.Service.DTO;
using Users.Service.Utils;

namespace UserApplication.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, GetUserDTO>
    {
        private readonly IRepository<ApplicationUser> _users;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public CreateUserCommandHandler(
            IRepository<ApplicationUser> users,
            IMapper mapper,
            IMemoryCache memoryCache)
        {
            _users = users;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<GetUserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _users.SingleOrDefaultAsync(u => u.Login == request.Login.Trim(), cancellationToken);

            if (existingUser != null)
            {
                throw new BadRequestException("User login exists");
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Login = request.Login,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Roles = new[] { new ApplicationUserApplicationRole { ApplicationUserRoleId = 2 } }
            };

            await _users.AddAsync(user, cancellationToken);

       //     _memoryCache.Cache.Clear();

            return _mapper.Map<GetUserDTO>(user);
        }
    }
}
