using MarketStack.Data.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Data;

public class MarketStackContext : DbContext
{
    public DbSet<Receipt> Receipt { get; set; }

    public DbSet<Product> Product { get; set; }

    public DbSet<ReceiptItem> ReceiptItem { get; set; }

    public DbSet<ReceiptTotal> ReceiptTotal { get; set; }

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
                .HasForeignKey(x => x.ReceiptId);
        });

        modelBuilder.Entity<Product>(p =>
        {
            p.ToTable("product");
            p.HasKey(x => x.Id);

            p.HasMany<ReceiptItem>()
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
        });

        modelBuilder.Entity<ReceiptItem>(r =>
        {
            r.ToTable("receipt_item");
            r.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ReceiptTotal>(r =>
        {
            r.ToTable("receipt_total");
            r.HasKey(x => x.Id);

            r.HasOne<Receipt>()
                .WithOne()
                .HasForeignKey<ReceiptTotal>(x => x.Id);
        });
    }
}