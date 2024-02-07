using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BCrypt.Net;
using WebApplication.Models;
using System.Net.Http.Headers;
using WebShop.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AccountController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var response = await _userService.AuthenticateAsync(username, password);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {             
                HttpContext.Response.Cookies.Append("Bearer", response.Data.JwtToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Рекомендуется использовать HTTPS
                    SameSite = SameSiteMode.Strict, // Рекомендуется использовать SameSite=Strict

                    Expires = DateTime.UtcNow.AddMinutes(15) // Время истечения срока действия куки
                });

                HttpContext.Response.Cookies.Append("RefreshToken", response.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Рекомендуется использовать HTTPS
                    SameSite = SameSiteMode.Strict, // Рекомендуется использовать SameSite=Strict

                    Expires = DateTime.UtcNow.AddDays(1) // Время истечения срока действия куки
                });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Если аутентификация не удалась, отобразим сообщение об ошибке на странице входа
                ViewData["ErrorMessage"] = "Неправильное имя пользователя или пароль.";
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null || model.Password != model.PasswordConfirm)
                return BadRequest();

            var response = await _userService.RegisterAsync(model);     

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                // Если аутентификация прошла успешно, перенаправим пользователя на главную страницу магазина
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Если аутентификация не удалась, отобразим сообщение об ошибке на странице входа
                ViewData["ErrorMessage"] = "Ошибка реггистрации";
                return View("Register");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            var response = await _tokenService.RefreshAsync(model);

            if (response != null)
            {
                HttpContext.Response.Cookies.Append("Bearer", response.JwtToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Рекомендуется использовать HTTPS
                    SameSite = SameSiteMode.Strict, // Рекомендуется использовать SameSite=Strict

                    Expires = DateTime.UtcNow.AddMinutes(1) // Время истечения срока действия куки
                });

                HttpContext.Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Рекомендуется использовать HTTPS
                    SameSite = SameSiteMode.Strict, // Рекомендуется использовать SameSite=Strict

                    Expires = DateTime.UtcNow.AddDays(1) // Время истечения срока действия куки
                });
                return Ok();
            }
            else
            {
                // Если аутентификация не удалась, отобразим сообщение об ошибке на странице входа
                ViewData["ErrorMessage"] = "Неправильное имя пользователя или пароль.";
                return View("Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }
    }
}
