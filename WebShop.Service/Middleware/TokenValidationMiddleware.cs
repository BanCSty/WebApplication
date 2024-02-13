using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.ViewModels.Account;
using WebShop.Services.Interfaces;

namespace WebShop.Service.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenService _tokenService)
        {
            try
            {

                var token = context.Request.Cookies["Bearer"];

                if (token != null)
                {

                        // Проверяем валидность токена
                        var isValidToken = await _tokenService.GetPrincipalFromExpiredToken(token);

                        // Если токен действителен, передаем управление следующему Middleware
                        await _next(context);
                    return;

                }
                await _next(context);
                return;
            }
            catch (SecurityTokenValidationException)
            {
                var refreshToken = context.Request.Cookies["RefreshToken"];
                var jwtToken = context.Request.Cookies["Bearer"];

                if (refreshToken != null)
                {
                        var isValidToken = await _tokenService.GetPrincipalWithoutExeptionFromExpiredToken(jwtToken);

                        var idUserClaim = isValidToken.FindFirst(ClaimTypes.NameIdentifier).Value;

                        // Проверяем валидность токена
                        var validRefreshToken = await _tokenService.ValidateRefreshTokenAsync(Guid.Parse(idUserClaim), refreshToken);

                        if (validRefreshToken)
                        {
                            var updateTokens = await _tokenService.RefreshAsync(new RefreshTokenModel { AccessToken = jwtToken, RefreshToken = refreshToken });

                            context.Response.Cookies.Append("Bearer", updateTokens.JwtToken, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true, 
                                SameSite = SameSiteMode.Strict, 

                                Expires = DateTime.UtcNow.AddHours(2) // Время истечения срока действия куки
                            });

                            await _next(context);
                            return;
                        }
                        else
                        {
                            context.Response.StatusCode = 401; // Unauthorized
                            await context.Response.WriteAsync("Unauthorized");
                            return;
                        }
                    
                }
                await _next(context);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        }
    }
}
