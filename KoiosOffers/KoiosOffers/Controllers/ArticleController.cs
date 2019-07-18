﻿using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KoiosOffers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase, IArticleController
    {
        private ArticleHandler _article;
        private OfferContext _offerContext;

        public ArticleController(OfferContext offerContext)
        {
            _offerContext = offerContext;
            _article = new ArticleHandler(_offerContext);
        }

        public ArticleController()
        {
            _article = new ArticleHandler(new OfferContext(new DbContextOptions<OfferContext>()));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _article.GetAllAsync();

            if (result.Count().Equals(0))
            {
                return NoContent();
            }

            else if (result == null)
            {
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            var result = await _article.GetByIdAsync(id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetIdByName([FromQuery]string name)
        {
            var result = await _article.GetIdByNameAsync(name);

            if (result < 1)
            {
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedAsync([FromQuery]string name, [FromQuery]int skip, [FromQuery]int take)
        {
            var result = await _article.GetPaginatedAsync(name, take, skip);

            if (result.Count().Equals(0))
            {
                return NoContent();
            }

            else if (result == null)
            {
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ArticleViewModel articleViewModel)
        {
            if (articleViewModel.Name == null)
            {
                return BadRequest();
            }

            var result = await _article.AddAsync(articleViewModel);

            if (result > 0)
            {
                return Created("", result);
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ArticleViewModel articleViewModel)
        {
            ArticleHandler _newArticle = new ArticleHandler(_offerContext);
            var result = await _newArticle.UpdateAsync(articleViewModel);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            try
            {
                await _article.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }
    }
}