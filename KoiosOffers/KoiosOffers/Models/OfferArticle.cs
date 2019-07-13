namespace KoiosOffers.Models
{
    public class OfferArticle
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
