using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebShop.Domain.ViewModels.Product;
using WebShop.Service.Interfaces;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var response = await _productService.GetProduct(id);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProductsByType(string id)
        {
            var response = await _productService.GetProductsByType(id);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var response = await _productService.DeleteProduct(id);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetProducts");
            }

            return View("Error", $"{response.DescriptionError}");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveProduct(Guid id)
        {
            if (id == Guid.Empty)
                return PartialView();

            var response = await _productService.GetProduct(id);

            if (response.StatusCode == WebShop.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            ModelState.AddModelError("", response.DescriptionError);
            return PartialView();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveProduct(ProductCreateViewModel productViewModel)
        {
            ModelState.Remove("Id");
            ModelState.Remove("DateCreate");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error");
            }

            var product = await _productService.GetProduct(productViewModel.ID);

            if (product.Data == null)
            {
                await _productService.CreateProduct(productViewModel);
            }


            else
                await _productService.Edit(productViewModel);

            return RedirectToAction("GetProducts");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SearchString(string searchString)
        {
            if (searchString == null)
                return NotFound();

            var products = await _productService.SearchProduct(searchString.ToLower());


            return View("GetProducts", products.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult GetTypes()
        {
            var types = _productService.GetTypes().Result;
            return Json(types.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult GetCountrys()
        {
            var types = _productService.GetCountrys().Result;
            return Json(types.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult GetManufactures()
        {
            var types = _productService.GetManufactures().Result;
            return Json(types.Data);
        }
    }
}
