using KoiosOffers.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoiosOffers.ViewModels
{
    public class ArticleViewModel : IId<int>, IViewModel, IValidatableObject
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this == null)
            {
                yield return new ValidationResult("Article not provided");
            }
        }
    }
}
