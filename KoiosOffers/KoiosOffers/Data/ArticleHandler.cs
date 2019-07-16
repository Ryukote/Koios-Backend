using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KoiosOffers.Contracts;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KoiosOffers.Data
{
    public class ArticleHandler<TId> : IArticleHandler<TId>
        where TId : struct
    {
        private readonly OfferContext _dbContext;

        public ArticleHandler(OfferContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(ArticleViewModel viewModel)
        {
            var objectToFind = _dbContext.ChangeTracker.Entries()
                .Where(a => a.State == EntityState.Added && a.Entity.GetType().Name.Equals("Article"))
                .Select(a => a.Entity as Article);

            _dbContext.Article.Where(a => a.Id.Equals(viewModel.Id)).ToList().AddRange(objectToFind);

            foreach (var item in _dbContext.Article)
            {
                if (item.Id == viewModel.Id)
                {
                    return 0;
                }
            }

            var converted = ModelConverter.ToArticle(viewModel);
            int result = 0;
            _dbContext.Article.Add(converted);

            result = await _dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<int> DeleteAsync(TId id)
        {
            IEnumerable<ArticleViewModel> result = await GetAsync(o => ((IId<TId>)o).Id.Equals(id));
            Article entity = ModelConverter.ToArticle(result.First());
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.Set<Article>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleViewModel>> GetAsync(Expression<Func<ArticleViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "")
        {
            var query = (IEnumerable<Article>)await _dbContext.Set<Article>().ToListAsync();

            var converted = ModelConverter.ToArticleViewModelEnumerable(query);

            if (filter != null)
            {
                converted = converted.Where(filter.Compile());
            }

            if (skip > 0)
            {
                converted = converted.Skip(skip);
            }

            if (take > 0)
            {
                converted = converted.Take(take);
            }

            return converted;
        }

        public async Task<int> UpdateAsync(ArticleViewModel model)
        {
            _dbContext.Set<Article>().Update(ModelConverter.ToArticle(model));
            return await _dbContext.SaveChangesAsync();
        }
    }
}
