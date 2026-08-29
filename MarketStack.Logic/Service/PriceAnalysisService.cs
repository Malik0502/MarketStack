using MarketStack.Common.ResponseBase;
using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Logic.Contracts.Dto;
using MarketStack.Logic.Contracts.Service;

namespace MarketStack.Logic.Service;

public class PriceAnalysisService : IPriceAnalysisService
{
    private readonly IReceiptPriceSummaryRepository _priceSummaryRepository;
    private readonly IReceiptRepository _receiptRepository;

    public PriceAnalysisService(IReceiptPriceSummaryRepository priceSummaryRepository, IReceiptRepository receiptRepository)
    {
        _priceSummaryRepository = priceSummaryRepository;
        _receiptRepository = receiptRepository;
    }

    public async Task<DataResponse<decimal>> GetTotalExpensesAsync()
    {
        ICollection<ReceiptPriceSummary> priceSummaries = await _priceSummaryRepository.GetReceiptPriceSummariesAsync();

        decimal totalExpense = 0m;

        foreach (var data in priceSummaries)
        {
            totalExpense += data.TaxBaseAmount;
        }

        return DataResponse<decimal>.CreateSuccessResponse(totalExpense, "Succesful", "Succesfully calculated the total expenses of all purchases");
    }

    public async Task<DataResponse<decimal>> GetTotalTaxExpensesAsync()
    {
        ICollection<ReceiptPriceSummary> priceSummaries = await _priceSummaryRepository.GetReceiptPriceSummariesAsync();

        decimal totalTaxExpenses = 0m;

        foreach (var data in priceSummaries)
        {
            totalTaxExpenses += data.TaxAmount;
        }

        return DataResponse<decimal>.CreateSuccessResponse(totalTaxExpenses, "Succesful", "Succesfully calculated the total tax expenses");
    }

    public async Task<DataResponse<IDictionary<string, MonthlyExpenseSummary>>> GetExpenseHistory()
    {
        Dictionary<string, MonthlyExpenseSummary> result = [];

        ICollection<ReceiptPriceSummary> priceSummaries = await _priceSummaryRepository.GetReceiptPriceSummariesAsync();

        ICollection<Receipt> receipts = await _receiptRepository.GetReceiptsAsync();

        foreach (var summary in priceSummaries.OrderBy(x => x.ReceiptId))
        {
            Receipt connectedReceipt = receipts.First(x => x.Id == summary.ReceiptId);

            if (result.Count == 0 )
            {
                AddToDictionary(result, summary.TaxBaseAmount, connectedReceipt);
                continue;
            }

            bool isExisting = result
                .TryGetValue($"{connectedReceipt.PurchasedAt.Month}-{connectedReceipt.PurchasedAt.Year}", out MonthlyExpenseSummary? expenseSummary);

            if (!isExisting)
            {
                AddToDictionary(result, summary.TaxBaseAmount, connectedReceipt);
                continue;
            }

            expenseSummary?.Expense += summary.TaxBaseAmount;
        }

        result = result
            .OrderByDescending(x => x.Value.PurchasedAt)
            .ToDictionary(x => x.Key, x => x.Value);

        return DataResponse<IDictionary<string, MonthlyExpenseSummary>>.CreateSuccessResponse(result,
            "Success",
            "Successfully exported monthly expense history");
    }

    public async Task<DataResponse<IDictionary<string, MonthlyExpenseSummary>>> GetTaxExpenseHistory()
    {
        Dictionary<string, MonthlyExpenseSummary> result = [];

        ICollection<ReceiptPriceSummary> priceSummaries = await _priceSummaryRepository.GetReceiptPriceSummariesAsync();

        ICollection<Receipt> receipts = await _receiptRepository.GetReceiptsAsync();

        foreach (var summary in priceSummaries.OrderBy(x => x.ReceiptId))
        {
            Receipt connectedReceipt = receipts.First(x => x.Id == summary.ReceiptId);

            if (result.Count == 0)
            {
                AddToDictionary(result, summary.TaxAmount, connectedReceipt);
                continue;
            }

            bool isExisting = result
                .TryGetValue($"{connectedReceipt.PurchasedAt.Month}-{connectedReceipt.PurchasedAt.Year}", out MonthlyExpenseSummary? expenseSummary);

            if (!isExisting)
            {
                AddToDictionary(result, summary.TaxAmount, connectedReceipt);
                continue;
            }

            expenseSummary?.Expense += summary.TaxAmount;
        }

        result = result
            .OrderByDescending(x => x.Value.PurchasedAt)
            .ToDictionary(x => x.Key, x => x.Value);

        return DataResponse<IDictionary<string, MonthlyExpenseSummary>>.CreateSuccessResponse(result,
            "Success",
            "Successfully exported monthly tax expense history");
    }

    private static void AddToDictionary(Dictionary<string, MonthlyExpenseSummary> dictionary, decimal amount, Receipt connectedReceipt)
    {
        // set day of datetime to 1
        var cleanDateTime = new DateTime(connectedReceipt.PurchasedAt.Year, connectedReceipt.PurchasedAt.Month, day: 1); 

        var expense = new MonthlyExpenseSummary()
        {
            Expense = amount,
            PurchasedAt = DateOnly.FromDateTime(cleanDateTime)
        };

        dictionary.Add($"{expense.PurchasedAt.Month}-{expense.PurchasedAt.Year}", expense);
    }
}