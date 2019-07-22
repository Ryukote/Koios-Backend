using KoiosOffers.Contracts;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace KoiosOffers.Data
{
    public static class ModelConverter
    {
        public static Article ToArticle(ArticleViewModel viewModel)
        {
            return new Article()
            {
                Id = ((ArticleViewModel)viewModel).Id,
                Name = ((ArticleViewModel)viewModel).Name,
                UnitPrice = ((ArticleViewModel)viewModel).UnitPrice
            };
        }

        public static Offer ToOffer(IViewModel viewModel)
        {
            return new Offer()
            {
                Id = ((OfferViewModel)viewModel).Id,
                Number = ((OfferViewModel)viewModel).Number,
                CreatedAt = ((OfferViewModel)viewModel).CreatedAt,
                TotalPrice = ((OfferViewModel)viewModel).TotalPrice,
            };
        }

        public static OfferArticle ToOfferArticle(IViewModel viewModel)
        {
            var article = ModelConverter.ToArticle(((OfferArticleViewModel)viewModel).Article);
            var offer = ModelConverter.ToOffer(((OfferArticleViewModel)viewModel).Offer);

            return new OfferArticle()
            {
                Id = ((OfferArticleViewModel)viewModel).Id,
                ArticleId = ((OfferArticleViewModel)viewModel).ArticleId,
                OfferId = ((OfferArticleViewModel)viewModel).OfferId,
                Article = article,
                Offer = offer
            };
        }

        public static IEnumerable<ArticleViewModel> ToArticleViewModelEnumerable(IEnumerable<Article> articleCollection)
        {
            List<ArticleViewModel> viewModelCollection = new List<ArticleViewModel>();

            foreach (var item in articleCollection)
            {
                viewModelCollection.Add(new ArticleViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    UnitPrice = item.UnitPrice
                });
            }

            return viewModelCollection;
        }

        public static IEnumerable<OfferViewModel> ToOfferViewModelEnumerable(IEnumerable<Offer> offerCollection)
        {
            List<OfferViewModel> viewModelCollection = new List<OfferViewModel>();

            foreach (var item in offerCollection)
            {
                viewModelCollection.Add(ToOfferViewModel(item));
            }

            return viewModelCollection;
        }

        public static OfferViewModel ToOfferViewModel(Offer item)
        {

            var offerViewModel = new OfferViewModel()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                Number = item.Number,
                TotalPrice = item.TotalPrice,
            };
            var articles = item.OfferArticles.Select(x => x.Article).ToList();
            foreach (var article in articles)
            {
                offerViewModel.Articles.Add(ToArticleViewModel(article));
            }

            return offerViewModel;
        }

        public static IEnumerable<OfferArticleViewModel> ToOfferArticleViewModelEnumerable(IEnumerable<OfferArticle> offerArticleCollection)
        {
            List<OfferArticleViewModel> viewModelCollection = new List<OfferArticleViewModel>();

            foreach (var item in offerArticleCollection)
            {
                viewModelCollection.Add(new OfferArticleViewModel()
                {
                    Id = item.Id,
                    ArticleId = item.ArticleId,
                    OfferId = item.OfferId
                });
            }

            return viewModelCollection;
        }

        public static ICollection<OfferArticleViewModel> ToOfferArticleViewModelCollection(IEnumerable<OfferArticle> offerArticleCollection)
        {
            List<OfferArticleViewModel> viewModelCollection = new List<OfferArticleViewModel>();

            foreach (var item in offerArticleCollection)
            {
                viewModelCollection.Add(new OfferArticleViewModel()
                {
                    Id = item.Id,
                    ArticleId = item.ArticleId,
                    OfferId = item.OfferId
                });
            }

            return viewModelCollection;
        }

        public static ArticleViewModel ToArticleViewModel(Article item)
        {
            return new ArticleViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                OfferArticles = ToOfferArticleViewModelCollection(item.OfferArticles),
                UnitPrice = item.UnitPrice
            };
        }
    }
}
