using KoiosOffers.Contracts;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        private IArticleHandler _article;

        public ArticleController(IArticleHandler articleHandler)
        {
            _article = articleHandler;
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
            try
            {
                await _article.DeleteAsync(id);
                return NoContent();
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }
    }
}