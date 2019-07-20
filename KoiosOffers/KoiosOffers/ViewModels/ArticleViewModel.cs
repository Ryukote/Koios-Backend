using KoiosOffers.Contracts;
using KoiosOffers.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.ViewModels
{
    public class ArticleViewModel : IViewModel
    {
        [JsonProperty]
        public int Id { get; set; }
        [Required]
        [JsonProperty]
        public string Name { get; set; }
        [Required]
        [JsonProperty]
        public decimal UnitPrice { get; set; }

        public ICollection<OfferArticleViewModel> OfferArticles { get; set; }
    }
}
