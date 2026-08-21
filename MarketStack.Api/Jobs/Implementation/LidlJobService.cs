using MarketStack.Api.Configuration;
using MarketStack.Api.Jobs.Interface;
using MarketStack.Common.ApiBase;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Logic.Contracts;
using MarketStack.Logic.Mapping;
using Microsoft.Extensions.Options;

namespace MarketStack.Api.Jobs.Implementation;

public class LidlJobService : ILidlJobService
{
    private readonly IReceiptInformationManager _receiptInformationManager;
    private readonly IReceiptRepository _repository;
    private readonly ApplicationOptions _options;

    public LidlJobService(IReceiptInformationManager receiptInformationManager, IReceiptRepository repository, IOptions<ApplicationOptions> options)
    {
        _receiptInformationManager = receiptInformationManager;
        _repository = repository;
        _options = options.Value;
    }

    public async Task ProcessLidlReceiptAsync()
    {
        var receipts = await _receiptInformationManager.GetReceiptsInfoAsync();

        if (!receipts.Success)
        {
            return;
        }

        var languageCode = _options.LanguageCode;

        foreach (var receipt in receipts.Data!.Items)
        {
            var result = await _receiptInformationManager.GetReceiptAsync(receipt.Id, languageCode);
            if (result.ErrorCode != ErrorCodes.None)
                continue;

            await _repository.AddReceiptAsync(result.Data!.ToReceipt());
        }

    }
}