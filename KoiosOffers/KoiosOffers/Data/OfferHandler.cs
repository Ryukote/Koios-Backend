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
    public class OfferHandler<TId> : IOfferHandler<TId>
        where TId : struct
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
            int result = 0;

            var articleIds = viewModel.Articles.Select(x => x.Id);

            var existingArticles = _dbContext.Article.Where(x => articleIds.Contains(x.Id));

            foreach (var item in viewModel.Articles)
            {
                var article = existingArticles.FirstOrDefault(x => x.Id == item.Id);

                //_dbContext.Entry(article).State = EntityState.Added;

                if (article == null)
                {
                    

                    article = ModelConverter.ToArticle(item);
                    _dbContext.Article.Add(article);

                    await _dbContext.SaveChangesAsync();
                }

                var offerArticle = new OfferArticle()
                {
                    Offer = converted,
                    Article = article
                };

                _dbContext.Add(offerArticle);
            }

            _dbContext.Offer.Add(converted);

            result = await _dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<int> DeleteAsync(TId id)
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

        public async Task<IEnumerable<OfferViewModel>> GetAllAsync()
        {
            var query = await _dbContext.Offer.ToListAsync();

            var converted = ModelConverter.ToOfferViewModelEnumerable(query);

            return converted;
        }

        public async Task<OfferViewModel> GetByIdAsync(int id)
        {
            var offer = await _dbContext.Offer.FindAsync(id);

            return ModelConverter.ToOfferViewModel(offer);
        }

        public async Task<IEnumerable<OfferViewModel>> GetAsync(Func<OfferViewModel, bool> filter = null, int skip = 0, int take = 0, string term = "")
        {
            var query = _dbContext.Offer;

            var converted = ModelConverter.ToOfferViewModelEnumerable(query);

            if (filter != null)
            {
                converted = converted.Where(filter);
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

        public async Task<int> UpdateAsync(OfferViewModel model)
        {
            _dbContext.Set<Offer>().Update(ModelConverter.ToOffer(model));
            return await _dbContext.SaveChangesAsync();
        }
    }
}
