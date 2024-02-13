using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Service.Interfaces;

namespace WebApplication.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            var response = await _basketService.GetItems(User.Identity.Name);
            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data.ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var response = await _basketService.GetItem(User.Identity.Name, id);
            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
