using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IOfferHandler
    {
        Task<IEnumerable<OfferViewModel>> GetAllAsync();
        Task<OfferViewModel> GetByIdAsync(int id);
        Task<IEnumerable<OfferViewModel>> GetPaginatedAsync(int take = default, int skip = default);
        Task<int> AddAsync(OfferViewModel model);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(OfferViewModel model);
    }
}
