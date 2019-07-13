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

        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(TModel model)
        {
            _dbContext.Set<TModel>().Add(model);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(TId id)
        {
            var entity = await GetByAsync(o => ((IId<TId>)o).Id.Equals(id));
            _dbContext.Set<TModel>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _dbContext.Set<TModel>().ToListAsync();
        }

        public async Task<TModel> GetByAsync(Expression<Func<TModel, bool>> expression)
        {
            return await _dbContext.Set<TModel>().FirstAsync(expression);
        }

        public async Task<int> UpdateAsync(TModel model)
        {
            _dbContext.Set<TModel>().Update(model);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
