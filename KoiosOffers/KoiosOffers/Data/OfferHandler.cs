using KoiosOffers.Contracts;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var existingOffer = _dbContext.Offer
                .Where(x => x.Number.Equals(viewModel.Number))
                .FirstOrDefault();

            if (existingOffer != null)
            {
                return 0;
            }

            var converted = ModelConverter.ToOffer(viewModel);

            _dbContext.Offer.Add(converted);

            var articleIds = viewModel.Articles.Select(x => x.Id);

            var existingArticles = _dbContext.Article
                .Where(x => articleIds.Contains(x.Id));

            decimal totalPrice = 0;

            if (viewModel.Articles != null || viewModel.Articles.Count > 0)
            {
                foreach (var item in viewModel.Articles)
                {
                    var article = existingArticles
                        .FirstOrDefault(x => x.Id.Equals(item.Id));

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
            try
            {
                _dbContext.OfferArticle.Add(new OfferArticle() {
                    OfferId = offerId,
                    ArticleId = articleId
                });

                await _dbContext.SaveChangesAsync();
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
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
                throw new ArgumentException("Provided id is not valid.",
                    nameof(id));
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteOfferArticle(int offerId, int articleId)
        {
            var result = await _dbContext.OfferArticle
                .Where(x => x.OfferId.Equals(offerId)
                    && x.ArticleId.Equals(articleId))
                .ToListAsync();

            if (result != null)
            {
                foreach(var item in result)
                {
                    _dbContext.OfferArticle.Remove(item);
                }
            }

            else
            {
                throw new ArgumentException("Either provided offer id or article id is not valid.", nameof(offerId) + " " + nameof(articleId));
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<OfferViewModel> GetByIdAsync(int id)
        {
            var offer = await _dbContext.Offer
                .Include(x => x.OfferArticles)
                .ThenInclude(x => x.Article)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            return ModelConverter.ToOfferViewModel(offer);
        }

        public async Task<IEnumerable<ArticleViewModel>> GetOfferArticlesByIdAsync(int offerId)
        {
            List<Article> articles = new List<Article>();

            var offers = await _dbContext.OfferArticle
                .Where(x => x.OfferId.Equals(offerId))
                .ToListAsync();

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
    }
}
