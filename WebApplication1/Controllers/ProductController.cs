using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.DAL.Interfaces;
using WebShop.Domain.Entity;
using WebShop.Domain;
using WebShop.Service.Interfaces;
using WebShop.Domain.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
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
        //[Authorize(Roles = "Admin")]
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
