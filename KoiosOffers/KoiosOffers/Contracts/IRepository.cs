using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KoiosOffers.Models;

namespace KoiosOffers.Contracts
{
    public interface IRepository<TId>
        where TId : struct
    {
        Task<IEnumerable<IViewModel>> GetAsync(Expression<Func<IViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "");
        Task<int> AddAsync(IViewModel model);
        Task<int> DeleteAsync(TId id);
        Task<int> UpdateAsync(IViewModel model);
    }
}
