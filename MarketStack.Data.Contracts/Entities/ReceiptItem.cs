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

    [Column("net_price")]
    [Required]
    public decimal NetPrice { get; set; }

    [Column("gross_price")]
    [Required]
    public decimal GrossPrice { get; set; }

    [Column("vat_rate")]
    [Required]
    public decimal VatRate { get; set; }

    public Receipt Receipt { get; set; }

    public Product Product { get; set; }

}