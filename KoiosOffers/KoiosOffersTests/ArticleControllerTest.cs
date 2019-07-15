﻿using KoiosOffers.Controllers;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace KoiosOffersTests
{
    public class ArticleControllerTest
    {
        public OfferContext SetupContext()
        {
            DbContextOptions<OfferContext> options;
            var builder = new DbContextOptionsBuilder<OfferContext>();
            builder.UseInMemoryDatabase(databaseName: "KoiosArticleController");
            options = builder.Options;
            OfferContext offerContext = new OfferContext(options);
            offerContext.Database.EnsureCreated();

            return offerContext;
        }

        [Fact]
        public async Task WillAddArticle()
        {
            ArticleController controller = new ArticleController(SetupContext());

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Id = 5,
                Name = "TestArticle",
                UnitPrice = 700
            };

            var result = await controller.Post(viewModel);

            Assert.NotNull(result);
            Assert.True(result is ObjectResult);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task WillNotAddArticle()
        {
            ArticleController controller = new ArticleController(SetupContext());

            ArticleViewModel viewModel = new ArticleViewModel()
            {
            };

            var result = await controller.Post(viewModel);

            Assert.NotNull(result);
            Assert.True(result is BadRequestResult);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestResult)result).StatusCode);
        }
    }
}
