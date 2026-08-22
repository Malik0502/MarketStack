using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class Receipt
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    [StringLength(255)]
    [Required]
    public required string ReceiptTicketId { get; set; }

    [Column("store")]
    [Required]
    [StringLength(255)]
    public required string Store { get; set; }

    [Column("chain")]
    [Required]
    [StringLength(255)]
    public required string Chain { get; set; }

    [Column("purchasedAt")]
    [Required]
    public DateTime PurchasedAt { get; set; }

    public List<ReceiptItem> Items { get; set; } = [];
}