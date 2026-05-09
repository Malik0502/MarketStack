namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptInfoDto
{
    public string Id { get; set; }
    public string Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int ArticlesCount { get; set; }
    public string Store { get; set; }
    public bool IsHtml { get; set; }
    public string Vendor { get; set; } 
    public ReceiptBadge Badges { get; set; }
}