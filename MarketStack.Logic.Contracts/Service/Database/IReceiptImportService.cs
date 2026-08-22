using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Logic.Contracts.Service.Database;

public interface IReceiptImportService
{
    public Task Insert(ReceiptDto receiptDto);

    public Task InsertReceipts(List<ReceiptDto> receiptDtos);
}