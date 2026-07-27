using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Logic.Contracts;

public interface IReceiptInformationManager
{
    public Task<string?> GetAuthTokenAsync();

    public Task<ReceiptDto?> GetReceiptAsync(string ticketId, string languageCode);

    public Task<ReceiptPageInfoDto?> GetReceiptsInfoAsync();
}