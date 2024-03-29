﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DAL.Interfaces;
using WebShop.Domain.Entity;
using WebShop.Domain.Responce;
using WebShop.Domain.Enum;
using WebShop.Service.Interfaces;
using WebShop.Domain.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Hosting;
using WebShop.Domain.DTO;
using WebShop.Domain.Extensions;
using WebShop.DAL;
using Microsoft.AspNetCore.Hosting;


namespace WebShop.Service.Implementations
{

    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;


        public ProductService(IBaseRepository<Product> productRepository,  AppDbContext db, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _db = db;
            _env = env;
        }

        public async Task<IBaseResponse<List<Product>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.Select().Where(x => x.InStock == true).ToListAsync();

                if(!products.Any())
                {
                    return new BaseResponse<List<Product>>()
                    {
                        DescriptionError = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Product>>()
                {
                    Data = products,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Product>>()
                {
                    DescriptionError = $"[GetProducts] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<ProductViewModel>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.Select().FirstOrDefaultAsync(x => x.Id == id);

                if(product == null)
                return new BaseResponse<ProductViewModel>()
                {
                    DescriptionError = "Найдено 0 элементов",
                    StatusCode = StatusCode.ProductNotFound
                };
 

                var data = new ProductViewModel()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Type = product.Type,
                    Image = product.Image,
                    InStock = product.InStock,
                    CountryName = product.CountryName,
                    Weignt = product.Weignt,
                    Price = product.Price,
                    ManufactureName = product.ManufactureName,
                    CategoryName = product.CategoryName
                };

                return new BaseResponse<ProductViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductViewModel>()
                {
                    DescriptionError = $"[GetProduct] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<List<Product>>> GetProductsByType(string type)
        {
            try
            {
                var product = await _productRepository.Select().Where(x => x.CategoryName == type && x.InStock == true).ToListAsync();

                if (!product.Any())
                    return new BaseResponse<List<Product>>()
                    {
                        DescriptionError = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };


                return new BaseResponse<List<Product>>()
                {
                    StatusCode = StatusCode.OK,
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Product>>()
                {
                    DescriptionError = $"[GetProduct] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<ProductViewModel>> GetProduct(string name)
        {
            try
            {
                var product = await _productRepository.Select().FirstOrDefaultAsync(x => x.Name.ToLower() == name && x.InStock == true);

                if(product == null)
                return new BaseResponse<ProductViewModel>()
                {
                    DescriptionError = "Найдено 0 элементов",
                    StatusCode = StatusCode.ProductNotFound
                };

                var data = new ProductViewModel()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Type = product.Type,
                    Image = product.Image,
                    InStock = product.InStock,
                    CountryName = product.CountryName
                };

                return new BaseResponse<ProductViewModel>()
                {
                    Data = data,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductViewModel>()
                {
                    DescriptionError = $"[GetProductByName] : {ex.Message}",
                    StatusCode = StatusCode.ProductNotFound
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.Select().FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                    return new BaseResponse<bool>()
                    {
                        DescriptionError = "Product not found",
                        StatusCode = StatusCode.ProductNotFound,
                        Data = false
                    };

                product.InStock = false;

                await _productRepository.Delete(product.Id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    DescriptionError = $"[DeleteProduct] : {ex.Message}",
                    StatusCode = StatusCode.ProductNotFound
                };
            }
        }

        public async Task<IBaseResponse<Product>> CreateProduct(ProductCreateViewModel productCreate)
        {
            try
            {
                if (productCreate == null)
                    return new BaseResponse<Product>()
                    {
                        DescriptionError = "Model is null",
                        StatusCode = StatusCode.InternalServer
                    };

                //UPLOAD IMAGE
                var uploadImage = await UploadImage(productCreate.IMG, productCreate.CategoryName);

                if (uploadImage.StatusCode != StatusCode.OK)
                    return new BaseResponse<Product>()
                    {
                        DescriptionError = "File was empty",
                        StatusCode = StatusCode.InternalServer,
                    };

                var product = new Product()
                {
                    Name = productCreate.Name,
                    Type = productCreate.Type,
                    Weignt = productCreate.Weignt,
                    Image =  $"{productCreate.CategoryName}/{uploadImage.Data.FileName}",
                    Description = productCreate.Description,
                    CategoryName = productCreate.CategoryName,
                    ManufactureName = productCreate.ManufactureName,
                    CountryName = productCreate.CountryName,
                    InStock = productCreate.InStock,
                    Price = productCreate.Price
                };

                await _productRepository.Create(product);

                return new BaseResponse<Product>()
                {
                    StatusCode = StatusCode.OK,
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Product>()
                {
                    DescriptionError = $"[CreateProduct] : {ex.Message}",
                    StatusCode = StatusCode.ProductNotFound
                };
            }
        }

        public async Task<IBaseResponse<Product>> Edit(ProductCreateViewModel productViewModel)
        {
            try
            {
                var product = await _productRepository.Select().FirstOrDefaultAsync(x => x.Id == productViewModel.ID);

                if (product == null)
                    return new BaseResponse<Product>
                    {
                        StatusCode = StatusCode.ProductNotFound,
                        DescriptionError = "Product was not found",
                    };


                var uploadImage = await UploadImage(productViewModel.IMG, product.CategoryName);

                product.Name = productViewModel.Name;
                product.Type = productViewModel.Type;
                product.Weignt = productViewModel.Weignt;
                product.Image = uploadImage.StatusCode == StatusCode.OK ? uploadImage.Data.FilePath : product.Image;
                product.Description = productViewModel.Description;
                product.CountryName = productViewModel.CountryName;
                product.CategoryName = productViewModel.CategoryName;
                product.ManufactureName = productViewModel.ManufactureName;
                product.InStock = productViewModel.InStock;

                await _productRepository.Update(product);

                return new BaseResponse<Product>()
                {
                    Data = product,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Product>()
                {
                    DescriptionError = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.ProductNotFound
                };
            }
        }

        private async Task<IBaseResponse<UploadImage>> UploadImage(IFormFile file, string categoryName)
        {
            if (file == null || file.Length == 0)
                return new BaseResponse<UploadImage>()
                {
                    DescriptionError = "File was empty",
                    StatusCode = StatusCode.InternalServer
                };

            try
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "image", categoryName);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //Image upload
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return new BaseResponse<UploadImage>()
                {
                    Data = new UploadImage { FileName = uniqueFileName, FilePath = filePath },
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UploadImage>()
                {
                    DescriptionError = $"[UploadImage] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<BaseResponse<List<Product>>> SearchProduct(string request)
        {
            try
            {
                var products = await _productRepository.Select()
                .Where(p => p.Name.ToLower().Contains(request) || p.Description.ToLower().Contains(request) || p.ManufactureName.Contains(request) || p.Type.Contains(request) || p.CategoryName.Contains(request))
                .ToListAsync();

                if (!products.Any())
                    return new BaseResponse<List<Product>>()
                    {
                        DescriptionError = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };


                return new BaseResponse<List<Product>>()
                {
                    StatusCode = StatusCode.OK,
                    Data = products
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Product>>()
                {
                    DescriptionError = $"[GetProduct] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }



        public async Task<BaseResponse<Dictionary<string, string>>> GetTypes()
        {
            try
            {
                var types = await _db.Categorys.ToListAsync();
                var typesDictionary = types.ToDictionary(type => type.Name.ToString(), type => type.Name.ToString());

                return new BaseResponse<Dictionary<string, string>>()
                {   
                    Data = typesDictionary,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<string, string>>()
                {
                    Data = null,
                    StatusCode = StatusCode.InternalServer,
                    DescriptionError = $"[GetTypes] : {ex}"
                };
            }

        }

        public async Task<BaseResponse<Dictionary<string, string>>> GetCountrys()
        {

            try
            {
                var countrys = await _db.Countrys.ToListAsync();
                var typesDictionary = countrys.ToDictionary(type => type.Name.ToString(), type => type.Name.ToString());

                return new BaseResponse<Dictionary<string, string>>()
                {
                    Data = typesDictionary,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<string, string>>()
                {
                    Data = null,
                    StatusCode = StatusCode.InternalServer,
                    DescriptionError = $"[GetCountrys] : {ex}"
                };
            }
        }

        public async Task<BaseResponse<Dictionary<string, string>>> GetManufactures()
        {
            try
            {
                var manufactures = await _db.Manufactures.ToListAsync();
                var typesDictionary = manufactures.ToDictionary(type => type.Name.ToString(), type => type.Name.ToString());


                return new BaseResponse<Dictionary<string, string>>()
                {
                    Data = typesDictionary,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<string, string>>()
                {
                    Data = null,
                    StatusCode = StatusCode.InternalServer,
                    DescriptionError = $"[GetManufactures] : {ex}"
                };
            }

        }
    }
}
