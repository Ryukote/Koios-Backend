//using KoiosOffers.Contracts;
//using KoiosOffers.Models;
//using KoiosOffers.ViewModels;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace KoiosOffers.Data
//{
//    public class OfferArticleHandler<TId> : IOfferArticleHandler<TId>
//        where TId : struct
//    {
//        private readonly OfferContext _dbContext;

//        public OfferArticleHandler(OfferContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<int> AddAsync(OfferArticleViewModel viewModel)
//        {
//            var objectToFind = from entry in _dbContext.OfferArticle
//                               where entry.Id == viewModel.Id
//                               select entry;

//            if (objectToFind.Count() > 0)
//            {
//                return 0;
//            }

//            if (viewModel.Article == null)
//            {
//                viewModel.Article = new Article();
//            }

//            if (viewModel.Offer == null)
//            {
//                viewModel.Offer = new Offer();
//            }

//            var converted = ModelConverter.ToOfferArticle(viewModel);
//            int result = 0;
//            _dbContext.OfferArticle.Add(converted);

//            decimal unitPriceSum = 0;
//            await _dbContext.SaveChangesAsync();
//            //_dbContext.Entry(converted).State = EntityState.Detached;
//            var offerArticles = _dbContext.OfferArticle;

//            //offerArticles
//            foreach (var item in offerArticles.)
//            {
//                unitPriceSum += item.UnitPrice;
//                //unitPriceSum += _dbContext.Article.Where(a => a.Id.Equals(item.ArticleId)).Select(a => a.UnitPrice).FirstOrDefault();
//            }

//            //find offer with specified offerId
//            var offer = await _dbContext.Offer.Where(o => o.Id.Equals(viewModel.OfferId)).FirstOrDefaultAsync();

//            if (offer != null)
//            {
//                offer.TotalPrice = unitPriceSum;

//                _dbContext.Offer.Update(offer);
//            }

//            result = await _dbContext.SaveChangesAsync();

//            return result;
//        }

//        public async Task<int> DeleteAsync(TId id)
//        {
//            IEnumerable<OfferArticleViewModel> result = await GetAsync(o => ((IId<TId>)o).Id.Equals(id));
//            OfferArticle entity = ModelConverter.ToOfferArticle(result.First());
//            _dbContext.Entry(entity).State = EntityState.Deleted;
//            _dbContext.Set<OfferArticle>().Remove(entity);
//            return await _dbContext.SaveChangesAsync();
//        }

//        public async Task<IEnumerable<OfferArticleViewModel>> GetAsync(Expression<Func<OfferArticleViewModel, bool>> filter = null, int skip = 0, int take = 0, string term = "")
//        {
//            var query = (IEnumerable<OfferArticle>)await _dbContext.Set<OfferArticle>().ToListAsync();

//            var converted = ModelConverter.ToOfferArticleViewModelEnumerable(query);

//            if (filter != null)
//            {
//                converted = converted.Where(filter.Compile());
//            }

//            if (skip > 0)
//            {
//                converted = converted.Skip(skip);
//            }

//            if (take > 0)
//            {
//                converted = converted.Take(take);
//            }

//            return converted;
//        }

//        public async Task<int> UpdateAsync(OfferArticleViewModel model)
//        {
//            _dbContext.Set<OfferArticle>().Update(ModelConverter.ToOfferArticle(model));

//            decimal unitPriceSum = 0;
//            _dbContext.Entry(ModelConverter.ToOfferArticle(model)).State = EntityState.Unchanged;

//            await _dbContext.SaveChangesAsync();

//            unitPriceSum += _dbContext.Article.Where(a => a.Id.Equals(model.ArticleId)).Select(a => a.UnitPrice).FirstOrDefault();

//            //find offer with specified offerId
//            var offer = await _dbContext.Offer.Where(o => o.Id.Equals(model.OfferId)).FirstOrDefaultAsync();

//            if (offer != null)
//            {
//                offer.TotalPrice = unitPriceSum;

//                _dbContext.Offer.Update(offer);
//            }

//            _dbContext.Entry(offer).State = EntityState.Detached;

//            return await _dbContext.SaveChangesAsync();
//        }
//    }
//}
