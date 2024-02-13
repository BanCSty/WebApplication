using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.DAL.Interfaces;
using WebShop.DAL.Repositories;
using WebShop.Domain.Entity;
using WebShop.Service.Implementations;
using WebShop.Service.Interfaces;
using WebShop.Service.Middleware;
using WebShop.Services.Implentations;
using WebShop.Services.Interfaces;

namespace WebApplication
{
    public static class StartapInit
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Product>, ProductRepository>();
            services.AddScoped<IBaseRepository<User>, AccountRepository>();
            services.AddScoped<IBaseRepository<Basket>, BasketRepository>();
            services.AddScoped<IBaseRepository<Order>, OrderRepository>();

        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
