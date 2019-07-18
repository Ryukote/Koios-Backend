using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IArticleHandler
    {
        Task<IEnumerable<ArticleViewModel>> GetAllAsync();
        Task<ArticleViewModel> GetByIdAsync(int id);
        Task<int> GetIdByNameAsync(string name);
        Task<IEnumerable<ArticleViewModel>> GetPaginatedAsync(string name = default, int take = default, int skip = default);
        Task<int> AddAsync(ArticleViewModel model);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(ArticleViewModel model);
    }
}
