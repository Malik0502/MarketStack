using MarketStack.Common.ApiBase;
using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Logic.Contracts;

public interface IReceiptInformationManager
{
    public Task<DataResponse<string>> GetAuthTokenAsync();

    public Task<DataResponse<ReceiptDto>> GetReceiptAsync(string ticketId, string languageCode);

    public Task<DataResponse<ReceiptPageInfoDto>> GetReceiptsInfoAsync();
}