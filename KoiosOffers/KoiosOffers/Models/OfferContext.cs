using Microsoft.EntityFrameworkCore;

namespace KoiosOffers.Models
{
    public class OfferContext : DbContext
    {
        public OfferContext(DbContextOptions<OfferContext> options) : base(options)
        {
        }

        /// <summary>
        /// Using Fluent API to define tables in db.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfferArticle>()
                .HasKey(oa => new { oa.ArticleId, oa.OfferId });
            modelBuilder.Entity<OfferArticle>()
                .HasOne(oa => oa.Offer)
                .WithMany(a => a.OfferArticles)
                .HasForeignKey(oa => oa.OfferId);
        }
    }
}
