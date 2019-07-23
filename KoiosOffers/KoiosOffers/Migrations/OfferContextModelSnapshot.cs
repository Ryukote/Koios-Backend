﻿// <auto-generated />
using System;
using KoiosOffers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KoiosOffers.Migrations
{
    [DbContext(typeof(OfferContext))]
    partial class OfferContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KoiosOffers.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("Id");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("KoiosOffers.Models.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Number");

                    b.Property<decimal>("TotalPrice");

                    b.HasKey("Id");

                    b.ToTable("Offer");
                });

            modelBuilder.Entity("KoiosOffers.Models.OfferArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArticleId");

                    b.Property<int>("OfferId");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("OfferId");

                    b.ToTable("OfferArticle");
                });

            modelBuilder.Entity("KoiosOffers.Models.OfferArticle", b =>
                {
                    b.HasOne("KoiosOffers.Models.Article", "Article")
                        .WithMany("OfferArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KoiosOffers.Models.Offer", "Offer")
                        .WithMany("OfferArticles")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
