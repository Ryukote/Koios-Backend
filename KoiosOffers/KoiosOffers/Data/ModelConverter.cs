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
    }
}
