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
    public class BasketRepository : IBaseRepository<Basket>
    {
        private readonly AppDbContext _db;

        public BasketRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(Basket entity)
        {
            await _db.Baskets.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Basket> Select()
        {
            return _db.Baskets;
        }

        public async Task Delete(Guid id)
        {
            var entity = await _db.Baskets.FirstOrDefaultAsync(x => x.Id == id);
            _db.Baskets.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Basket> Update(Basket entity)
        {
            _db.Baskets.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
