using MarketStack.Api.Configuration;
using MarketStack.Api.Jobs.Interface;
using MarketStack.Common.ErrorHandling;
using MarketStack.Common.ResponseBase;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts.Service.Database;
using MarketStack.Logic.Contracts.Service;
using Microsoft.Extensions.Options;

namespace MarketStack.Api.Jobs.Implementation;

public class LidlJobService : ILidlJobService
{
    private readonly IReceiptLibraryService _receiptLibraryManager;
    private readonly IReceiptImportService _receiptImportService;
    private readonly ApplicationOptions _options;

    public LidlJobService(IReceiptLibraryService receiptLibraryManager, IReceiptImportService receiptImportService, IOptions<ApplicationOptions> options)
    {
        _receiptLibraryManager = receiptLibraryManager;
        _receiptImportService = receiptImportService;
        _options = options.Value;
    }

    public async Task ProcessLidlReceiptAsync()
    {
        DataResponse<ReceiptPageInfoDto> receipts = await _receiptLibraryManager.GetReceiptsInfoAsync();

        if (!receipts.Success)
        {
            return;
        }

        string languageCode = _options.LanguageCode;
        List<ReceiptDto> receiptDtos = new List<ReceiptDto>();

        foreach (var receipt in receipts.Data!.Items)
        {
            var result = await _receiptLibraryManager.GetReceiptAsync(receipt.Id, languageCode);
            if (result.ErrorCode != ErrorCodes.None)
                continue;

            receiptDtos.Add(result.Data!);
        }

        await _receiptImportService.InsertReceipts(receiptDtos);
    }
}