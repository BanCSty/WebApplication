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
    public class OrderRepository : IBaseRepository<Order>
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task Create(Order entity)
        {
            await _db.Orders.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Order> Select()
        {
            return _db.Orders;
        }

        public async Task Delete(Guid id)
        {
            var entity = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            _db.Orders.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Order> Update(Order entity)
        {
            _db.Orders.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
