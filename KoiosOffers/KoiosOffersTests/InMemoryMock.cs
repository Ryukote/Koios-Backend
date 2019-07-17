using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;

namespace KoiosOffersTests
{
    public static class InMemoryMock
    {
        private static List<ArticleViewModel> MockArticles()
        {
            List<ArticleViewModel> articles = new List<ArticleViewModel>();

            for (int i = 0; i < 50000; i++)
            {
                articles.Add(new ArticleViewModel()
                {
                    Name = "article" + i.ToString(),
                    UnitPrice = 10 * i
                });
            }

            return articles;
        }

        public static List<OfferViewModel> MockOffers()
        {
            List<OfferViewModel> offers = new List<OfferViewModel>();

            for (int i = 0; i < 10000; i++)
            {
                offers.Add(new OfferViewModel()
                {
                    CreatedAt = DateTime.UtcNow,
                    Number = i,
                    TotalPrice = i
                });
            }

            return offers;
        }

        public static List<OfferViewModel> MockOfferArticles()
        {
            List<ArticleViewModel> articles = MockArticles();
            List<OfferViewModel> offers = MockOffers();
            List<OfferArticleViewModel> offerArticles = new List<OfferArticleViewModel>();

            int counter = 0;

            foreach (OfferViewModel offer in offers)
            {
                for (int i = 0; i < 5; i++)
                {
                    //offer.Articles.Add(articles[counter]);

                    offerArticles.Add(new OfferArticleViewModel()
                    {
                        Article = articles[counter],
                        Offer = offer
                    });

                    counter++;
                }
            }


            return offers;
        }
    }
}
