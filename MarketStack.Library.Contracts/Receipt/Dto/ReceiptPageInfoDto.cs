namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptPageInfoDto
{
    public int Size { get; set; }
    public int TotalCount { get; set; }
    
    public List<ReceiptInfoDto> Items { get; set; }
}