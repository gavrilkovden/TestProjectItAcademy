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

namespace UserApplication.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IRepository<ApplicationUser> _users;

        public DeleteUserCommandHandler(IRepository<ApplicationUser> users)
        {
            _users = users;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _users.SingleOrDefaultAsync(d => d.Id == request.Id);

            if (existingUser == null)
            {
                throw new NotFoundException("User with specified Id not found.");
            }

            return await _users.DeleteAsync(existingUser);

        }
    }
}
