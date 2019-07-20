using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IOfferController
    {
        [HttpGet]
        Task<IActionResult> GetAll();

        [HttpGet]
        Task<IActionResult> GetById([FromQuery]int id);

        [HttpGet]
        Task<IActionResult> GetPaginatedAsync([FromQuery]int offerNumber, [FromQuery]int skip, [FromQuery]int take);

        [HttpGet]
        Task<IActionResult> GetOfferByOfferNumberAsync(int offerNumber);

        [HttpPost]
        Task<IActionResult> Post([FromBody]OfferViewModel offerViewModel);

        [HttpPut]
        Task<IActionResult> Put([FromBody]OfferViewModel offerViewModel);

        [HttpDelete]
        Task<IActionResult> Delete([FromQuery]int id);
    }
}
