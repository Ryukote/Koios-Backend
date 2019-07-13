using System.Collections.Generic;

namespace KoiosOffers.Models
{
    /// <summary>
    /// Article model
    /// </summary>
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public ICollection<OfferArticle> OfferArticles { get; set; }
    }
}
