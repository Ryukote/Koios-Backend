using KoiosOffers.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IArticleHandler
    {
        Task<ArticleViewModel> GetByIdAsync(int id);
        Task<IEnumerable<ArticleViewModel>> GetPaginatedAsync(string name = default, int take = default, int skip = default);
        Task<int> AddAsync(ArticleViewModel model);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(ArticleViewModel model);
    }
}
