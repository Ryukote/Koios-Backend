using KoiosOffers.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IOfferHandler
    {
        Task<IEnumerable<OfferViewModel>> GetAllAsync();
        Task<OfferViewModel> GetByIdAsync(int id);
        Task<IEnumerable<OfferViewModel>> GetPaginatedAsync(int offerNumber = default, int take = default, int skip = default);
        Task<IEnumerable<ArticleViewModel>> GetOfferArticlesByIdAsync(int offerId);
        Task<OfferViewModel> GetOfferByOfferNumberAsync(int offerNumber);
        Task<int> AddAsync(OfferViewModel model);
        Task<int> AddOfferArticleAsync(int offerId, int articleId);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteOfferArticle(int offerId, int articleId);
        Task<int> UpdateAsync(OfferViewModel model);
    }
}
