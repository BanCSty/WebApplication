using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Auth.Models;
using WebShop.Service.Interfaces;
using Auth.Services.Interfaces;
using Auth.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.AuthenticateAsync(model.Username, model.Password);

            if (user.Data == null)
            {
                return Unauthorized("Invalid username or password");
            }


            HttpContext.Response.Cookies.Append("Bearer", user.Data.JwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Рекомендуется использовать HTTPS
                SameSite = SameSiteMode.Strict, // Рекомендуется использовать SameSite=Strict
                Domain = "localhost", // Замените на ваш домен
                Path = "/", // Путь на сервере, где куки должны быть доступны
                Expires = DateTime.UtcNow.AddDays(1) // Время истечения срока действия куки
            });

            HttpContext.Response.Cookies.Append("RefreshToken", user.Data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Рекомендуется использовать HTTPS
                SameSite = SameSiteMode.Strict, // Рекомендуется использовать SameSite=Strict
                Domain = "localhost", // Замените на ваш домен
                Path = "/", // Путь на сервере, где куки должны быть доступны
                Expires = DateTime.UtcNow.AddDays(1) // Время истечения срока действия куки
            });

            return Ok(new {user.Data.JwtToken, user.Data.RefreshToken });
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel model)
        {

            if (model == null)
                return Unauthorized();

            var refreshToken = await _tokenService.RefreshAsync(model);

            if (refreshToken == null)
                return BadRequest();

            return Ok(new LoginResponse
            {
                JwtToken = refreshToken.JwtToken,
                Expiration = refreshToken.Expiration,
                RefreshToken = refreshToken.RefreshToken
            });
        }    

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null || model.Password != model.ConfirmPassword)
                return BadRequest();

            var reg = await _userService.RegisterAsync(model);

            if (reg.DescriptionError == null)
                return Ok("User successfully created");

            else
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to create user {reg.DescriptionError}");
        }
    }
}
