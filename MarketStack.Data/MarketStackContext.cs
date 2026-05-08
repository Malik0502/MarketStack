using MarketStack.Data.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Data;

public class MarketStackContext : DbContext
{
    public DbSet<Receipt> Receipt { get; set; }

    public DbSet<Product> Product { get; set; }

    public DbSet<ReceiptItem> ReceiptItem { get; set; }

    public DbSet<ReceiptTotal> ReceiptTotal { get; set; }

    public DbSet<StoreChain> StoreChain { get; set; }

    public DbSet<StoreLocation> StoreLocation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Receipt>(r =>
        {
            r.ToTable("receipt");
            r.HasKey(x => x.Id);

            r.HasOne(x => x.StoreLocation)
                .WithMany(x => x.Receipts)
                .HasForeignKey(x => x.StoreLocationId);

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

        modelBuilder.Entity<StoreChain>(s =>
        {
            s.ToTable("store_chain");
            s.HasKey(x => x.Id);

            s.HasMany(x => x.Locations)
                .WithOne(x => x.StoreChain)
                .HasForeignKey(x => x.StoreChainId);
        });

        modelBuilder.Entity<StoreLocation>(s =>
        {
            s.ToTable("store_location");
            s.HasKey(x => x.Id);

            s.HasOne(x => x.StoreChain)
                .WithMany(x => x.Locations)
                .HasForeignKey(x => x.StoreChainId);

            s.HasMany(x => x.Receipts)
                .WithOne(x => x.StoreLocation)
                .HasForeignKey(x => x.StoreLocationId);
        });
    }
}