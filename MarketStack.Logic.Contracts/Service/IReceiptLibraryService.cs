using MarketStack.Common.ResponseBase;
using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Logic.Contracts.Service;

public interface IReceiptLibraryService
{
    public Task<DataResponse<string>> GetAuthTokenAsync();

    public Task<DataResponse<ReceiptDto>> GetReceiptAsync(string ticketId, string languageCode);

    public Task<DataResponse<ReceiptPageInfoDto>> GetReceiptsInfoAsync();
}