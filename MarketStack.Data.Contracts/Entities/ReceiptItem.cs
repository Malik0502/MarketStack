using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class ReceiptItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("receipt_id")]
    [Required]
    public int ReceiptId { get; set; }

    [Column("product_id")]
    [Required]
    public int ProductId { get; set; }

    [Column("quantity")]
    [Required]
    public decimal Quantity { get; set; }

    [Column("price")]
    [Required]
    public decimal Price { get; set; }

    [Column("vat_rate")]
    [Required]
    public int TaxType { get; set; }

    [Column("store_intern_item_id")]
    [StringLength(255)]
    public string? StoreInternItemId { get; set; } = null;

    [Column("promotion_id")]
    [StringLength(255)]
    public string? PromotionId { get; set; } = null;

    public Receipt? Receipt { get; set; }

    public Product? Product { get; set; }

}