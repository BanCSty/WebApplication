using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Domain.ViewModels.Order;
using WebShop.Service.Interfaces;

namespace WebApplication.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult CreateOrder(Guid id)
        {
            var orderModel = new CreateOrderViewModel()
            {
                ProductId = id,
                Login = User.Identity.Name,
                Quantity = 0
            };
            return View(orderModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _orderService.Create(model);
                if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.DescriptionError });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _orderService.Delete(id);
            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Error", $"{response.DescriptionError}");
        }
    }
}
