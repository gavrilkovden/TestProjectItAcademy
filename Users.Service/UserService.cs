using Common.Domain;
using Common.Repositories;
using System.Linq.Expressions;
using System;
using AutoMapper;
using Users.Service.DTO;
using System.Xml.Linq;
using Users.Service.Validators;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Serilog;
using Newtonsoft.Json;
using Common.Domain.Exceptions;

namespace Users.Service

{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        public UserService(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            Log.Information("Getting all users.");

            var users = await _repository.GetListAsync();

            if (!users.Any())
            {
                throw new NotFoundException("No Users found.");
            }

            return users;
        }


        public async Task<User> GetUserAsync(Expression<Func<User, bool>>? predicate = null)
        {
            Log.Information($"Getting user with predicate: {predicate?.ToString() ?? "null"}.");

            var user = await _repository.SingleOrDefaultAsync(predicate);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return user;
        }

        public async Task<int> GetUserCountAsync()
        {
            Log.Information("Getting user count.");

            var count = await _repository.CountAsync();

            if (count == 0)
            {
                throw new NotFoundException("No Users found.");
            }

            return count;
        }

        public async Task<User> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            Log.Information($"Creating user: {JsonConvert.SerializeObject(createUserDTO)}");

            if (string.IsNullOrWhiteSpace(createUserDTO.UserName))
            {
                throw new BadRequestException("UserName is required.");
            }

            var user = _mapper.Map<User>(createUserDTO);
            return await _repository.AddAsync(user);
        }

        public async Task<User> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        {
            Log.Information($"Updating user: {JsonConvert.SerializeObject(updateUserDTO)}");

            var existingUser = await GetUserAsync(d => d.Id == updateUserDTO.Id);

            if (existingUser == null)
            {
                throw new NotFoundException("User with specified Id not found.");
            }

            _mapper.Map(updateUserDTO, existingUser);

            return await _repository.UpdateAsync(existingUser);
        }

        public async Task<bool> DeleteUserAsync(UpdateUserDTO updateUserDTO)
        {
            Log.Information($"Deleting user: {JsonConvert.SerializeObject(updateUserDTO)}");

            var existingUser = await GetUserAsync(d => d.Id == updateUserDTO.Id);

            if (existingUser == null)
            {
                throw new NotFoundException("User with specified Id not found.");
            }

            return await _repository.DeleteAsync(existingUser);
        }
    }
}