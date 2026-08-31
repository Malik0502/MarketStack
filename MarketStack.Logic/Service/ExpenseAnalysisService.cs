using MarketStack.Common.ResponseBase;
using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Logic.Contracts.Dto;
using MarketStack.Logic.Contracts.Service;

namespace MarketStack.Logic.Service;

public class ExpenseAnalysisService : IExpenseAnalysisService
{
    private readonly IReceiptPriceSummaryRepository _priceSummaryRepository;
    private readonly IReceiptRepository _receiptRepository;

    public ExpenseAnalysisService(IReceiptPriceSummaryRepository priceSummaryRepository, IReceiptRepository receiptRepository)
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

        return DataResponse<decimal>.CreateSuccessResponse(totalExpense, "Success", "Succesfully calculated the total expenses of all purchases");
    }

    public async Task<DataResponse<decimal>> GetTotalTaxExpensesAsync()
    {
        ICollection<ReceiptPriceSummary> priceSummaries = await _priceSummaryRepository.GetReceiptPriceSummariesAsync();

        decimal totalTaxExpenses = 0m;

        foreach (var data in priceSummaries)
        {
            totalTaxExpenses += data.TaxAmount;
        }

        return DataResponse<decimal>.CreateSuccessResponse(totalTaxExpenses, "Success", "Succesfully calculated the total tax expenses");
    }

    public async Task<DataResponse<decimal>> GetPercentageChangeSinceLastWeekAsync()
    {
        decimal lastWeekExpense = await CalculateLastWeeksTotalExpense(isTaxExpense: false);

        DataResponse<decimal> totalExpense = await GetTotalExpensesAsync();

        decimal lastWeekTotalExpense = totalExpense.Data - lastWeekExpense;

        decimal result = Math.Round((totalExpense.Data - lastWeekTotalExpense) / lastWeekTotalExpense * 100, 2);

        return DataResponse<decimal>.CreateSuccessResponse(result, "Success", "Successfully calculated total percentage change since last week");
    }

    public async Task<DataResponse<decimal>> GetTaxPercentageChangeSinceLastWeekAsync()
    {
        decimal lastWeekExpense = await CalculateLastWeeksTotalExpense(isTaxExpense: true);

        DataResponse<decimal> totalExpense = await GetTotalTaxExpensesAsync();

        decimal lastWeekTotalExpense = totalExpense.Data - lastWeekExpense;

        decimal result = Math.Round((totalExpense.Data - lastWeekTotalExpense) / lastWeekTotalExpense * 100, 2);

        return DataResponse<decimal>.CreateSuccessResponse(result, "Success", "Successfully calculated percentage change in taxes since last week");
    }

    public async Task<DataResponse<IDictionary<string, MonthlyExpenseSummary>>> GetExpenseHistory()
    {
        Dictionary<string, MonthlyExpenseSummary> result = [];

        ICollection<ReceiptPriceSummary> priceSummaries = await _priceSummaryRepository.GetReceiptPriceSummariesAsync();

        ICollection<Receipt> receipts = await _receiptRepository.GetReceiptsAsync();

        foreach (var summary in priceSummaries.OrderBy(x => x.ReceiptId))
        {
            Receipt connectedReceipt = receipts.First(x => x.Id == summary.ReceiptId);
            CalculateExpenseHistory(connectedReceipt, summary.TaxBaseAmount, result);
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
            CalculateExpenseHistory(connectedReceipt, summary.TaxAmount, result);
        }

        result = result
            .OrderByDescending(x => x.Value.PurchasedAt)
            .ToDictionary(x => x.Key, x => x.Value);

        return DataResponse<IDictionary<string, MonthlyExpenseSummary>>.CreateSuccessResponse(result,
            "Success",
            "Successfully exported monthly tax expense history");
    }

    private async Task<decimal> CalculateLastWeeksTotalExpense(bool isTaxExpense)
    {
        ICollection<Receipt> receipts = await _receiptRepository.GetReceiptsAsync();

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        // today - one week
        DateOnly start = today.AddDays(-7);

        decimal result = 0;
        foreach (var receipt in receipts)
        {
            DateOnly purchasedDate = DateOnly.FromDateTime(receipt.PurchasedAt);

            bool isInLast7Days = purchasedDate >= start && purchasedDate <= today;

            if (!isInLast7Days)
                continue;

            ICollection<ReceiptPriceSummary> priceSummaries =
                await _priceSummaryRepository.GetReceiptPriceSummaryFromReceiptAsync(receipt.Id);

            if (priceSummaries.Count == 0)
                continue;


            foreach (var summary in priceSummaries)
            {
                if (isTaxExpense)
                {
                    result += summary.TaxAmount;
                    continue;
                }

                result += summary.TaxBaseAmount;
            }
        }

        return result;
    }

    private static void CalculateExpenseHistory(Receipt receipt, decimal amount, Dictionary<string, MonthlyExpenseSummary> result)
    {
        if (result.Count == 0)
        {
            AddToDictionary(result, amount, receipt);
            return;
        }

        bool isExisting = result
            .TryGetValue($"{receipt.PurchasedAt.Month}-{receipt.PurchasedAt.Year}", out MonthlyExpenseSummary? expenseSummary);

        if (!isExisting)
        {
            AddToDictionary(result, amount, receipt);
            return;
        }

        expenseSummary?.Expense += amount;
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