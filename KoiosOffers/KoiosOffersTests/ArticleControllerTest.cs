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
    public class ArticleControllerTest
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
        public async Task WillAddArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);
            var value = Convert.ToInt32(((ObjectResult)result).Value);

            Assert.NotNull(result);
            Assert.True(result is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)result).StatusCode);
            Assert.True(value > 0);
        }

        [Fact]
        public async Task WillNotAddArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel();

            var result = await controller.Post(viewModel);

            Assert.NotNull(result);
            Assert.True(result is BadRequestResult);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestResult)result).StatusCode);
        }

        [Fact]
        public async Task WillGetAllArticles()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle",
                UnitPrice = 700
            };

            ArticleViewModel viewModel2 = new ArticleViewModel()
            {
                Name = "TestArticle2",
                UnitPrice = 770
            };

            var result = await controller.Post(viewModel);
            var result2 = await controller.Post(viewModel2);
            var allItems = await controller.GetAll();

            var statusCode = ((ObjectResult)result).StatusCode;
            var statusCode2 = ((ObjectResult)result2).StatusCode;
            var statusCodeGetAll = ((ObjectResult)allItems).StatusCode;

            var json = ((ObjectResult)allItems).Value;

            Assert.NotNull(result);
            Assert.NotNull(result2);
            Assert.True(result is ObjectResult);
            Assert.True(result2 is ObjectResult);
            Assert.True(allItems is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status201Created, statusCode2);
            Assert.Equal(StatusCodes.Status200OK, statusCodeGetAll);
        }

        [Fact]
        public async Task WillGetArticleByFilter()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle",
                UnitPrice = 700
            };

            ArticleViewModel viewModel2 = new ArticleViewModel()
            {
                Name = "NotGettingBackArticle",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);
            var result2 = await controller.Post(viewModel2);

            var statusCode = ((ObjectResult)result).StatusCode;
            var value = ((ObjectResult)result).Value;

            var statusCode2 = ((ObjectResult)result2).StatusCode;
            var value2 = ((ObjectResult)result2).Value;

            var filteredResult = await controller.GetPaginatedAsync("Test", 0, 1);
            var filteredStatusCode = ((ObjectResult)filteredResult).StatusCode;
            var filteredValue = ((ObjectResult)filteredResult).Value;

            Assert.NotNull(value);
            Assert.NotNull(value2);
            Assert.NotNull(filteredValue);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status201Created, statusCode2);
            Assert.Equal(StatusCodes.Status200OK, filteredStatusCode);
        }

        [Fact]
        public async Task WillGetArticleByPaginationSkip()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle1",
                UnitPrice = 700
            };

            ArticleViewModel viewModel2 = new ArticleViewModel()
            {
                Name = "TestArticle2",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);
            var result2 = await controller.Post(viewModel2);

            var statusCode = ((ObjectResult)result).StatusCode;
            var value = ((ObjectResult)result).Value;

            var statusCode2 = ((ObjectResult)result2).StatusCode;
            var value2 = ((ObjectResult)result2).Value;

            var filteredResult = await controller.GetPaginatedAsync("TestArticle", 1, 1);
            var filteredStatusCode = ((ObjectResult)filteredResult).StatusCode;
            var filteredValue = ((ObjectResult)filteredResult).Value;

            var filteredArticleName = JsonConvert.DeserializeObject<List<ArticleViewModel>>(filteredValue.ToString());

            Assert.NotNull(value);
            Assert.NotNull(value2);
            Assert.NotNull(filteredValue);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status201Created, statusCode2);
            Assert.Equal(StatusCodes.Status200OK, filteredStatusCode);
            Assert.Equal("TestArticle2", filteredArticleName[0].Name);
        }

        [Fact]
        public async Task WillGetArticleByPaginationTake()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle1",
                UnitPrice = 700
            };

            ArticleViewModel viewModel2 = new ArticleViewModel()
            {
                Name = "TestArticle2",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);
            var result2 = await controller.Post(viewModel2);

            var statusCode = ((ObjectResult)result).StatusCode;
            var value = ((ObjectResult)result).Value;

            var statusCode2 = ((ObjectResult)result2).StatusCode;
            var value2 = ((ObjectResult)result2).Value;

            var filteredResult = await controller.GetPaginatedAsync("TestArticle", 0, 1);
            var filteredStatusCode = ((ObjectResult)filteredResult).StatusCode;
            var filteredValue = ((ObjectResult)filteredResult).Value;

            var filteredJson = JsonConvert.DeserializeObject<dynamic>(filteredValue.ToString());

            Assert.NotNull(value);
            Assert.NotNull(value2);
            Assert.NotNull(filteredValue);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status201Created, statusCode2);
            Assert.Equal(StatusCodes.Status200OK, filteredStatusCode);
            Assert.Equal("TestArticle1", filteredJson[0].Name.ToString());
        }

        [Fact]
        public async Task WillGetArticleById()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);

            var resultJson = ((ObjectResult)result).Value;

            var resultName = Convert.ToInt32(resultJson);

            var findById = await controller.GetById(resultName);

            var statusCode = ((ObjectResult)result).StatusCode;
            var statusCode2 = ((ObjectResult)findById).StatusCode;

            Assert.NotNull(result);
            Assert.NotNull(findById);
            Assert.True(result is ObjectResult);
            Assert.True(findById is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status200OK, statusCode2);
        }

        [Fact]
        public async Task WillUpdateArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));
            var handler2 = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);
            ArticleController controller2 = new ArticleController(handler2);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);
            var resultId = Convert.ToInt32(((ObjectResult)result).Value);
            var statusCode = ((ObjectResult)result).StatusCode;

            viewModel.Id = resultId;
            viewModel.UnitPrice = 750;

            var update = await controller2.Put(viewModel);
            var updateStatusCode = ((NoContentResult)update).StatusCode;

            Assert.NotNull(result);
            Assert.NotNull(update);
            Assert.True(result is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, statusCode);
            Assert.Equal(StatusCodes.Status204NoContent, updateStatusCode);
        }

        [Fact]
        public async Task WillDeleteArticle()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Name = "TestArticle",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);

            var value = Convert.ToInt32(((ObjectResult)result).Value);

            var delete = await controller.Delete(value);

            Assert.NotNull(result);
            Assert.NotNull(delete);
            Assert.True(result is ObjectResult);
            Assert.True(delete is NoContentResult);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)result).StatusCode);
            Assert.Equal(StatusCodes.Status204NoContent, ((NoContentResult)delete).StatusCode);
        }

        [Fact]
        public async Task WillNotDelete()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            var result = await controller.Delete(75);

            var statusCode = ((BadRequestResult)result).StatusCode;

            Assert.NotNull(result);
            Assert.True(result is BadRequestResult);
            Assert.Equal(StatusCodes.Status400BadRequest, statusCode);
        }

        [Fact]
        public async Task WillNotUpdate()
        {
            var databaseName = Guid.NewGuid().ToString();

            var handler = new ArticleHandler(SetupContext(databaseName));

            ArticleController controller = new ArticleController(handler);

            ArticleViewModel viewModel = new ArticleViewModel();

            var result = await controller.Put(viewModel);

            var statusCode = ((BadRequestResult)result).StatusCode;

            Assert.NotNull(result);
            Assert.True(result is BadRequestResult);
            Assert.Equal(StatusCodes.Status400BadRequest, statusCode);
        }
    }
}
