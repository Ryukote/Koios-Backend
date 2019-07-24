using KoiosOffers.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IOfferHandler
    {
        Task<OfferViewModel> GetByIdAsync(int id);
        Task<IEnumerable<ArticleViewModel>> GetOfferArticlesByIdAsync(int offerId);
        Task<int> AddAsync(OfferViewModel model);
        Task<int> AddOfferArticleAsync(int offerId, int articleId);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteOfferArticle(int offerId, int articleId);
    }
}
