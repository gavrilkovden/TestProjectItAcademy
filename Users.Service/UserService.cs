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
            try
            {
                Log.Information("Getting all users.");

                var users = await _repository.GetListAsync();

                if (!users.Any())
                {
                    throw new NotFoundException("No Users found.");
                }

                return users;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error getting all users - NotFoundException.");
                throw ;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting all users.");
                throw;
            }
        }

        public async Task<User> GetUserAsync(Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                Log.Information($"Getting user with predicate: {predicate?.ToString() ?? "null"}.");

                var user = await _repository.SingleOrDefaultAsync(predicate);

                if (user == null)
                {
                    throw new NotFoundException("User not found.");
                }

                return user;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, messageTemplate: "Error getting user - NotFoundException.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting user.");
                throw;
            }
        }

    public async Task<int> GetUserCountAsync()
    {
        try
        {
            Log.Information("Getting user count.");

            var count = await _repository.CountAsync();

            if (count == 0)
            {
                throw new NotFoundException("No Users found.");
            }

            return count;
        }
        catch (NotFoundException ex)
        {
            Log.Error(ex, messageTemplate: "Error getting user count - NotFoundException.");
            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex, messageTemplate: "Error getting user count.");
            throw;
        }
    }

    public async Task<User> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        try
        {
            Log.Information($"Creating user: {JsonConvert.SerializeObject(createUserDTO)}");

            if (string.IsNullOrWhiteSpace(createUserDTO.UserName))
            {
                throw new BadRequestException("UserName is required.");
            }

            var user = _mapper.Map<User>(createUserDTO);
            return await _repository.AddAsync(user);
        }
        catch (BadRequestException ex)
        {
            Log.Error(ex, messageTemplate: "Error creating user - BadRequestException.");
            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex, messageTemplate: "Error creating user.");
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(UpdateUserDTO updateUserDTO)
    {
        try
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
        catch (NotFoundException ex)
        {
            Log.Error(ex, messageTemplate: "Error updating user - NotFoundException.");
            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex, messageTemplate: "Error updating user.");
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(UpdateUserDTO updateUserDTO)
    {
        try
        {
            Log.Information($"Deleting user: {JsonConvert.SerializeObject(updateUserDTO)}");

            var existingUser = await GetUserAsync(d => d.Id == updateUserDTO.Id);

            if (existingUser == null)
            {
                throw new NotFoundException("User with specified Id not found.");
            }

            return await _repository.DeleteAsync(existingUser);
        }
        catch (NotFoundException ex)
        {
            Log.Error(ex, messageTemplate: "Error deleting user - NotFoundException.");
            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex, messageTemplate: "Error deleting user.");
            throw;
        }
    }
    }
}