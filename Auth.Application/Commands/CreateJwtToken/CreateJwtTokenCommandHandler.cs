using Auth.Application.DTO;
using AutoMapper;
using Common.Application;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Service.Utils;

namespace Auth.Application.Commands.CreateJwtToken
{
    public class CreateJwtTokenCommandHandler : IRequestHandler<CreateJwtTokenCommand, JwtTokenDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<RefreshToken> _refreshTokens;

        public CreateJwtTokenCommandHandler(
            IRepository<RefreshToken> refreshTokens,
            IConfiguration configuration,
            IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
            _refreshTokens = refreshTokens;
            _configuration = configuration;
        }

        public async Task<JwtTokenDto> Handle(CreateJwtTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.SingleOrDefaultAsync(u => u.Login == request.Login.Trim(), cancellationToken);
            if (user is null)
            {
                throw new NotFoundException("user not found", filter: $"User width login {request.Login} don't exist");
            }


            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ForbiddenException("");
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.Login),
            new Claim(ClaimTypes.NameIdentifier, value: user.Id.ToString()),
        };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name));
            }

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            // Создаем учетные данные для подписи токена
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            var dataExpires = DateTime.UtcNow.AddMinutes(5);
            // Создаем описание токена
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: dataExpires,
                signingCredentials: credentials);

            // Создаем JWT токен
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            var refreshToken = await _refreshTokens.AddAsync(new RefreshToken { ApplicationUserId = user.Id }, cancellationToken);

            return new JwtTokenDto()
            {
                JwtToken = token,
                RefreshToken = refreshToken.Id,
                Expires = dataExpires,
            };
        }
    }
}
