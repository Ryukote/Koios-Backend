using KoiosOffers.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.Models
{
    public class Offer : IId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public decimal TotalPrice { get; set; }
        public ICollection<OfferArticle> OfferArticles { get; set; }
    }
}
