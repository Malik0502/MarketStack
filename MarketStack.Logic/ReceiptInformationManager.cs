using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts;

namespace MarketStack.Logic;

public class ReceiptInformationManager : IReceiptInformationManager
{
    private readonly IReceiptClient _receiptClient;

    public ReceiptInformationManager(IReceiptClient receiptClient)
    {
        _receiptClient = receiptClient;
    }

    public async Task<string?> GetAuthTokenAsync()
    {
        return await _receiptClient.GetAuthTokenAsync();
    }

    public async Task<ReceiptDto?> GetReceiptAsync(string ticketId, string languageCode)
    {
        return await _receiptClient.GetReceiptAsync(ticketId, languageCode);
    }

    public async Task<ReceiptPageInfoDto?> GetReceiptsInfoAsync()
    {
        return await _receiptClient.GetReceiptsInfoAsync();
    }
}