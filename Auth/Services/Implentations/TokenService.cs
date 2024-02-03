using Auth.DAL;
using Auth.Entity;
using Auth.Models;
using Auth.Models.Enum;
using Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Services.Implentations
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _db;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public TokenService(AppDbContext db, IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _db = db;
        }

        public Task<string> GenerateJwtToken(GenJwtTokenModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.Role == Role.Admin ? "Admin" : "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationMinutes),
                Issuer = _jwtSettings.Value.Issuer,
                Audience = _jwtSettings.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[_jwtSettings.Value.RefreshTokenLength];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Task.FromResult(Convert.ToBase64String(randomNumber));
            }
        }

        public async Task<LoginResponse> RefreshAsync(RefreshTokenModel model)
        {
            var principal = GetPrincipalFromExpiredToken(model.AccessToken).Result;

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var login = principal.FindFirst(ClaimTypes.Name).Value;
            var userRole = principal.FindFirst(ClaimTypes.Role).Value;

            if (principal.Identity.Name is null)
                return new LoginResponse { JwtToken = null, RefreshToken = null ,Expiration = default};

            var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            if (refreshToken is null)
                return default;
            if (refreshToken.TokenRefresh != model.RefreshToken)
                return default;
            if (refreshToken.ExpirationDate <= DateTime.UtcNow)
                return default;

            var jwtTokenModel = new GenJwtTokenModel()
            {
                Id = Guid.Parse(userId),
                Login = login,
                Role = userRole == "Admin" ? Role.Admin : Role.User
            };

            var token = GenerateJwtToken(jwtTokenModel).Result;

            return new LoginResponse()
            {
                JwtToken = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationMinutes),
                RefreshToken = model.RefreshToken
            };
        }

        public async Task<bool> UpdateRefrshTokenAsync(Guid id, string token)
        {
    
            if (id == Guid.Empty || token == null)
                return false;
            try
            {
                var user = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id);
                var updateUserToken = new RefreshToken()
                {
                    UserId = user.UserId,
                    TokenRefresh = token,
                    ExpirationDate = DateTime.Now.AddDays(7),
                };

                _db.Remove(user);
                await _db.AddAsync(updateUserToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            // Поиск Refresh токена пользователя
            var latestToken = await _db.RefreshTokens
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id) // Сортировка по Id в обратном порядке
                .FirstOrDefaultAsync();

            // Проверка найденного токена
            if (latestToken != null && latestToken.TokenRefresh == refreshToken && latestToken.ExpirationDate > DateTime.UtcNow)
            {
                return true; // Токен найден и действителен
            }

            return false; // Токен не найден или истек
        }


        public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var secret = _jwtSettings.Value.Secret ?? throw new InvalidOperationException("Secret not configured");

            var validation = new TokenValidationParameters
            {
                ValidIssuer = _jwtSettings.Value.Issuer,
                ValidAudience = _jwtSettings.Value.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false
            };

            return Task.FromResult(new JwtSecurityTokenHandler().ValidateToken(token, validation, out _));
        }
    }
}
