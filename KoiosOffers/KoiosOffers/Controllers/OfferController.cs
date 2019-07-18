using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiosOffers.Contracts;
using KoiosOffers.Data;
using KoiosOffers.Models;
using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KoiosOffers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase, IOfferController
    {
        private OfferHandler _offer;
        private OfferContext _offerContext;

        public OfferController(OfferContext offerContext)
        {
            _offerContext = offerContext;
            _offer = new OfferHandler(_offerContext);
        }

        public OfferController()
        {
            _offer = new OfferHandler(new OfferContext(new DbContextOptions<OfferContext>()));
        }

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
        public async Task<IActionResult> GetPaginatedAsync([FromQuery]int skip, [FromQuery]int take)
        {
            var result = await _offer.GetPaginatedAsync(take, skip);

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

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]OfferViewModel offerViewModel)
        {
            var result = await _offer.UpdateAsync(offerViewModel);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            var result = await _offer.DeleteAsync(id);

            if (result > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}