namespace MarketStack.Library.Contracts.Receipt.Dto;


public class ReceiptBadge
{
    public int Coupons { get; set; }
    public bool Invoice { get; set; }
    public int Returns { get; set; }
    public bool IsAvailable { get; set; }
}