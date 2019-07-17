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
            var objectToFind = _dbContext.ChangeTracker.Entries()
                .Where(a => a.State == EntityState.Added && a.Entity.GetType().Name.Equals("Offer"))
                .Select(a => a.Entity as Offer);

            _dbContext.Offer.Where(a => a.Id.Equals(viewModel.Id)).ToList().AddRange(objectToFind);

            foreach (var item in _dbContext.Offer)
            {
                if (item.Id == viewModel.Id)
                {
                    return 0;
                }
            }

            var converted = ModelConverter.ToOffer(viewModel);
            int result = 0;
            _dbContext.Offer.Add(converted);

            result = await _dbContext.SaveChangesAsync();

            

            return result;
        }

        public async Task<int> DeleteAsync(TId id)
        {
            IEnumerable<OfferViewModel> result = await GetByIdAsync(id);
            Offer entity = ModelConverter.ToOffer(result.First());
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.Set<Offer>().Remove(entity);
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
