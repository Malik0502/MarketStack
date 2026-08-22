using MarketStack.Common.ResponseBase;
using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts.Service;

namespace MarketStack.Logic.Service;

public class ReceiptLibraryService : IReceiptLibraryService
{
    private readonly IReceiptClient _receiptClient;

    public ReceiptLibraryService(IReceiptClient receiptClient)
    {
        _receiptClient = receiptClient;
    }

    public async Task<DataResponse<string>> GetAuthTokenAsync()
    {
        return await _receiptClient.GetAuthTokenAsync();
    }

    public async Task<DataResponse<ReceiptDto>> GetReceiptAsync(string ticketId, string languageCode)
    {
        return await _receiptClient.GetReceiptAsync(ticketId, languageCode);
    }

    public async Task<DataResponse<ReceiptPageInfoDto>> GetReceiptsInfoAsync()
    {
        return await _receiptClient.GetReceiptsInfoAsync();
    }
}