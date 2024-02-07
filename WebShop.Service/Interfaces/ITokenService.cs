using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebShop.Domain.ViewModels.Account;

namespace WebShop.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(GetJwtTokenModel user);
        Task<string> GenerateRefreshToken();
        Task<bool> ValidateRefreshTokenAsync(Guid id, string refreshToken);
        Task<bool> UpdateRefrshTokenAsync(Guid id, string token);

        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<ClaimsPrincipal> GetPrincipalWithoutExeptionFromExpiredToken(string token);

        Task<LoginResponse> RefreshAsync(RefreshTokenModel model);
    }
}
