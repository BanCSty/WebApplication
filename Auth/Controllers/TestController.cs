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
using System.Collections.Generic;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [Authorize]
        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            var user = new List<int>() { 1, 2, 3, 4 };


            return Ok(new { user.Count });
        }
    }  
}
