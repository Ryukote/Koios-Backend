using KoiosOffers.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.ViewModels
{
    public class OfferArticleViewModel : IViewModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int OfferId { get; set; }
    }
}
