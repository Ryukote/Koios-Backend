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
    public class OfferArticleHandler : IRepository<int>
    {
        private readonly OfferContext _dbContext;

        public OfferArticleHandler(OfferContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(IViewModel viewModel)
        {
            var converted = ModelConverter.ToOfferArticle((OfferArticleViewModel)viewModel);
            _dbContext.Set<OfferArticle>().Add(converted);
            _dbContext.Entry(converted).State = EntityState.Added;
            int result = await _dbContext.SaveChangesAsync();

            _dbContext.Entry(converted).State = EntityState.Detached;

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            IEnumerable<IViewModel> result = await GetAsync(o => ((IId<int>)o).Id.Equals(id));
            OfferArticle entity = (OfferArticle)result.First();
            _dbContext.Set<OfferArticle>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<IViewModel>> GetAsync(Expression<Func<IViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "")
        {
            var query = (IEnumerable<IViewModel>)_dbContext.Set<IViewModel>();

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
            _dbContext.Set<OfferArticle>().Update((OfferArticle)model);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
