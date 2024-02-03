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

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClient();
            var authUrl = "https://localhost:44308/api/Auth/Login";

            //Объект, содержащий данные пользователя для отправки на сервер аутентификации
            var data = new LoginModel{ Username = username, Password = password /*BCrypt.Net.BCrypt.HashPassword(password)*/ };

            // Отправка запроса на сервер аутентификации
            var response = await client.PostAsJsonAsync(authUrl, data);

            if (response.IsSuccessStatusCode)
            {
                // Если аутентификация прошла успешно, перенаправим пользователя на главную страницу магазина
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Headers.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Если аутентификация не удалась, отобразим сообщение об ошибке на странице входа
                ModelState.AddModelError(string.Empty, "Неправильное имя пользователя или пароль.");
                return View("Login");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var authUrl = "https://localhost:44308/api/Auth/Register";

            if (model == null || model.Password != model.PasswordConfirm)
                return BadRequest();

            var hashPass = BCrypt.Net.BCrypt.HashPassword(model.PasswordConfirm);

            //Объект, содержащий данные пользователя для отправки на сервер аутентификации
            var data = new RegisterViewModel { Name = model.Name, Password = hashPass, PasswordConfirm = hashPass };

            // Отправка запроса на сервер аутентификации
            var response = await client.PostAsJsonAsync(authUrl, data);

            if (response.IsSuccessStatusCode)
            {
                // Если аутентификация прошла успешно, перенаправим пользователя на главную страницу магазина
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Если аутентификация не удалась, отобразим сообщение об ошибке на странице входа
                ModelState.AddModelError(string.Empty, "Registration faild");
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
