using KoiosOffers.Contracts;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;

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
                TotalPrice = ((OfferViewModel)viewModel).TotalPrice
            };
        }

        public static OfferArticle ToOfferArticle(IViewModel viewModel)
        {
            return new OfferArticle()
            {
                Id = ((OfferArticleViewModel)viewModel).Id,
                ArticleId = ((OfferArticleViewModel)viewModel).Id,
                OfferId = ((OfferArticleViewModel)viewModel).Id
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
                viewModelCollection.Add(new OfferViewModel()
                {
                    Id = item.Id,
                    CreatedAt = item.CreatedAt,
                    Number = item.Number,
                    TotalPrice = item.TotalPrice
                });
            }

            return viewModelCollection;
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
    }
}
