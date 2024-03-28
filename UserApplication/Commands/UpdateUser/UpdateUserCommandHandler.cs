using AutoMapper;
using Common.Application.Exceptions;
using Common.Domain;
using Common.Application;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApplication.Commands.CreateUser;
using Users.Service.DTO;

namespace UserApplication.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserDTO>
    {
        private readonly IRepository<ApplicationUser> _users;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            IRepository<ApplicationUser> users,
            IMapper mapper)
        {
            _users = users;
            _mapper = mapper;
        }

        public async Task<GetUserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _users.SingleOrDefaultAsync(d => d.Id == request.Id);

            if (existingUser == null)
            {
                throw new NotFoundException("User with specified Id not found.");
            }

            await _users.UpdateAsync(_mapper.Map(request, existingUser));

            return _mapper.Map<GetUserDTO>(existingUser);
        }
    }
}
