using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class ReceiptPriceSummary
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("receipt_id")]
    public int ReceiptId { get; set; }

    [Column("vat_rate")]
    [Required]
    public required int TaxType { get; set; }

    [Column("tax_inclusive_price")]
    [Required]
    public required decimal TaxBaseAmount { get; set; }

    [Column("tax_amount")]
    [Required]
    public required decimal TaxAmount { get; set; }

    public Receipt Receipt { get; set; } = null!;
}