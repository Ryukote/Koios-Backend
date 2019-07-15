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
    public class RepositoryTests
    {
        private static IArticleHandler<int> GetInMemoryForArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: "KoiosArticle");
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureDeleted();
            offerContext.Database.EnsureCreated();
            return new ArticleHandler<int>(offerContext);
        }

        private static IRepository<int> GetInMemoryForOffer()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: "KoiosOffer");
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new OfferHandler(offerContext);
        }

        private static IRepository<int> GetInMemoryForOfferArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new OfferArticleHandler(offerContext);
        }

        [Fact]
        public async Task WillCreateArticle()
        {
            var _id = 1;

            var sut = GetInMemoryForArticle();

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = _id,
                Name = "HDD1",
                UnitPrice = 750
            };

            int result = await sut.AddAsync(article);

            Assert.True(result > 0);
        }

        [Fact]
        public async Task ArticleAlreadyExists()
        {
            var sut = GetInMemoryForArticle();

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = 10,
                Name = "HDD1",
                UnitPrice = 750
            };

            await sut.AddAsync(article);

            var result = await sut.AddAsync(article);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task WillUpdateArticle()
        {
            var sut = GetInMemoryForArticle();

            var id = 0;

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Id = id,
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Id = id,
                Name = "HDD2",
                UnitPrice = 755
            };

            await sut.AddAsync(article1);
            
            int result = await sut.UpdateAsync(article2);

            Assert.True(result > 0);
        }

        [Fact]
        public async Task UpdateArticleNonExistingId()
        {
            var sut = GetInMemoryForArticle();

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = 15,
                Name = "HDD2",
                UnitPrice = 755
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => sut.UpdateAsync(article));
        }

        [Fact]
        public async Task DeleteArticleNonExistingId()
        {
            var sut = GetInMemoryForArticle();

            var id = 75;

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.DeleteAsync(id));
        }

        [Fact]
        public async Task WillDeleteArticle()
        {
            var id = 1;

            var sut = GetInMemoryForArticle();

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = id,
                Name = "HDD1",
                UnitPrice = 750
            };

            int saved = await sut.AddAsync(article);

            int deleted = await sut.DeleteAsync(id);

            Assert.True(deleted.Equals(0));
        }

        [Fact]
        public async Task WillCreateOfferArticle()
        {
            IRepository<int> sut = GetInMemoryForOfferArticle();

            var articleint = 1;
            var offerint = 1;

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Id = articleint,
                Name = "HDD1",
                UnitPrice = 700
            };

            OfferViewModel offer = new OfferViewModel()
            {
                Id = offerint,
                Number = 5,
                TotalPrice = 1500
            };

            OfferArticleViewModel offerArticle = new OfferArticleViewModel()
            {
                Id =7,
                ArticleId = articleint,
                OfferId = offerint
            };

            int result = await sut.AddAsync(offerArticle);

            Assert.True(result > 0);
        }

        [Fact]
        public async Task WillCreateRelatedData()
        {
            var sut = GetInMemoryForOfferArticle();

            List<OfferArticleViewModel> filledOffers = InMemoryMock.MockOfferArticles();

            int saved = 0;

            foreach(OfferArticleViewModel offerArticle in filledOffers)
            {
                if(await sut.AddAsync(offerArticle) > 0)
                {
                    saved++;
                }
            }

            Assert.Equal(50000, saved);
        }
    }
}
