using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KoiosOffersTests
{
    public class OfferArticleHandlerTests
    {
        private static IOfferArticleHandler<int> GetInMemoryForOfferArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new OfferArticleHandler<int>(offerContext);
        }

        [Fact]
        public async Task WillCreateOfferArticle()
        {
            IOfferArticleHandler<int> sut = GetInMemoryForOfferArticle();

            var articleId = 1;
            var offerId = 1;

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = articleId,
                Name = "HDD1",
                UnitPrice = 700
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Id = offerId,
                Number = 5,
                TotalPrice = 1500
            };

            OfferArticleViewModel offerArticle = new OfferArticleViewModel()
            {
                Id = 7,
                ArticleId = articleId,
                OfferId = offerId
            };

            int result = await sut.AddAsync(offerArticle);

            Assert.False(article == null);
            Assert.False(offer == null);
            Assert.False(offerArticle == null);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task WillCreateRelatedData()
        {
            var sut = GetInMemoryForOfferArticle();

            List<OfferArticleViewModel> filledOffers = InMemoryMock.MockOfferArticles();

            int saved = 0;

            foreach (OfferArticleViewModel offerArticle in filledOffers)
            {
                if (await sut.AddAsync(offerArticle) > 0)
                {
                    saved++;
                }
            }

            Assert.False(filledOffers == null);
            Assert.True(filledOffers.Count > 0);
            Assert.Equal(50000, saved);
        }

        [Fact]
        public async Task OfferArticleAlreadyExists()
        {
            var sut = GetInMemoryForOfferArticle();

            var articleId = 1;
            var offerId = 1;

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = articleId,
                Name = "HDD1",
                UnitPrice = 700
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Id = offerId,
                Number = 5,
                TotalPrice = 1500
            };

            OfferArticleViewModel offerArticle = new OfferArticleViewModel()
            {
                Id = 7,
                ArticleId = articleId,
                OfferId = offerId
            };

            await sut.AddAsync(offerArticle);
            int result = await sut.AddAsync(offerArticle);

            Assert.False(article == null);
            Assert.False(offer == null);
            Assert.False(offerArticle == null);
            Assert.Equal(0, result);
        }
    }
}
