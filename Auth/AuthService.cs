//using Auth.DTO;
//using Common.Domain;
//using Common.Domain.Exceptions;
//using Common.Application;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Users.Service.DTO;
//using Users.Service.Utils;

//namespace Users.Service
//{
//    public class AuthService : IAuthService
//    {
//        private readonly IRepository<ApplicationUser> _userRepository;
//        private readonly IConfiguration _configuration;
//        private readonly IRepository<RefreshToken> _refreshTokens;


//        public AuthService(IRepository<ApplicationUser> userRepository, IRepository<RefreshToken> refreshTokens, IConfiguration configuration)
//        {
//            _userRepository = userRepository;
//            _configuration = configuration;
//            _refreshTokens = refreshTokens;

//        }

//        public async Task<JwtTokenDto> GetJwtTokenAsync(AuthDTO authDTO, CancellationToken cancellationToken = default)
//        {
//            var user = await _userRepository.SingleOrDefaultAsync(u => u.Login == authDTO.Login.Trim(), cancellationToken);
//            if (user is null)
//            {
//                throw new NotFoundException("user not found", filter: $"User width login {authDTO.Login} don't exist");
//            }


//            if (!PasswordHasher.VerifyPassword(authDTO.Password, user.PasswordHash))
//            {
//                throw new ForbiddenException("");
//            }

//            var claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.Name, authDTO.Login), 
//            new Claim(ClaimTypes.NameIdentifier, value: user.Id.ToString()), 
//        };

//            foreach (var role in user.Roles)
//            {
//                claims.Add(new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name));
//            }

//            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

//            // Создаем учетные данные для подписи токена
//            var securityKey = new SymmetricSecurityKey(key);
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


//            var dataExpires = DateTime.UtcNow.AddMinutes(5);
//            // Создаем описание токена
//            var tokenDescriptor = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                claims: claims,
//                expires: dataExpires,
//                signingCredentials: credentials);

//            // Создаем JWT токен
//            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//            var refreshToken = await _refreshTokens.AddAsync(new RefreshToken { ApplicationUserId =  user.Id }, cancellationToken);

//            return new JwtTokenDto()
//            {
//                JwtToken = token,
//                RefreshToken = refreshToken.Id,
//                Expires = dataExpires,
//            };

//        }

//        public async Task<JwtTokenDto> GetJwtTokenByRefreshTokenAsync(string refreshtoken, CancellationToken cancellationToken = default)
//        {
//            var refreshToken = await _refreshTokens.SingleOrDefaultAsync(e => e.Id == refreshtoken, cancellationToken);

//            if (refreshToken is null)
//            {
//                throw new ForbiddenException(nameof(refreshtoken));
//            }

//            var user = await _userRepository.SingleAsync(u => u.Id == refreshToken.ApplicationUserId, cancellationToken);

//            var claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.Name, user.Login),
//            new Claim(ClaimTypes.NameIdentifier, value: user.Id.ToString()),
//        };

//            foreach (var role in user.Roles)
//            {
//                claims.Add(new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name));
//            }

//            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

//            // Создаем учетные данные для подписи токена
//            var securityKey = new SymmetricSecurityKey(key);
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


//            var dataExpires = DateTime.UtcNow.AddMinutes(5);
//            // Создаем описание токена
//            var tokenDescriptor = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                claims: claims,
//                expires: dataExpires,
//                signingCredentials: credentials);

//            // Создаем JWT токен
//            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);


//            return new JwtTokenDto()
//            {
//                JwtToken = token,
//                RefreshToken = refreshToken.Id,
//                Expires = dataExpires,
//            };
//        }
//    }
//}



