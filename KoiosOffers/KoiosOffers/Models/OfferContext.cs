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
        }
    }
}
