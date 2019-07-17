using KoiosOffers.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.Models
{
    public class OfferArticle : IId<int>, IViewModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
