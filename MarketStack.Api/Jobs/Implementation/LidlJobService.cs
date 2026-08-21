using MarketStack.Api.Configuration;
using MarketStack.Api.Jobs.Interface;
using MarketStack.Common.ErrorHandling;
using MarketStack.Common.ResponseBase;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts;
using Microsoft.Extensions.Options;

namespace MarketStack.Api.Jobs.Implementation;

public class LidlJobService : ILidlJobService
{
    private readonly IReceiptInformationManager _receiptInformationManager;
    private readonly IReceiptDatabaseManager _receiptDatabaseManager;
    private readonly ApplicationOptions _options;

    public LidlJobService(IReceiptInformationManager receiptInformationManager, IReceiptDatabaseManager receiptDatabaseManager, IOptions<ApplicationOptions> options)
    {
        _receiptInformationManager = receiptInformationManager;
        _receiptDatabaseManager = receiptDatabaseManager;
        _options = options.Value;
    }

    public async Task ProcessLidlReceiptAsync()
    {
        DataResponse<ReceiptPageInfoDto> receipts = await _receiptInformationManager.GetReceiptsInfoAsync();

        if (!receipts.Success)
        {
            return;
        }

        string languageCode = _options.LanguageCode;
        List<ReceiptDto> receiptDtos = new List<ReceiptDto>();

        foreach (var receipt in receipts.Data!.Items)
        {
            var result = await _receiptInformationManager.GetReceiptAsync(receipt.Id, languageCode);
            if (result.ErrorCode != ErrorCodes.None)
                continue;

            receiptDtos.Add(result.Data!);
        }

        await _receiptDatabaseManager.InsertReceipts(receiptDtos);
    }
}