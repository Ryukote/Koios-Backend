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
        public async Task WillUpdateOffer()
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

            offer.Id = offerAddValue;
            offer.Number = 7;

            OfferController newOfferController = new OfferController(handler2);

            var offerUpdateResult = await newOfferController.Put(offer);

            Assert.NotNull(offerAddResult);
            Assert.NotNull(offerUpdateResult);
            Assert.True(offerAddResult is ObjectResult);
            Assert.True(offerUpdateResult is NoContentResult);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)offerAddResult).StatusCode);
            Assert.Equal(StatusCodes.Status204NoContent, ((NoContentResult)offerUpdateResult).StatusCode);
            Assert.True(offerAddValue > 0);
        }

        [Fact]
        public async Task WillNotUpdateOffer()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new OfferHandler(SetupContext(databaseName));

            OfferController offerController = new OfferController(handler);

            OfferViewModel offer = new OfferViewModel()
            {
                Id = 755,
                CreatedAt = DateTime.UtcNow,
                Number = 1,
                TotalPrice = 70
            };

            var offerUpdateResult = await offerController.Put(offer);

            Assert.NotNull(offerUpdateResult);
            Assert.True(offerUpdateResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)offerUpdateResult).StatusCode);
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
        public async Task WillGetAllOffers()
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

            OfferViewModel viewModel2 = new OfferViewModel()
            {
                CreatedAt = DateTime.UtcNow,
                Number = 2,
                TotalPrice = 70
            };

            var result = await controller.Post(viewModel);
            var result2 = await controller.Post(viewModel2);
            var allItems = await controller.GetAll();

            var statusCode = ((ObjectResult)result).StatusCode;
            var statusCode2 = ((ObjectResult)result2).StatusCode;
            var statusCodeGetAll = ((ObjectResult)allItems).StatusCode;

            var json = ((ObjectResult)allItems).Value;

            ICollection<OfferViewModel> offerCollection = JsonConvert.DeserializeObject<ICollection<OfferViewModel>>(json.ToString());

            Assert.NotNull(result);
            Assert.NotNull(result2);
            Assert.True(result is ObjectResult);
            Assert.True(result2 is ObjectResult);
            Assert.True(allItems is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status201Created, statusCode2);
            Assert.Equal(StatusCodes.Status200OK, statusCodeGetAll);
            Assert.Equal(2, offerCollection.Count);
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
