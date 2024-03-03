using Common.Domain;
using Common.Repositories;
using System.Linq.Expressions;
using System;
using AutoMapper;
using Users.Service.DTO;
using System.Xml.Linq;

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
                _repository.Add(new User { Id = 1, UserName = "Name1", LastUserName = "LastName1"});
                _repository.Add(new User { Id = 2, UserName = "Name2", LastUserName = "LastName2" });
                _repository.Add(new User { Id = 3, UserName = "Name3", LastUserName = "LastName3" });
                _repository.Add(new User { Id = 4, UserName = "Name4", LastUserName = "LastName4" });
                _repository.Add(new User { Id = 5, UserName = "Name5", LastUserName = "LastName5" });
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _repository.GetList();
        }

        public User GetUser(Expression<Func<User, bool>>? predicate = null)
        {
            return _repository.SingleOrDefault(predicate);
        }

        public int GetUserCount()
        {
            return _repository.Count();
        }

        public User GreateUser(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            return _repository.Add(user);
        }

        public User UpdateUser(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            return _repository.Update(user);
        }

        public bool DeleteUser(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            return _repository.Delete(user);
        }
    }
}