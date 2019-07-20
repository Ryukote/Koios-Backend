using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiosOffers.Controllers
{
    [Route("api/[controller]/[action]")]
    //[EnableCors("AllowOrigin")]
    [ApiController]
    public class OfferController : ControllerBase, IOfferController
    {
        private IOfferHandler _offer;

        public OfferController(IOfferHandler offer)
        {
            _offer = offer;
        }

        //public OfferController()
        //{
        //    _offer = new OfferHandler(new OfferContext(new DbContextOptions<OfferContext>()));
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _offer.GetAllAsync();

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
            var result = await _offer.GetByIdAsync(id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        [ActionName("GetPaginated")]
        public async Task<IActionResult> GetPaginatedAsync([FromQuery]int offerNumber, [FromQuery]int skip, [FromQuery]int take)
        {
            var result = await _offer.GetPaginatedAsync(offerNumber, take, skip);

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
        [ActionName("GetOfferByOfferNumber")]
        public async Task<IActionResult> GetOfferByOfferNumberAsync(int offerNumber)
        {
            var result = await _offer.GetOfferByOfferNumberAsync(offerNumber);

            if(result == null)
            {
                return BadRequest();
            }

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        [ActionName("GetOfferArticles")]
        public async Task<IActionResult> GetOfferArticlesByIdAsync([FromQuery]int offerId)
        {
            if(offerId > 0)
            {
                IEnumerable<ArticleViewModel> list = await _offer.GetOfferArticlesByIdAsync(offerId);

                if(list.Count().Equals(0))
                {
                    return NoContent();
                }

                return Ok(JsonConvert.SerializeObject(list));
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OfferViewModel offerViewModel)
        {
            if (offerViewModel.Number < 1)
            {
                return BadRequest();
            }

            offerViewModel.CreatedAt = DateTime.UtcNow;

            var result = await _offer.AddAsync(offerViewModel);

            if (result > 0)
            {
                return Created("", offerViewModel.Number);
            }

            return BadRequest();
        }

        [HttpPost]
        [ActionName("AddOfferArticle")]
        public async Task<IActionResult> PostOfferArticle([FromBody]OfferArticleViewModel offerArticleViewModel)
        {
            if (offerArticleViewModel.ArticleId.Equals(0) || offerArticleViewModel.OfferId.Equals(0))
            {
                return BadRequest();
            }

            var result = await _offer.AddOfferArticleAsync(offerArticleViewModel.OfferId, offerArticleViewModel.ArticleId);

            if (result > 0)
            {
                return Created("", result);
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]OfferViewModel offerViewModel)
        {
            var result = 0;

            try
            {
                result = await _offer.UpdateAsync(offerViewModel);
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest("You can't update record that doesn't exist.");
            }

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            var result = 0;

            try
            {
                result = await _offer.DeleteAsync(id);
            }
            catch(ArgumentException)
            {
                return BadRequest("There is no offer you want to delete.");
            }

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        [ActionName("DeleteOfferArticle")]
        public async Task<IActionResult> DeleteOfferArticle([FromQuery]int offerId, [FromQuery]int articleId)
        {
            var result = 0;

            try
            {
                result = await _offer.DeleteOfferArticle(offerId, articleId);
            }
            catch (ArgumentException)
            {
                return BadRequest("There is no article under that offer you want to delete.");
            }

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}