using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DAL.Interfaces;
using WebShop.Domain.Entity;

namespace WebShop.DAL.Repositories
{
    public class ProductRepository : IBaseRepository<Product>
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task Create(Product entity)
        {
            await _db.Product.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var responce = await _db.Product.FirstOrDefaultAsync(x => x.Id == id);

            responce.InStock = false;
            await _db.SaveChangesAsync();
        }

        public async Task<Product> Get(Guid id)
        {
            return await _db.Product.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Product> Select()
        {
            return _db.Product;
        }

        public async Task<Product> Update(Product entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
