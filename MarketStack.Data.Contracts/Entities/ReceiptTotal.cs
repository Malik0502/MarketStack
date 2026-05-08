using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class ReceiptTotal
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("net_total")]
    [Required]
    public decimal NetTotal { get; set; }

    [Column("vat_total")]
    [Required]
    public decimal VatTotal { get; set; }

    [Column("vat_type_A_total")]
    [Required]
    public decimal VatTypeATotal { get; set; }

    [Column("vat_type_B_total")]
    [Required]
    public decimal VatTypeBTotal { get; set; }

    [Column("gros_total")]
    [Required]
    public decimal GrossTotal { get; set; }
}