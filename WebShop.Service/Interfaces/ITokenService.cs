using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebShop.Domain.ViewModels.Account;

namespace WebShop.Services.Interfaces
{
    public interface ITokenService
    {
        //Генерация access токена
        Task<string> GenerateJwtToken(GetJwtTokenModel user);

        //Генерация refresh токена
        Task<string> GenerateRefreshToken();

        //Проверка refresh токена 
        Task<bool> ValidateRefreshTokenAsync(Guid id, string refreshToken);
        
        //Получение данных из токена (id/login/role)
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);

        Task<ClaimsPrincipal> GetPrincipalWithoutExeptionFromExpiredToken(string token);

        //Обновление access токена
        Task<LoginResponse> RefreshAsync(RefreshTokenModel model);
    }
}
