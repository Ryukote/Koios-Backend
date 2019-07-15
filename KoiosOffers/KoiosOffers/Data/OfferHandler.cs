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
    public class OfferHandler : IRepository<int>
    {
        private readonly OfferContext _dbContext;

        public OfferHandler(OfferContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(IViewModel viewModel)
        {
            var converted = ModelConverter.ToOffer((OfferViewModel)viewModel);
            _dbContext.Set<Offer>().Add(converted);
            int result = await _dbContext.SaveChangesAsync();

            _dbContext.Entry(converted).State = EntityState.Detached;

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            IEnumerable<IViewModel> result = await GetAsync(o => ((IId<int>)o).Id.Equals(id));
            Offer entity = (Offer)result.First();
            _dbContext.Set<Offer>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<IViewModel>> GetAsync(Expression<Func<IViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "")
        {
            var query = (IEnumerable<IViewModel>)_dbContext.Set<Offer>();

            if (filter != null)
            {
                query = query.Where(filter.Compile());
            }

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            return await query.AsQueryable().ToListAsync();
        }

        public async Task<int> UpdateAsync(IViewModel model)
        {
            _dbContext.Set<Offer>().Update((Offer)model);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
