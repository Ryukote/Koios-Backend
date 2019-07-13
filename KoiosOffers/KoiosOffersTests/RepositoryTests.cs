using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KoiosOffersTests
{
    public class RepositoryTests
    {
        private static IRepository<Article, int> GetInMemoryForArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: "KoiosArticle");
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new Repository<Article, int, OfferContext>(offerContext);
        }

        private static IRepository<Offer, int> GetInMemoryForOffer()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: "KoiosOffer");
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new Repository<Offer, int, OfferContext>(offerContext);
        }

        private static IRepository<OfferArticle, int> GetInMemoryForOfferArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: "KoiosOfferArticle");
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new Repository<OfferArticle, int, OfferContext>(offerContext);
        }

        [Fact]
        public async Task WillCreateArticle()
        {
            IRepository<Article, int> sut = GetInMemoryForArticle();

            Article article = new Article()
            {
                Id = 1,
                Name = "HDD1",
                UnitPrice = 750
            };

            int result = await sut.AddAsync(article);

            Assert.True(result > 0);
        }

        [Fact]
        public async Task ArticleAlreadyExists()
        {
            IRepository<Article, int> sut = GetInMemoryForArticle();

            Article article = new Article()
            {
                Id = 1,
                Name = "HDD1",
                UnitPrice = 750
            };

            await Assert.ThrowsAsync<ArgumentException>(() => sut.AddAsync(article));
        }

        [Fact]
        public async Task WillUpdateArticle()
        {
            IRepository<Article, int> sut = GetInMemoryForArticle();

            Article article1 = new Article()
            {
                Id = 10,
                Name = "HDD1",
                UnitPrice = 750
            };

            Article article2 = new Article()
            {
                Id = 10,
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
            IRepository<Article, int> sut = GetInMemoryForArticle();

            Article article = new Article()
            {
                Id = 5,
                Name = "HDD2",
                UnitPrice = 755
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => sut.UpdateAsync(article));
        }

        [Fact]
        public async Task DeleteArticleNonExistingId()
        {
            IRepository<Article, int> sut = GetInMemoryForArticle();

            int id = 2;

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.DeleteAsync(id));
        }

        [Fact]
        public async Task WillDeleteArticle()
        {
            IRepository<Article, int> sut = GetInMemoryForArticle();

            int id = 1;

            int result = await sut.DeleteAsync(id);

            Assert.True(result > 0);
        }

        [Fact]
        public async Task WillCreateOfferArticle()
        {
            IRepository<OfferArticle, int> sut = GetInMemoryForOfferArticle();

            Article article1 = new Article()
            {
                Id = 0,
                Name = "HDD1",
                UnitPrice = 700
            };

            Offer offer = new Offer()
            {
                Id = 1,
                Number = 5,
                TotalPrice = 1500
            };

            OfferArticle offerArticle = new OfferArticle()
            {
                Id = 0,
                Article = article1,
                ArticleId = article1.Id,
                Offer = offer,
                OfferId = 1
            };

            int result = await sut.AddAsync(offerArticle);

            Assert.True(result > 0);
        }

        [Fact]
        public async Task EnsureRelatedDataExists()
        {
            int saved = 0;

            IRepository<OfferArticle, int> sut = GetInMemoryForOfferArticle();

            Article article1 = new Article()
            {
                Id = 10,
                Name = "HDD7",
                UnitPrice = 700
            };

            Offer offer = new Offer()
            {
                Id = 2,
                Number = 6,
                TotalPrice = 1500
            };

            OfferArticle offerArticle1 = new OfferArticle()
            {
                Id = 1,
                Article = article1,
                ArticleId = article1.Id,
                Offer = offer,
                OfferId = offer.Id,
            };

            if(await sut.AddAsync(offerArticle1) > 0)
            {
                saved++;
            }

            Article article2 = new Article()
            {
                Id = 12,
                Name = "SSD3",
                UnitPrice = 1000
            };

            OfferArticle offerArticle2 = new OfferArticle()
            {
                Id = 2,
                Article = article2,
                ArticleId = article2.Id,
                Offer = offer,
                OfferId = offer.Id
            };

            if (await sut.AddAsync(offerArticle2) > 0)
            {
                saved++;
            }

            Assert.True(saved.Equals(2));
        }
    }
}
