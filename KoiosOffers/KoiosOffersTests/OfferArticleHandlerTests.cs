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
        private static IArticleHandler GetInMemoryForArticle(string databaseName)
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: databaseName);
            builder.EnableSensitiveDataLogging(true);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);

            offerContext.Database.EnsureCreated();
            return new ArticleHandler(offerContext);
        }

        private static IOfferHandler<int> GetInMemoryForOffer(string databaseName)
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: databaseName);
            builder.EnableSensitiveDataLogging(true);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new OfferHandler<int>(offerContext);
        }

        [Fact]
        public async Task WillCreateOfferArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            IOfferHandler<int> sut = GetInMemoryForOffer(databaseName);

            ArticleViewModel article = new ArticleViewModel()
            {
                //Id = articleId,
                Name = "HDD1",
                UnitPrice = 700
            };

            OfferViewModel offer = new OfferViewModel()
            {
                //Id = offerId,
                Number = 5,
                TotalPrice = 1500
            };

            offer.Articles.Add(article);

            int result = await sut.AddAsync(offer);

            Assert.False(article == null);
            Assert.False(offer == null);
            Assert.True(offer.Articles.Any());
            Assert.True(result > 0);
        }

        [Fact]
        public async Task OfferArticleAlreadyExists()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForOffer(databaseName);

            ArticleViewModel article = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 700
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Number = 5,
                TotalPrice = 1500
            };

            OfferArticleViewModel offerArticle = new OfferArticleViewModel()
            {
                Article = article,
                Offer = offer
            };

            offer.Articles.Add(article);

            await sut.AddAsync(offer);

            int result = await sut.AddAsync(offer);

            Assert.False(article == null);
            Assert.False(offer == null);
            Assert.False(offerArticle == null);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task WillUpdateTotalPriceOnAdd()
        {
            var databaseName = Guid.NewGuid().ToString();

            IOfferHandler<int> offerSut = GetInMemoryForOffer(databaseName);
            IArticleHandler articleSut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 700
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 900
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Number = 5
            };

            OfferArticleViewModel offerArticle1 = new OfferArticleViewModel()
            {
                Offer = offer,
                Article = article1
            };

            OfferArticleViewModel offerArticle2 = new OfferArticleViewModel()
            {
                Offer = offer,
                Article = article2
            };

            article1.OfferArticles = new List<OfferArticleViewModel>();
            article1.OfferArticles.Add(offerArticle1);

            article2.OfferArticles = new List<OfferArticleViewModel>();
            article2.OfferArticles.Add(offerArticle2);

            offer.Articles = new List<ArticleViewModel>();
            offer.Articles.Add(article1);

            offer.Articles.Add(article2);

            int addedOfferArticle1 = await offerSut.AddAsync(offer);

            var newOfferSut2 = GetInMemoryForOffer(databaseName);
            var specifiedOffer = await newOfferSut2.GetByIdAsync(addedOfferArticle1);

            Assert.False(article1 == null);
            Assert.False(article2 == null);
            Assert.False(offer == null);
            Assert.False(offerArticle1 == null);
            Assert.False(offerArticle2 == null);
            Assert.True(addedOfferArticle1 > 0);
            Assert.Equal(1600, specifiedOffer.TotalPrice);
        }

        [Fact]
        public async Task WillUpdateTotalPriceOnUnitPriceUpdate()
        {
            var databaseName = Guid.NewGuid().ToString();

            IOfferHandler<int> offerSut = GetInMemoryForOffer(databaseName);
            IArticleHandler articleSut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 700
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 900
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Number = 5
            };

            offer.Articles.Add(article1);

            offer.Articles.Add(article2);

            int addedOfferArticleId = await offerSut.AddAsync(offer);

            var specifiedOffer = await offerSut.GetByIdAsync(addedOfferArticleId);

            article1.Id = addedOfferArticleId;
            article1.UnitPrice = 1000;

            await articleSut.UpdateAsync(article1);

            var updatedOffers = GetInMemoryForOffer(databaseName);

            var updatedPrice = await updatedOffers.GetByIdAsync(addedOfferArticleId);

            Assert.False(article1 == null);
            Assert.False(article2 == null);
            Assert.False(offer == null);
            Assert.Equal(1600, specifiedOffer.TotalPrice);
            Assert.Equal(1900, updatedPrice.TotalPrice);
        }

        [Fact]
        public async Task WillUpdateTotalPriceOnArticleDelete()
        {
            var databaseName = Guid.NewGuid().ToString();

            IOfferHandler<int> offerSut = GetInMemoryForOffer(databaseName);
            IArticleHandler articleSut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 700
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 900
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Number = 5
            };

            offer.Articles.Add(article1);

            offer.Articles.Add(article2);

            int offerId = await offerSut.AddAsync(offer);

            var specifiedOffer = await offerSut.GetByIdAsync(offerId);

            article1.Id = specifiedOffer.Articles.First().Id;

            await articleSut.DeleteAsync(article1.Id);

            var updatedOffers = GetInMemoryForOffer(databaseName);

            var updatedPrice = await updatedOffers.GetByIdAsync(offerId);

            Assert.False(article1 == null);
            Assert.False(article2 == null);
            Assert.False(offer == null);
            Assert.Equal(1600, specifiedOffer.TotalPrice);
            Assert.Equal(900, updatedPrice.TotalPrice);
        }
    }
}
