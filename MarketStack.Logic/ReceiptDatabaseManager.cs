using MarketStack.Data.Contracts.Repositories;
using MarketStack.Logic.Contracts;

namespace MarketStack.Logic;

public class ReceiptDatabaseManager : IReceiptDatabaseManager
{
    private readonly IReceiptInformationManager _receiptInformationManager;
    private readonly IReceiptRepository _receiptRepository;

    public ReceiptDatabaseManager(IReceiptInformationManager receiptInformationManager, IReceiptRepository receiptRepository)
    {
        _receiptInformationManager = receiptInformationManager;
        _receiptRepository = receiptRepository;
    }

    public async Task Insert(string ticketId, string languageCode = "de-DE")
    {
        var receipt = await _receiptInformationManager.GetReceiptAsync(ticketId, languageCode);
        //_receiptRepository.AddReceiptAsync(receipt);
    }

    public async Task InsertReceipts()
    {
        throw new NotImplementedException();
    }
}