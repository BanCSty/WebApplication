using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Entity;
using WebShop.Domain.Responce;
using WebShop.Domain.ViewModels.Product;

namespace WebShop.Service.Interfaces
{
    public interface IProductService
    {
        public Task<IBaseResponse<List<Product>>> GetProducts();

        public Task<IBaseResponse<ProductViewModel>> GetProduct(Guid id);

        public Task<IBaseResponse<bool>> DeleteProduct(Guid id);

        public Task<IBaseResponse<Product>> CreateProduct(ProductCreateViewModel productViewModel, IFormFile file);

        public Task<IBaseResponse<ProductViewModel>> GetProduct(string name);

        public Task<IBaseResponse<Product>> Edit(ProductCreateViewModel productViewModel, IFormFile file);

        BaseResponse<Dictionary<int, string>> GetTypes();
    }
}
