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
    public class OfferArticleHandler<TId> : IOfferArticleHandler<TId>
        where TId : struct
    {
        private readonly OfferContext _dbContext;

        public OfferArticleHandler(OfferContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(OfferArticleViewModel viewModel)
        {
            var objectToFind = _dbContext.ChangeTracker.Entries()
                .Where(a => a.State == EntityState.Added && a.Entity.GetType().Name.Equals("OfferArticle"))
                .Select(a => a.Entity as OfferArticle);

            _dbContext.OfferArticle.Where(a => a.Id.Equals(viewModel.Id)).ToList().AddRange(objectToFind);

            foreach (var item in _dbContext.OfferArticle)
            {
                if (item.Id == viewModel.Id)
                {
                    return 0;
                }
            }

            var converted = ModelConverter.ToOfferArticle(viewModel);
            int result = 0;
            _dbContext.OfferArticle.Add(converted);

            result = await _dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<int> DeleteAsync(TId id)
        {
            IEnumerable<OfferArticleViewModel> result = await GetAsync(o => ((IId<TId>)o).Id.Equals(id));
            OfferArticle entity = ModelConverter.ToOfferArticle(result.First());
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.Set<OfferArticle>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OfferArticleViewModel>> GetAsync(Expression<Func<OfferArticleViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "")
        {
            var query = (IEnumerable<OfferArticle>)await _dbContext.Set<OfferArticle>().ToListAsync();

            var converted = ModelConverter.ToOfferArticleViewModelEnumerable(query);

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

        public async Task<int> UpdateAsync(OfferArticleViewModel model)
        {
            _dbContext.Set<OfferArticle>().Update(ModelConverter.ToOfferArticle(model));
            return await _dbContext.SaveChangesAsync();
        }
    }
}
