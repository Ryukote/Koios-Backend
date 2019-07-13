using KoiosOffers.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiosOffers.Models
{
    /// <summary>
    /// Article model
    /// </summary>
    public class Article : IId<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public ICollection<OfferArticle> OfferArticles { get; set; }
    }
}
