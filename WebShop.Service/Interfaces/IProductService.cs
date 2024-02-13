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

        public Task<IBaseResponse<Product>> CreateProduct(ProductCreateViewModel productViewModel);

        public Task<IBaseResponse<ProductViewModel>> GetProduct(string name);

        public Task<IBaseResponse<Product>> Edit(ProductCreateViewModel productViewModel);

        public Task<IBaseResponse<List<Product>>> GetProductsByType(string type);

        public Task<BaseResponse<List<Product>>> SearchProduct(string request);

        public Task<BaseResponse<Dictionary<string, string>>> GetTypes();
        public Task<BaseResponse<Dictionary<string, string>>> GetCountrys();
        public Task<BaseResponse<Dictionary<string, string>>> GetManufactures();
    }
}
