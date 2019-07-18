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
    public class OfferHandlerTests
    {
        private static IOfferHandler GetInMemoryForOffer(string databaseName)
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: databaseName);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();
            return new OfferHandler(offerContext);
        }

        [Fact]
        public async Task WillCreateOffer()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForOffer(databaseName);

            OfferViewModel offer = new OfferViewModel()
            {
                CreatedAt = DateTime.UtcNow,
                Number = 1,
                TotalPrice = 70
            };

            int result = await sut.AddAsync(offer);

            Assert.False(offer == null);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task OfferAlreadyExists()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForOffer(databaseName);

            OfferViewModel offer = new OfferViewModel()
            {
                CreatedAt = DateTime.UtcNow,
                Number = 2,
                TotalPrice = 70
            };

            await sut.AddAsync(offer);

            var resultId = await sut.AddAsync(offer);

            offer.Id = resultId;

            var result = await sut.AddAsync(offer);

            Assert.False(offer == null);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task WillUpdateOffer()
        {
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForOffer(databaseName);

            OfferViewModel offer = new OfferViewModel()
            {
                //Id = id,
                CreatedAt = DateTime.UtcNow,
                Number = 2,
                TotalPrice = 70
            };

            OfferViewModel updatedOffer = new OfferViewModel()
            {
                //Id = id,
                CreatedAt = DateTime.UtcNow,
                Number = 2,
                TotalPrice = 70
            };

            var resultId = await sut.AddAsync(offer);

            updatedOffer.Id = resultId;

            var newSut = GetInMemoryForOffer(databaseName);

            int result = await newSut.UpdateAsync(updatedOffer);

            Assert.False(offer == null);
            Assert.False(updatedOffer == null);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task UpdateOfferNonExistingId()
        {
            int id = 1000;
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForOffer(databaseName);

            OfferViewModel updatedOffer = new OfferViewModel()
            {
                Id = id,
                CreatedAt = DateTime.UtcNow,
                Number = 2,
                TotalPrice = 70
            };

            Assert.False(updatedOffer == null);
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => sut.UpdateAsync(updatedOffer));
        }

        [Fact]
        public async Task DeleteOfferNonExistingId()
        {
            var id = 555;
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForOffer(databaseName);

            await Assert.ThrowsAsync<ArgumentException>(() => sut.DeleteAsync(id));
        }

        [Fact]
        public async Task WillDeleteOffer()
        {
            //var id = 1;
            var databaseName = Guid.NewGuid().ToString();

            var sut = GetInMemoryForOffer(databaseName);

            OfferViewModel offer = new OfferViewModel()
            {
                //Id = id,
                CreatedAt = DateTime.UtcNow,
                Number = 2,
                TotalPrice = 70
            };

            int saved = await sut.AddAsync(offer);

            int deleted = await sut.DeleteAsync(saved);

            Assert.False(offer == null);
            Assert.True(deleted.Equals(1));
        }
    }
}
