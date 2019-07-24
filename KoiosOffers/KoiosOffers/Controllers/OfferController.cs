using KoiosOffers.Contracts;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiosOffers.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OfferController : ControllerBase, IOfferController
    {
        private IOfferHandler _offer;

        public OfferController(IOfferHandler offer)
        {
            _offer = offer;
        }

        [HttpGet]
        [ActionName("GetById")]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            var result = await _offer.GetByIdAsync(id);

            if (result == null)
            {
                return BadRequest();
            }

            //bilo je samo Ok(result)
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

                return Ok(list);
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ActionName("Create")]
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
                return Created("", result);
            }

            return BadRequest();
        }

        [HttpPost]
        [ActionName("AddOfferArticle")]
        public async Task<IActionResult> PostOfferArticle([FromBody]OfferArticleViewModel offerArticleViewModel)
        {
            if (offerArticleViewModel.ArticleId.Equals(0) 
                || offerArticleViewModel.OfferId.Equals(0))
            {
                return BadRequest();
            }

            var result = await _offer.AddOfferArticleAsync
                (offerArticleViewModel.OfferId, offerArticleViewModel.ArticleId);

            if (result > 0)
            {
                return Created("", result);
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