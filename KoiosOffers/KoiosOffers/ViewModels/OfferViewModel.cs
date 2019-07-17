﻿using KoiosOffers.Contracts;
using KoiosOffers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.ViewModels
{
    public class OfferViewModel : IId<int>, IViewModel
    {
        public OfferViewModel()
        {
            Articles = new List<ArticleViewModel>();
        }


        [Required]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }

        public ICollection<ArticleViewModel> Articles { get; set; }
    }
}