using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IOfferHandler<TId>
        where TId : struct
    {
        Task<IEnumerable<OfferViewModel>> GetAsync(Func<OfferViewModel, bool> filter = null, int skip = 0, int take = 0, string term = "");
        Task<int> AddAsync(OfferViewModel model);
        Task<int> DeleteAsync(TId id);
        Task<int> UpdateAsync(OfferViewModel model);
    }
}
