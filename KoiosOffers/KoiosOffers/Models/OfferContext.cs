using Microsoft.EntityFrameworkCore;

namespace KoiosOffers.Models
{
    public class OfferContext : DbContext
    {
        public OfferContext(DbContextOptions<OfferContext> options) : base(options)
        {
        }

        public DbSet<Article> Article { get; set; }
        public DbSet<Offer> Offer { get; set; }
        public DbSet<OfferArticle> OfferArticle { get; set; }

        /// <summary>
        /// Using Fluent API to define tables in db.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfferArticle>()
                .HasKey(oa => new { oa.Id, oa.OfferId, oa.ArticleId });
            modelBuilder.Entity<OfferArticle>()
                .HasOne(oa => oa.Offer)
                .WithMany()
                .HasForeignKey(oa => oa.OfferId);
            modelBuilder.Entity<OfferArticle>()
                .HasOne(oa => oa.Article)
                .WithMany()
                .HasForeignKey(oa => oa.ArticleId);
        }
    }
}
