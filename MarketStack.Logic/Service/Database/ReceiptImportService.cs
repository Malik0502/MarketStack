using MarketStack.Data.Contracts.Repositories;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts.Service.Database;
using MarketStack.Logic.Mapping;

namespace MarketStack.Logic.Service.Database;

public class ReceiptImportService : IReceiptImportService
{
    private readonly IReceiptRepository _receiptRepository;
    public ReceiptImportService(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public async Task Insert(ReceiptDto receiptDto)
    {
        await _receiptRepository.AddReceiptAsync(receiptDto.ToReceipt());
    }

    public async Task InsertReceipts(List<ReceiptDto> receiptDtos)
    {
        await _receiptRepository.AddReceiptRangeAsync(receiptDtos.ToReceipts());
    }
}