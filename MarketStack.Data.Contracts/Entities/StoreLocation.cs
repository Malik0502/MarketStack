using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketStack.Data.Contracts.Entities;

public class StoreLocation
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("store_chain_id")]
    public int StoreChainId { get; set; }

    // optional
    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("street")]
    [Required]
    public string Street { get; set; }

    [Column("city")]
    [Required]
    public string City { get; set; }

    [Column("postal_code")]
    [Required]
    public string PostalCode { get; set; }

    public StoreChain StoreChain { get; set; }

    public List<Receipt> Receipts { get; set; }
}