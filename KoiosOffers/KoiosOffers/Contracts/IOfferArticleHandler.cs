using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IOfferArticleHandler<TId>
        where TId : struct
    {
        Task<IEnumerable<OfferArticleViewModel>> GetAsync(Expression<Func<OfferArticleViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "");
        Task<int> AddAsync(OfferArticleViewModel model);
        Task<int> DeleteAsync(TId id);
        Task<int> UpdateAsync(OfferArticleViewModel model);
    }
}
