using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Library.Contracts.Receipt
{
    public interface IReceiptClient
    {
        public Task<string?> GetAuthTokenAsync();
        
        public Task<ReceiptDto?> GetReceiptAsync(string ticketId, string languageCode);
        
        public Task<ReceiptPageInfoDto?> GetReceiptsInfoAsync();
    }
}
