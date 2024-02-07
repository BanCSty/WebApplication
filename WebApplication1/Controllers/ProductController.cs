using Microsoft.AspNetCore.Mvc;
using System;

using System.Threading.Tasks;

using WebShop.Service.Interfaces;
using WebShop.Domain.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _productService.GetProducts();

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var response = await _productService.GetProduct(id);

            if(response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var response = await _productService.GetProduct(name);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var response = await _productService.DeleteProduct(id);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveProduct(Guid id)
        {
            if (id == Guid.Empty)
                return View();

            var response = await _productService.GetProduct(id);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveProduct(ProductCreateViewModel productViewModel, IFormFile file)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Error");
            }

            if (_productService.GetProduct(productViewModel.ID) == null)
                await _productService.CreateProduct(productViewModel, file);

            else
                await _productService.Edit(productViewModel, file);

            return RedirectToAction("GetProducts");
        }

        [HttpPost]
        public JsonResult GetTypes()
        {
            var types = _productService.GetTypes();
            return Json(types.Data);
        }
    }
}
