using KoiosOffers.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.Models
{
    /// <summary>
    /// Article model
    /// </summary>
    public class Article : IViewModel
    {
        public Article()
        {
            OfferArticles = new List<OfferArticle>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        public virtual ICollection<OfferArticle> OfferArticles { get; set; }
    }
}
