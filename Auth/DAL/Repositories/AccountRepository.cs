using Auth.DAL.Interfaces;
using Auth.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.Repositories
{
    public class AccountRepository : IBaseRepository<User>
    {
        private readonly AppDbContext _db;

        public AccountRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task Create(User entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id);
            _db.RefreshTokens.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<User> Select()
        {
            throw new NotImplementedException();
        }

        public Task<User> Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
