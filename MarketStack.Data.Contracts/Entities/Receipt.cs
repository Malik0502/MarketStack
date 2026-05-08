using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class Receipt
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("store_location_id")]
    [Required]
    public int StoreLocationId { get; set; }

    [Column("purchasedAt")]
    [Required]
    public DateTime PurchasedAt { get; set; }

    public StoreLocation StoreLocation { get; set; }

    public List<ReceiptItem> Items { get; set; }
}