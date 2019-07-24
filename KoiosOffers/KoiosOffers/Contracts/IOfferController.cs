using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IOfferController
    {
        [HttpGet]
        Task<IActionResult> GetById([FromQuery]int id);

        [HttpGet]
        Task<IActionResult> GetOfferArticlesByIdAsync([FromQuery]int offerId);

        [HttpPost]
        Task<IActionResult> Post([FromBody]OfferViewModel offerViewModel);

        [HttpPost]
        Task<IActionResult> PostOfferArticle([FromBody]OfferArticleViewModel offerArticleViewModel);

        [HttpDelete]
        Task<IActionResult> Delete([FromQuery]int id);

        [HttpDelete]
        Task<IActionResult> DeleteOfferArticle([FromQuery]int offerId, [FromQuery]int articleId);
    }
}
