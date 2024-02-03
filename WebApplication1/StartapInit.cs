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

namespace WebApplication
{
    public static class StartapInit
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Product>, ProductRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
