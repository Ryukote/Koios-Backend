using KoiosOffers.Contracts;
using KoiosOffers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.ViewModels
{
    public class ArticleViewModel : IViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        public ICollection<OfferArticleViewModel> OfferArticles { get; set; }
    }
}
