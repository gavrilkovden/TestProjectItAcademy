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


            if (_repository.Count() == 0)
            {
                _repository.Add(new User { Id = 1, UserName = "Name1" });
                _repository.Add(new User { Id = 2, UserName = "Name2" });
                _repository.Add(new User { Id = 3, UserName = "Name3" });
                _repository.Add(new User { Id = 4, UserName = "Name4" });
                _repository.Add(new User { Id = 5, UserName = "Name5" });
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                Log.Information("Getting all users.");

                return _repository.GetList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting all users.");
                throw;
            }
        }

        public User GetUser(Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                Log.Information($"Getting user with predicate: {predicate?.ToString() ?? "null"}.");

                return _repository.SingleOrDefault(predicate);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting user.");
                throw;
            }
        }

        public int GetUserCount()
        {
            try
            {
                Log.Information("Getting user count.");

                return _repository.Count();
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error getting user count.");
                throw;
            }
        }

        public User GreateUser(CreateUserDTO createUserDTO)
        {
            try
            {
                Log.Information($"Creating user: {JsonConvert.SerializeObject(createUserDTO)}");

                var user = _mapper.Map<User>(createUserDTO);
                return _repository.Add(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error creating user.");
                throw;
            }
        }

        public User UpdateUser(UpdateUserDTO updateUserDTO)
        {
            try
            {
                Log.Information($"Updating user: {JsonConvert.SerializeObject(updateUserDTO)}");

                var existingUser = GetUser(d => d.Id == updateUserDTO.Id);

                if (existingUser == null)
                {
                    throw new Exception("User with specified Id not found.");
                }
                _mapper.Map(updateUserDTO, existingUser);

                return _repository.Update(existingUser);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error updating user.");
                throw;
            }
        }

        public bool DeleteUser(UpdateUserDTO updateUserDTO)
        {
            try
            {
                Log.Information($"Deleting user: {JsonConvert.SerializeObject(updateUserDTO)}");

                var existingUser = GetUser(d => d.Id == updateUserDTO.Id);

                if (existingUser == null)
                {
                    throw new Exception("User with specified Id not found.");
                }
                return _repository.Delete(existingUser);
            }
            catch (Exception ex)
            {
                Log.Error(ex, messageTemplate: "Error deleting user.");
                throw;
            }
        }
    }
}