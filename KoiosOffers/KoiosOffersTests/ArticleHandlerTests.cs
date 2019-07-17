using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace KoiosOffersTests
{
    public class ArticleHandlerTests
    {
        private static string _databasename = Guid.NewGuid().ToString();

        private static IArticleHandler<int> GetInMemoryForArticle()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: "Test");
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new ArticleHandler<int>(offerContext);
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

            Assert.False(article == null);
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

            Assert.False(article == null);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task WillUpdateArticle()
        {
            var sut = GetInMemoryForArticle();

            var id = 0;

            ArticleViewModel article = new ArticleViewModel()
            {
                Id = 10,
                Name = "HDD1",
                UnitPrice = 750
            };

            ArticleViewModel updatedArticle = new ArticleViewModel()
            {
                Id = 10,
                Name = "HDD2",
                UnitPrice = 755
            };

            await sut.AddAsync(article);

            var newSut = GetInMemoryForArticle();

            int result = await newSut.UpdateAsync(updatedArticle);

            Assert.False(article == null);
            Assert.False(updatedArticle == null);
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

            Assert.False(article == null);
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

            Assert.False(article == null);
            Assert.True(deleted.Equals(0));
        }
    }
}
