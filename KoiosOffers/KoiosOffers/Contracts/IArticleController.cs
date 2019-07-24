using KoiosOffers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KoiosOffers.Contracts
{
    public interface IArticleController
    {
        [HttpGet]
        Task<IActionResult> GetById([FromQuery]int id);

        [HttpGet]
        Task<IActionResult> GetPaginatedAsync([FromQuery]string name, [FromQuery]int skip, [FromQuery]int take);

        [HttpPost]
        Task<IActionResult> Post([FromBody]ArticleViewModel articleViewModel);

        [HttpPut]
        Task<IActionResult> Put([FromBody]ArticleViewModel articleViewModel);

        [HttpDelete]
        Task<IActionResult> Delete([FromQuery]int id);
    }
}
