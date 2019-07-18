using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KoiosOffersTests
{
    public class ArticleHandlerTests
    {
        private static IArticleHandler GetInMemoryForArticle(string databaseName)
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: databaseName);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new ArticleHandler(offerContext);
        }

        [Fact]
        public async Task WillCreateArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            int result = await sut.AddAsync(article);

            Assert.False(article == null);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task ArticleAlreadyExists()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            await sut.AddAsync(article);

            var result = await sut.AddAsync(article);

            Assert.False(article == null);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task WillUpdateArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel updatedArticle = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 755
            };

            await sut.AddAsync(article);

            var newSut = GetInMemoryForArticle(databaseName);

            int result = await newSut.UpdateAsync(updatedArticle);

            Assert.False(article == null);
            Assert.False(updatedArticle == null);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task UpdateArticleNonExistingId()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = 15,
                Name = "HDD2",
                UnitPrice = 755
            };

            Assert.False(article == null);
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => sut.UpdateAsync(article));
        }

        [Fact]
        public async Task DeleteArticleNonExistingId()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            var id = 75;

            await Assert.ThrowsAsync<InvalidOperationException>(() => sut.DeleteAsync(id));
        }

        [Fact]
        public async Task WillDeleteArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            var addedId = await sut.AddAsync(article);

            int deleted = await sut.DeleteAsync(addedId);

            Assert.False(article == null);
            Assert.True(deleted > 0);
        }

        [Fact]
        public async Task WillReturnFirstTwoRecords()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            var result = await sut.GetPaginatedAsync("HDD", 2, 0);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task WillReturnLastTwoRecords()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            var result = await sut.GetPaginatedAsync("HDD", 2, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task OutofRangeRecords()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            var result = await sut.GetPaginatedAsync("HDD", 7, 10);

            Assert.Empty(result);
        }

        [Fact]
        public async Task WillReturnWithOutOfRangeRecords()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            var result = await sut.GetPaginatedAsync("HDD", 7, 0);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task NameNullThrowsException()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetPaginatedAsync(null, 7, 0));
        }

        [Fact]
        public async Task NameEmptyReturnsSelected()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            var result = await sut.GetPaginatedAsync("", 7, 0);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task NameEmptyReturnsLastTwo()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            var result = await sut.GetPaginatedAsync("", 2, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task NameEmptyReturnsFirstTwo()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForArticle(databaseName);

            ArticleViewModel article1 = new ArticleViewModel()
            {
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel article2 = new ArticleViewModel()
            {
                Name = "HDD2",
                UnitPrice = 750
            };

            ArticleViewModel article3 = new ArticleViewModel()
            {
                Name = "HDD3",
                UnitPrice = 750
            };

            await sut.AddAsync(article1);
            await sut.AddAsync(article2);
            await sut.AddAsync(article3);

            var result = await sut.GetPaginatedAsync("", 2, 0);

            Assert.Equal(2, result.Count());
        }
    }
}
