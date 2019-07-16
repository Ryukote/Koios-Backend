﻿using KoiosOffers.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiosOffers.Models
{
    public class Offer : IId<int>, IViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }

        public ICollection<OfferArticle> OfferArticles { get; set; }
    }
}
