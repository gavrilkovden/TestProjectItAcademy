//using Common.Domain;
//using Common.Application;
//using System.Linq.Expressions;
//using AutoMapper;
//using Serilog;
//using Newtonsoft.Json;
//using Users.Service.Utils;
//using Common.Application.Exceptions;

//namespace Users.Service

//{
//    public class UserService : IUserService
//    {
//        private readonly IRepository<ApplicationUser> _repository;
//        private readonly IMapper _mapper;
//        private readonly IRepository<ApplicationUserRole> _userRoles;

//        public UserService(IRepository<ApplicationUser> repository, IRepository<ApplicationUserRole> userRoles, IMapper mapper)
//        {
//            _repository = repository;
//            _mapper = mapper;
//            _userRoles = userRoles;
//        }


//        public async Task<IEnumerable<GetUserDTO>> GetAllUsersAsync()
//        {
//            Log.Information("Getting all users.");

//            var users = await _repository.GetListAsync();

//            if (!users.Any())
//            {
//                throw new NotFoundException("No ApplicationUsers found.");
//            }

//            return _mapper.Map<IEnumerable<GetUserDTO>>(users);
//        }


//        public async Task<GetUserDTO> GetUserAsync(Expression<Func<ApplicationUser, bool>>? predicate = null, CancellationToken cancellationToken = default)
//        {
//            Log.Information($"Getting user with predicate: {predicate?.ToString() ?? "null"}.");

//            var user = await _repository.SingleOrDefaultAsync(predicate, cancellationToken);

//            if (user == null)
//            {
//                throw new NotFoundException("User not found.");
//            }

//            return _mapper.Map<GetUserDTO>(user);
//        }

//        public async Task<int> GetUserCountAsync()
//        {
//            Log.Information("Getting user count.");

//            var count = await _repository.CountAsync();

//            if (count == 0)
//            {
//                throw new NotFoundException("No ApplicationUsers found.");
//            }

//            return count;
//        }

//        public async Task<GetUserDTO> CreateUserAsync(CreateUserDTO createUserDTO, CancellationToken cancellationToken = default)
//        {
//            Log.Information($"Creating user: {JsonConvert.SerializeObject(createUserDTO)}");

//            if (string.IsNullOrWhiteSpace(createUserDTO.UserName))
//            {
//                throw new BadRequestException("UserName is required.");
//            }
//            if (await _repository.SingleOrDefaultAsync(u => u.Login == createUserDTO.Login.Trim(), cancellationToken) != null)
//            {
//                throw new BadRequestException("User login exists");
//            }

//            var userRole = await _userRoles.SingleOrDefaultAsync(r => r.Name == "Client", cancellationToken);

//            var entity = new ApplicationUser()
//            {
//                Login = createUserDTO.Login,
//                PasswordHash = PasswordHasher.HashPassword(createUserDTO.Password),
//                UserName = createUserDTO.UserName,
//                Roles = new[] { new ApplicationUserApplicationRole() { ApplicationUserRoleId = userRole.Id } }
//                };


//            var createdUser = await _repository.AddAsync(entity);
//            return _mapper.Map<GetUserDTO>(createdUser);
//        }

      
//        public async Task<GetUserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO)
//        {
//            Log.Information($"Updating user: {JsonConvert.SerializeObject(updateUserDTO)}");

//            var existingUser = await _repository.SingleOrDefaultAsync(d => d.Id == updateUserDTO.Id);

//            if (existingUser == null)
//            {
//                throw new NotFoundException("User with specified Id not found.");
//            }

//            if (existingUser.Roles.Any(role => role.ApplicationUserRole.Name == "Client"))
//            {
//                throw new ForbiddenException("Client users are not allowed to update their profiles.");
                
//            }

//            await _repository.UpdateAsync(_mapper.Map(updateUserDTO, existingUser));

//            return _mapper.Map<GetUserDTO>(existingUser);
//        }

//        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
//        {
//            var user = await _repository.SingleOrDefaultAsync(u => u.Id == userId);
//            if (user == null)
//            {
//                throw new NotFoundException("User not found.");
//            }

//            user.PasswordHash = PasswordHasher.HashPassword(newPassword);

//            await _repository.UpdateAsync(user);

//            return true;
//        }

//        public async Task<bool> DeleteUserAsync(UpdateUserDTO updateUserDTO)
//        {
//            Log.Information($"Deleting user: {JsonConvert.SerializeObject(updateUserDTO)}");

//            var existingUser = await _repository.SingleOrDefaultAsync(d => d.Id == updateUserDTO.Id);

//            if (existingUser == null)
//            {
//                throw new NotFoundException("User with specified Id not found.");
//            }

//            return await _repository.DeleteAsync(existingUser);
//        }
//    }
//}