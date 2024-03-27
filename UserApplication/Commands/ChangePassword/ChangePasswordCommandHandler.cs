using AutoMapper;
using Common.Application.Exceptions;
using Common.Domain;
using Common.Application;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserApplication.Commands.CreateUser;
using Users.Service.DTO;
using Users.Service.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserApplication.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, GetUserDTO>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;

        public ChangePasswordCommandHandler(IRepository<ApplicationUser> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetUserDTO> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.SingleOrDefaultAsync(d => d.Id == request.UserId);

            if (existingUser == null)
            {
                throw new NotFoundException("User with specified Id not found.");
            }

            if (existingUser.Roles.Any(role => role.ApplicationUserRole.Name == "Client"))
            {
                throw new ForbiddenException("Client users are not allowed to update their profiles.");

            }

            await _userRepository.UpdateAsync(_mapper.Map(request, existingUser));

            return _mapper.Map<GetUserDTO>(existingUser);
        }
    }
}
