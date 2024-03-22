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
        public Task<IEnumerable<GetUserDTO>> GetAllUsersAsync();

        public Task<GetUserDTO> GetUserAsync(Expression<Func<ApplicationUser, bool>>? predicate = null, CancellationToken cancellationToken = default);

        public Task<int> GetUserCountAsync();

        public Task<GetUserDTO> CreateUserAsync(CreateUserDTO userDTO, CancellationToken cancellationToken = default);

        public Task<GetUserDTO> UpdateUserAsync(UpdateUserDTO userDTO);

        public Task<bool> DeleteUserAsync(UpdateUserDTO updateUserDTO);

        public  Task<bool> ChangePasswordAsync(int userId, string newPassword);
    }
}
