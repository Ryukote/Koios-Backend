using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace KoiosOffers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private ArticleHandler<int> _article;
        private OfferContext _offerContext;

        public ArticleController(OfferContext offerContext)
        {
            _offerContext = offerContext;
            _article = new ArticleHandler<int>(_offerContext);
        }

        public ArticleController()
        {
            _article = new ArticleHandler<int>(new OfferContext(new DbContextOptions<OfferContext>()));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ArticleGetViewModel articleGetViewModel)
        {
            var results = await _article.GetAsync();

            if(results.ToList().Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(results));
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ArticleViewModel articleViewModel)
        {
            if(string.IsNullOrEmpty(articleViewModel.Name) || string.IsNullOrWhiteSpace(articleViewModel.Name))
            {
                return BadRequest();
            }

            var result = await _article.AddAsync(articleViewModel);

            if (result > 0)
            {
                return Created("", articleViewModel.Name);
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ArticleViewModel articleViewModel)
        {
            var result = await _article.UpdateAsync(articleViewModel);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            var result = await _article.DeleteAsync(id);

            if(result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}