using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class Product
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    public ICollection<ReceiptItem> ReceiptItems { get; set; } = [];

    public ICollection<ProductTag> ProductTags { get; set; } = [];
}