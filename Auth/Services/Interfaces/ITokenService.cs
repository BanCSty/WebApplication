using Auth.DAL;
using Auth.Entity;
using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(GenJwtTokenModel user);
        Task<string> GenerateRefreshToken();
        Task<bool> ValidateRefreshTokenAsync(Guid id, string refreshToken);
        Task<bool> UpdateRefrshTokenAsync(Guid id, string token);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<LoginResponse> RefreshAsync(RefreshTokenModel model);
    }
}
