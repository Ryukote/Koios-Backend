using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KoiosOffersTests
{
    public class OfferArticleHandlerTests
    {
        private static string _databaseName = Guid.NewGuid().ToString();
        public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
#pragma warning disable CS0618 // Type or member is obsolete
            new ConsoleLoggerProvider((_, __) => true, true)
#pragma warning restore CS0618 // Type or member is obsolete
        });

        public OfferArticleHandlerTests()
        {
            //_databaseName = Guid.NewGuid().ToString();
        }

        private static IOfferArticleHandler<int> GetInMemoryForOfferArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: _databaseName);
            builder.UseLoggerFactory(loggerFactory);
            builder.EnableSensitiveDataLogging(true);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureDeleted();
            offerContext.Database.EnsureCreated();
            return new OfferArticleHandler<int>(offerContext);
        }

        private static IArticleHandler<int> GetInMemoryForArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: _databaseName);
            builder.UseLoggerFactory(loggerFactory);
            builder.EnableSensitiveDataLogging(true);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new ArticleHandler<int>(offerContext);
        }

        private static IOfferHandler<int> GetInMemoryForOffer()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: _databaseName);
            builder.UseLoggerFactory(loggerFactory);
            builder.EnableSensitiveDataLogging(true);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new OfferHandler<int>(offerContext);
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
                int result = await sut.AddAsync(offerArticle);

                if (result > 0)
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

        [Fact]
        public async Task WillUpdateTotalPriceOnAdd()
        {
            IOfferArticleHandler<int> offerArticleSut = GetInMemoryForOfferArticle();
            IOfferHandler<int> offerSut = GetInMemoryForOffer();
            IArticleHandler<int> articleSut = GetInMemoryForArticle();

            var articleId = 1;
            var offerId = 1;

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Id = articleId,
                Name = "HDD1",
                UnitPrice = 700
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Id = articleId + 1,
                Name = "HDD2",
                UnitPrice = 900
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Id = 1,
                Number = 5
            };

            OfferArticleViewModel offerArticle1 = new OfferArticleViewModel()
            {
                Id = 7,
                ArticleId = articleId,
                OfferId = offerId
            };

            OfferArticleViewModel offerArticle2 = new OfferArticleViewModel()
            {
                Id = 8,
                ArticleId = articleId + 1,
                OfferId = offerId
            };

            article1.OfferArticles = new List<OfferArticleViewModel>();
            article1.OfferArticles.Add(offerArticle1);

            article2.OfferArticles = new List<OfferArticleViewModel>();
            article2.OfferArticles.Add(offerArticle2);

            offerArticle1.Article = ModelConverter.ToArticle(article1);
            offerArticle1.Offer = ModelConverter.ToOffer(offer);

            offerArticle2.Article = ModelConverter.ToArticle(article2);
            offerArticle2.Offer = ModelConverter.ToOffer(offer);

            int addedOfferArticle1 = await offerArticleSut.AddAsync(offerArticle1);
            int addedOfferArticle2 = await offerArticleSut.AddAsync(offerArticle2);

            var offerResult = await offerSut.GetAsync(o => o.Id.Equals(offer.Id));

            var specifiedOffer = await offerSut.GetAsync(o => o.Id.Equals(offer.Id));

            Assert.False(article1 == null);
            Assert.False(article2 == null);
            Assert.False(offer == null);
            Assert.False(offerArticle1 == null);
            Assert.False(offerArticle2 == null);
            Assert.True(addedOfferArticle1 > 0);
            Assert.True(addedOfferArticle2 > 0);
            Assert.Equal(1600, specifiedOffer.First().TotalPrice);
        }

        [Fact]
        public async Task WillUpdateTotalPriceOnUnitPriceUpdate()
        {
            IOfferArticleHandler<int> offerArticleSut = GetInMemoryForOfferArticle();
            IOfferHandler<int> offerSut = GetInMemoryForOffer();
            IArticleHandler<int> articleSut = GetInMemoryForArticle();

            var articleId = 1;
            var offerId = 1;

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Id = articleId,
                Name = "HDD1",
                UnitPrice = 700
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Id = articleId + 1,
                Name = "HDD2",
                UnitPrice = 900
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Id = 1,
                Number = 5
            };

            OfferArticleViewModel offerArticle1 = new OfferArticleViewModel()
            {
                Id = 7,
                ArticleId = articleId,
                OfferId = offerId
            };

            OfferArticleViewModel offerArticle2 = new OfferArticleViewModel()
            {
                Id = 8,
                ArticleId = articleId + 1,
                OfferId = offerId
            };

            article1.OfferArticles = new List<OfferArticleViewModel>();
            article1.OfferArticles.Add(offerArticle1);

            article2.OfferArticles = new List<OfferArticleViewModel>();
            article2.OfferArticles.Add(offerArticle2);

            offerArticle1.Article = ModelConverter.ToArticle(article1);
            offerArticle1.Offer = ModelConverter.ToOffer(offer);

            offerArticle2.Article = ModelConverter.ToArticle(article2);
            offerArticle2.Offer = ModelConverter.ToOffer(offer);

            int addedOfferArticle1 = await offerArticleSut.AddAsync(offerArticle1);
            int addedOfferArticle2 = await offerArticleSut.AddAsync(offerArticle2);

            var offerResult = await offerSut.GetAsync(o => o.Id.Equals(offer.Id));

            var specifiedOffer = await offerSut.GetAsync(o => o.Id.Equals(offer.Id));

            article1.UnitPrice = 1000;

            offerArticle1.Article = ModelConverter.ToArticle(article1);
            offerArticle1.ArticleId = article1.Id;

            var updated = await articleSut.UpdateAsync(article1);
            var updatedPrice = await offerSut.GetAsync(o => o.Id.Equals(offer.Id));
            

            Assert.False(article1 == null);
            Assert.False(article2 == null);
            Assert.False(offer == null);
            Assert.False(offerArticle1 == null);
            Assert.False(offerArticle2 == null);
            Assert.True(addedOfferArticle1 > 0);
            Assert.True(addedOfferArticle2 > 0);
            Assert.Equal(1600, specifiedOffer.First().TotalPrice);
            Assert.Equal(1900, updatedPrice.First().TotalPrice);
        }
    }
}
