using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using System;
using System.Collections.Generic;

namespace KoiosOffersTests
{
    public static class InMemoryMock
    {
        private static List<Article> MockArticles()
        {
            List<Article> articles = new List<Article>();

            for (int i = 0; i < 50000; i++)
            {
                articles.Add(new Article()
                {
                    Id = i,
                    Name = "article" + i.ToString(),
                    UnitPrice = 10 * i
                });
            }

            return articles;
        }

        public static List<Offer> MockOffers()
        {
            List<Offer> offers = new List<Offer>();

            for (int i = 0; i < 10000; i++)
            {
                offers.Add(new Offer()
                {
                    Id = i,
                    CreatedAt = DateTime.UtcNow,
                    Number = i,
                    TotalPrice = i
                });
            }

            return offers;
        }

        public static List<OfferArticleViewModel> MockOfferArticles()
        {
            List<Article> articles = MockArticles();
            List<Offer> offers = MockOffers();

            List<OfferArticleViewModel> connectedOffers = new List<OfferArticleViewModel>();

            int counter = 0;

            lock (connectedOffers)
            {
                foreach (Offer offer in offers)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        connectedOffers.Add(new OfferArticleViewModel()
                        {
                            Id = articles[counter].Id,
                            ArticleId = counter,
                            OfferId = offer.Id,
                            Article = articles[counter],
                            Offer = offer
                        });

                        counter++;
                    }
                }
            }
            

            return connectedOffers;
        }
    }
}
