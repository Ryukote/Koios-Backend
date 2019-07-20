using KoiosOffers.Contracts;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KoiosOffers.Data
{
    public class OfferHandler : IOfferHandler
    {
        private readonly OfferContext _dbContext;

        public OfferHandler(OfferContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(OfferViewModel viewModel)
        {
            var existingOffer = _dbContext.Offer.Where(x => x.Id.Equals(viewModel.Id)).FirstOrDefault();

            if (existingOffer != null)
            {
                return 0;
            }

            var converted = ModelConverter.ToOffer(viewModel);
            _dbContext.Offer.Add(converted);

            var articleIds = viewModel.Articles.Select(x => x.Id);

            var existingArticles = _dbContext.Article.Where(x => articleIds.Contains(x.Id));

            decimal totalPrice = 0;

            if (viewModel.Articles != null || viewModel.Articles.Count > 0)
            {
                foreach (var item in viewModel.Articles)
                {
                    var article = existingArticles.FirstOrDefault(x => x.Id.Equals(item.Id));

                    if (article == null)
                    {
                        article = ModelConverter.ToArticle(item);
                        _dbContext.Article.Add(article);
                    }

                    totalPrice += item.UnitPrice;

                    _dbContext.OfferArticle.Add(new OfferArticle()
                    {
                        Offer = converted,
                        Article = article
                    });
                }
            }

            converted.TotalPrice = totalPrice;
            _dbContext.Offer.Add(converted);
            await _dbContext.SaveChangesAsync();
            return converted.Id;
        }

        public async Task<int> AddOfferArticleAsync(int offerId, int articleId)
        {
            var oaExisting = await _dbContext.OfferArticle.FirstAsync(x => x.OfferId.Equals(offerId) && x.ArticleId.Equals(articleId));

            if(oaExisting == null)
            {
                return 0;
            }

            _dbContext.OfferArticle.Add(new OfferArticle() { OfferId = offerId, ArticleId = articleId });
            await _dbContext.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = _dbContext.Offer.FirstOrDefault(x => x.Id.Equals(id));

            if (result != null)
            {
                _dbContext.Offer.Remove(result);
            }

            else
            {
                throw new ArgumentException("Provided id is not valid.", nameof(id));
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteOfferArticle(int offerId, int articleId)
        {
            var result = _dbContext.OfferArticle.FirstOrDefault(x => x.OfferId.Equals(offerId) && x.ArticleId.Equals(articleId));

            if (result != null)
            {
                _dbContext.OfferArticle.Remove(result);
            }

            else
            {
                throw new ArgumentException("Either provided offer id or article id is not valid.", nameof(offerId) + " " + nameof(articleId));
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OfferViewModel>> GetAllAsync()
        {
            var query = await _dbContext.Offer.ToListAsync();

            var converted = ModelConverter.ToOfferViewModelEnumerable(query);

            return converted;
        }

        public async Task<OfferViewModel> GetByIdAsync(int id)
        {
            var offer = await _dbContext.Offer
                .Include(x => x.OfferArticles)
                    .ThenInclude(x => x.Article)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));


            return ModelConverter.ToOfferViewModel(offer);
        }

        public async Task<OfferViewModel> GetOfferByOfferNumberAsync(int offerNumber)
        {
            var offer = await _dbContext.Offer
                .FirstOrDefaultAsync(x => x.Number.Equals(offerNumber));


            return ModelConverter.ToOfferViewModel(offer);
        }

        public async Task<IEnumerable<ArticleViewModel>> GetOfferArticlesByIdAsync(int offerId)
        {
            List<Article> articles = new List<Article>();

            var offers = await _dbContext.OfferArticle
                .Where(x => x.OfferId.Equals(offerId)).ToListAsync();

            foreach(var item in offers)
            {
                var article = await _dbContext.Article
                    .FirstAsync(x => x.Id.Equals(item.ArticleId));

                if(article != null)
                {
                    articles.Add(article);
                }
            }

            return ModelConverter.ToArticleViewModelEnumerable(articles.AsEnumerable());
        }

        public async Task<IEnumerable<OfferViewModel>> GetPaginatedAsync(int offerNumber, int take = default, int skip = default)
        {
            var query = await _dbContext.Offer
                .Where(x => x.Number.ToString().Contains(offerNumber.ToString()))
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var converted = ModelConverter.ToOfferViewModelEnumerable(query);

            return converted;
        }

        //public async Task<IEnumerable<OfferViewModel>> GetAsync(Func<OfferViewModel, bool> filter = null, int skip = 0, int take = 0, string term = "")
        //{
        //    var query = _dbContext.Offer;

        //    var converted = ModelConverter.ToOfferViewModelEnumerable(query);

        //    if (filter != null)
        //    {
        //        converted = converted.Where(filter);
        //    }

        //    if (skip > 0)
        //    {
        //        converted = converted.Skip(skip);
        //    }

        //    if (take > 0)
        //    {
        //        converted = converted.Take(take);
        //    }

        //    return converted;
        //}

        public async Task<int> UpdateAsync(OfferViewModel model)
        {
            _dbContext.Offer.Update(ModelConverter.ToOffer(model));
            return await _dbContext.SaveChangesAsync();
        }
    }
}
