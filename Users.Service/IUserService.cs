using Common.Domain;
using Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;

namespace Users.Service
{
    public interface IUserService
    {
        public IEnumerable<User> GetAllUsers();

        public User GetUser(Expression<Func<User, bool>>? predicate = null);

        public int GetUserCount();

        public User GreateUser(CreateUserDTO userDTO);

        public User UpdateUser(UpdateUserDTO userDTO);

        public bool DeleteUser(UpdateUserDTO updateUserDTO);
    }
}
