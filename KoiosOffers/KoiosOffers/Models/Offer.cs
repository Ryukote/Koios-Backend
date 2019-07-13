﻿using KoiosOffers.Contracts;
using System;
using System.Collections.Generic;

namespace KoiosOffers.Models
{
    public class Offer : IId<int>
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OfferArticle> OfferArticles { get; set; }
    }
}