using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IArticleHandler<TId>
        where TId : struct
    {
        Task<IEnumerable<ArticleViewModel>> GetAsync(Expression<Func<ArticleViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "");
        Task<int> AddAsync(ArticleViewModel model);
        Task<int> DeleteAsync(TId id);
        Task<int> UpdateAsync(ArticleViewModel model);
    }
}
