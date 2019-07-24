using KoiosOffers.Controllers;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KoiosOffersTests
{
    public class OfferControllerTests
    {
        public OfferContext SetupContext(string databaseName)
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: databaseName);
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();

            return offerContext;
        }

        [Fact]
        public async Task WillAddOffer()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new OfferHandler(SetupContext(databaseName));

            OfferController offerController = new OfferController(handler);

            OfferViewModel offer = new OfferViewModel()
            {
                CreatedAt = DateTime.UtcNow,
                Number = 1,
                TotalPrice = 70
            };

            var offerAddResult = await offerController.Post(offer);
            var offerAddValue = Convert.ToInt32(((ObjectResult)offerAddResult).Value);

            Assert.NotNull(offerAddResult);
            Assert.True(offerAddResult is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)offerAddResult).StatusCode);
            Assert.True(offerAddValue > 0);
        }

        [Fact]
        public async Task WillNotAddOffer()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new OfferHandler(SetupContext(databaseName));

            OfferController offerController = new OfferController(handler);

            OfferViewModel offer = new OfferViewModel();

            var offerAddResult = await offerController.Post(offer);

            Assert.NotNull(offerAddResult);
            Assert.True(offerAddResult is BadRequestResult);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestResult)offerAddResult).StatusCode);
        }

        [Fact]
        public async Task WillDeleteOffer()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new OfferHandler(SetupContext(databaseName));
            var handler2 = new OfferHandler(SetupContext(databaseName));

            OfferController offerController = new OfferController(handler);

            OfferViewModel offer = new OfferViewModel()
            {
                CreatedAt = DateTime.UtcNow,
                Number = 1,
                TotalPrice = 70
            };

            var offerAddResult = await offerController.Post(offer);
            var offerAddValue = Convert.ToInt32(((ObjectResult)offerAddResult).Value);

            OfferController newOfferController = new OfferController(handler2);

            var offerDeleteResult = await newOfferController.Delete(offerAddValue);

            Assert.NotNull(offerAddResult);
            Assert.NotNull(offerDeleteResult);
            Assert.True(offerAddResult is ObjectResult);
            Assert.True(offerDeleteResult is NoContentResult);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)offerAddResult).StatusCode);
            Assert.Equal(StatusCodes.Status204NoContent, ((NoContentResult)offerDeleteResult).StatusCode);
            Assert.True(offerAddValue > 0);
        }
        [Fact]
        public async Task WillNotDeleteOffer()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new OfferHandler(SetupContext(databaseName));

            OfferController offerController = new OfferController(handler);

            var offerDeleteResult = await offerController.Delete(100);

            Assert.NotNull(offerDeleteResult);
            Assert.True(offerDeleteResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)offerDeleteResult).StatusCode);
        }

        [Fact]
        public async Task WillGetOfferById()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new OfferHandler(SetupContext(databaseName));

            OfferController controller = new OfferController(handler);

            OfferViewModel viewModel = new OfferViewModel()
            {
                CreatedAt = DateTime.UtcNow,
                Number = 1,
                TotalPrice = 70
            };

            var result = await controller.Post(viewModel);

            var resultJson = ((ObjectResult)result).Value;

            var resultName = Convert.ToInt32(resultJson);

            var findById = await controller.GetById(resultName);

            var statusCode = ((ObjectResult)result).StatusCode;
            var statusCode2 = ((ObjectResult)findById).StatusCode;

            var jsonById = ((ObjectResult)findById).Value;

            var convertedJson = JsonConvert.DeserializeObject<object>(jsonById.ToString());

            Assert.NotNull(result);
            Assert.NotNull(findById);
            Assert.True(result is ObjectResult);
            Assert.True(findById is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status200OK, statusCode2);
            Assert.NotNull(convertedJson);
        }
    }
}
