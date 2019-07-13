using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IRepository<TModel, TId>
        where TModel : class
        where TId : struct
    {
        Task<IEnumerable<TModel>> GetAsync(Expression<Func<TModel, bool>> filter = null);
        Task<int> AddAsync(TModel model);
        Task<int> DeleteAsync(TId id);
        Task<int> UpdateAsync(TModel model);
    }
}
