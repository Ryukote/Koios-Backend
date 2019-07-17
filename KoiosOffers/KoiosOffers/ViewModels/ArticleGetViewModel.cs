using KoiosOffers.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.ViewModels
{
    public class ArticleGetViewModel : IViewModel
    {
        public string Name { get; set; } = "";
        public decimal? UnitPrice { get; set; } = default;
        public int? Skip { get; set; } = default;
        public int? Take { get; set; } = default;
    }
}
