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
        public Task<IEnumerable<User>> GetAllUsersAsync();

        public Task<User> GetUserAsync(Expression<Func<User, bool>>? predicate = null);

        public Task<int> GetUserCountAsync();

        public Task<User> CreateUserAsync(CreateUserDTO userDTO);

        public Task<User> UpdateUserAsync(UpdateUserDTO userDTO);

        public Task<bool> DeleteUserAsync(UpdateUserDTO updateUserDTO);
    }
}
