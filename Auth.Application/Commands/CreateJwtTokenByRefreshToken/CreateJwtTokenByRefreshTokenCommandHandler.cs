using Auth.Application.Commands.CreateJwtToken;
using Auth.Application.DTO;
using Common.Application;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Commands.CreateJwtTokenByRefreshToken
{
    public class CreateJwtTokenByRefreshTokenCommandHandler : IRequestHandler<CreateJwtTokenByRefreshTokenCommand, JwtTokenDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<RefreshToken> _refreshTokens;

        public CreateJwtTokenByRefreshTokenCommandHandler(
            IRepository<RefreshToken> refreshTokens,
            IConfiguration configuration,
            IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
            _refreshTokens = refreshTokens;
            _configuration = configuration;
        }

        public async Task<JwtTokenDto> Handle(CreateJwtTokenByRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokens.SingleOrDefaultAsync(e => e.Id == request.RefreshToken, cancellationToken);

            if (refreshToken is null)
            {
                throw new ForbiddenException(nameof(request.RefreshToken));
            }

            var user = await _userRepository.SingleAsync(u => u.Id == refreshToken.ApplicationUserId, cancellationToken);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login),
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


            return new JwtTokenDto()
            {
                JwtToken = token,
                RefreshToken = refreshToken.Id,
                Expires = dataExpires,
            };
        }
    }
}
