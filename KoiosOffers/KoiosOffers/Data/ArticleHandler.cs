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
    public class ArticleHandler : IArticleHandler
    {
        private readonly OfferContext _dbContext;

        public ArticleHandler(OfferContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(ArticleViewModel viewModel)
        {
            foreach (var item in _dbContext.Article)
            {
                if (item.Id == viewModel.Id)
                {
                    return 0;
                }
            }

            var converted = ModelConverter.ToArticle(viewModel);
            _dbContext.Article.Add(converted);

            await _dbContext.SaveChangesAsync();

            return converted.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {

            var articleToDelete = await _dbContext.Article
                .Include(x => x.OfferArticles)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (articleToDelete == null)
            {
                throw new ArgumentException("Provided id is not valid.", nameof(id));
            }

            var offersToUpdateIds = articleToDelete.OfferArticles.Select(x => x.OfferId).ToList();
            var offersToUpdate = await _dbContext.Offer
                .Include(x => x.OfferArticles)
                .ThenInclude(x => x.Article)
                .Where(x => offersToUpdateIds.Contains(x.Id))
                .ToListAsync();

            foreach (var offer in offersToUpdate)
            {
                offer.TotalPrice = offer.OfferArticles.Sum(x => x.Article.UnitPrice) - articleToDelete.UnitPrice;
            }

            _dbContext.Article.Remove(articleToDelete);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleViewModel>> GetAllAsync()
        {
            var query = await _dbContext.Article.AsNoTracking().ToListAsync();

            var converted = ModelConverter.ToArticleViewModelEnumerable(query);

            return converted;
        }

        public async Task<ArticleViewModel> GetByIdAsync(int id)
        {
            var article = await _dbContext.Article.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return ModelConverter.ToArticleViewModel(article);
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
            var originalPrice = _dbContext.Article
                .AsNoTracking()
                .Where(x => x.Id == model.Id)
                .Select(x => x.UnitPrice)
                .FirstOrDefault();

            _dbContext.Article.Update(ModelConverter.ToArticle(model));

            var updated = await _dbContext.SaveChangesAsync();

            if (originalPrice != model.UnitPrice)
            {
                var offersToUpdate = _dbContext.Offer
                    .Include(x => x.OfferArticles)
                        .ThenInclude(x => x.Article)
                    .Where(x => x.OfferArticles.Any(a => a.ArticleId == model.Id))
                    .ToList();

                if (offersToUpdate.Count == 0)
                {
                    return updated;
                }

                foreach (var offer in offersToUpdate)
                {
                    offer.TotalPrice = offer.OfferArticles.Sum(x => x.Article.UnitPrice);
                }

            }

            return await _dbContext.SaveChangesAsync();
        }
    }
}
