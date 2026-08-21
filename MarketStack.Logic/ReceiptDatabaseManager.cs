using MarketStack.Data.Contracts.Repositories;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts;
using MarketStack.Logic.Mapping;

namespace MarketStack.Logic;

public class ReceiptDatabaseManager : IReceiptDatabaseManager
{
    private readonly IReceiptRepository _receiptRepository;
    public ReceiptDatabaseManager(IReceiptRepository receiptRepository)
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