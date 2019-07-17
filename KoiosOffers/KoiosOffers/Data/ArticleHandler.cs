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
            var articleToDelete = await GetByIdAsync(id);

            var offerArticles = await _dbContext.OfferArticle
                .Where(x => x.ArticleId == articleToDelete.Id)
                .Select(x => x)
                .ToListAsync();

            var offerArticleIds = offerArticles
                .Select(x => x.OfferId).ToList();

            if(offerArticles.Count == 0)
            {
                var entity = ModelConverter.ToArticle(articleToDelete);

                _dbContext.Article.Remove(entity);

                return await _dbContext.SaveChangesAsync();
            }

            foreach(var item in offerArticles)
            {
                _dbContext.OfferArticle.Remove(item);

                var offers = await _dbContext.Offer
                    .Where(x => x.OfferArticles.Contains(item))
                    .ToListAsync();

                await _dbContext.SaveChangesAsync();

                var offer = offers.First();

                var prices = await _dbContext.OfferArticle
                    .Where(x => x.OfferId == offer.Id)
                    .Select(x => x.Article.UnitPrice)
                    .ToListAsync();

                decimal totalPrice = 0;

                foreach(var price in prices)
                {
                    totalPrice += price;
                }

                offer.TotalPrice = totalPrice;

                _dbContext.Offer.Update(offer);
            }



            //var entity = ModelConverter.ToArticle(articleToDelete);

            //_dbContext.Article.Remove(entity);

            return await _dbContext.SaveChangesAsync();
            //----------------------------------------------------------------------------------------
            //kod koji trenutno radi as is
            //----------------------------------------------------------------------------------------

            //IEnumerable<ArticleViewModel> result = await GetAsync(o => ((IId<TId>)o).Id.Equals(id));
            //Article entity = ModelConverter.ToArticle(result.First());
            //_dbContext.Entry(entity).State = EntityState.Deleted;
            //_dbContext.Set<Article>().Remove(entity);
            //return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleViewModel>> GetAllAsync()
        {
            var query = await _dbContext.Article.ToListAsync();

            var converted = ModelConverter.ToArticleViewModelEnumerable(query);

            return converted;
        }

        public async Task<ArticleViewModel> GetByIdAsync(int id)
        {
            //.FindAsync(id)
            var articles = await _dbContext.Article.Where(x => x.Id.Equals(id)).ToListAsync();

            var wantedArticle = articles.First();
            _dbContext.Entry(wantedArticle).State = EntityState.Detached;

            if (wantedArticle.OfferArticles == null)
            {
                wantedArticle.OfferArticles = new List<OfferArticle>();
            }

            return ModelConverter.ToArticleViewModel(wantedArticle);
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
            var originalPrice = _dbContext.Article.Where(x => x.Id.Equals(model.Id)).Select(x => x.UnitPrice).FirstOrDefault();

            _dbContext.Article.Update(ModelConverter.ToArticle(model));

            var updated = await _dbContext.SaveChangesAsync();

            if (originalPrice != model.UnitPrice)
            {
                var offerIdList = _dbContext.OfferArticle.Where(x => x.ArticleId.Equals(model.Id)).Select(x => x.OfferId).ToList();

                if(offerIdList.Count.Equals(0))
                {
                    return updated;
                }

                foreach(var offerId in offerIdList)
                {
                    decimal unitPriceSum = 0;

                    var offerArticles = await _dbContext.OfferArticle.ToListAsync();

                    foreach (var item in offerArticles)
                    {
                        unitPriceSum += _dbContext.Article
                            .Where(a => a.Id.Equals(item.ArticleId))
                            .Select(a => a.UnitPrice)
                            .FirstOrDefault();
                    }

                    var offer = await _dbContext.Offer.Where(o => o.Id.Equals(offerId)).FirstOrDefaultAsync();

                    if (offer != null)
                    {
                        offer.TotalPrice = unitPriceSum;
                        _dbContext.Offer.Update(offer);
                    }
                }
            }

            return await _dbContext.SaveChangesAsync();
        }
    }
}
