using KoiosOffers.Contracts;
using KoiosOffers.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.ViewModels
{
    public class OfferArticleViewModel : IId<int>, IViewModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int OfferId { get; set; }
        public ArticleViewModel Article { get; set; }
        public OfferViewModel Offer { get; set; }
    }
}
