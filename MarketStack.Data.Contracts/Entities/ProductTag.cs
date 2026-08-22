using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class ProductTag
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Column("tag_id")]
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}