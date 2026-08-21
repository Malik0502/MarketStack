using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Logic.Contracts;

public interface IReceiptDatabaseManager
{
    public Task Insert(ReceiptDto receiptDto);

    public Task InsertReceipts(List<ReceiptDto> receiptDtos);
}