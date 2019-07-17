using KoiosOffers.Contracts;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.Models
{
    public class OfferArticle : IId<int>, IViewModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
