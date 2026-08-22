using MarketStack.Data.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Data;

public class MarketStackContext : DbContext
{
    public DbSet<Receipt> Receipt { get; set; }

    public DbSet<Product> Product { get; set; }

    public DbSet<ReceiptItem> ReceiptItem { get; set; }

    public DbSet<Tag> Tag { get; set; }

    public DbSet<ProductTag> ProductTag { get; set; }

    public MarketStackContext(DbContextOptions<MarketStackContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Receipt>(r =>
        {
            r.ToTable("receipt");
            r.HasKey(x => x.Id);

            r.HasMany(x => x.Items)
                .WithOne(x => x.Receipt)
                .HasForeignKey(x => x.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(p =>
        {
            p.ToTable("product");
            p.HasKey(x => x.Id);

            p.HasMany<ReceiptItem>()
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReceiptItem>(ri =>
        {
            ri.ToTable("receipt_item");
            ri.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Tag>(t =>
        {
            t.ToTable("tag");
            t.HasKey(x => x.Id);

            t.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<ProductTag>(pt =>
        {
            pt.ToTable("product_tag");
            pt.HasKey(x => new
            {
                x.ProductId,
                x.TagId
            });

            pt.HasOne(x => x.Product)
                .WithMany(x => x.ProductTags)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            pt.HasOne(x => x.Tag)
                .WithMany(x => x.ProductTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}