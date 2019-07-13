using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KoiosOffers.Contracts;
using Microsoft.EntityFrameworkCore;

namespace KoiosOffers.Data
{
    public class Repository<TModel, TId, TDbContext> : IRepository<TModel, TId>
        where TModel : class
        where TId : struct
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private DbSet<TModel> _dbSet;

        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(TModel model)
        {
            _dbContext.Set<TModel>().Add(model);
            int result = await _dbContext.SaveChangesAsync();

            _dbContext.Entry(model).State = EntityState.Detached;

            return result;
        }

        public async Task<int> DeleteAsync(TId id)
        {
            IEnumerable<TModel> result = await GetAsync(o=>((IId<TId>)o).Id.Equals(id));
            TModel entity = result.First();
            _dbContext.Set<TModel>().Remove(entity);
            return await _dbContext.SaveChangesAsync();

            //TModel entityToDelete = await _dbSet.FindAsync(id);

            //if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            //{
            //    _dbSet.Attach(entityToDelete);
            //}
            //_dbSet.Remove(entityToDelete);
            //return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAsync(Expression<Func<TModel, bool>> filter = null)
        {
            IQueryable<TModel> query = _dbContext.Set<TModel>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<int> UpdateAsync(TModel model)
        {
            _dbContext.Set<TModel>().Update(model);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
