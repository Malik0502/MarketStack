using MarketStack.Common.ResponseBase;
using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Logic.Contracts.Service;

namespace MarketStack.Logic.Service;

public class PriceAnalysisService : IPriceAnalysisService
{
    private readonly IReceiptPriceSummaryRepository _repository;

    public PriceAnalysisService(IReceiptPriceSummaryRepository repository)
    {
        _repository = repository;
    }

    public async Task<DataResponse<decimal>> GetTotalExpensesAsync()
    {
        ICollection<ReceiptPriceSummary> priceSummary= await _repository.GetReceiptPriceSummariesAsync();

        decimal totalExpense = 0m;

        foreach (var data in priceSummary)
        {
            totalExpense += data.TaxBaseAmount;
        }

        return DataResponse<decimal>.CreateSuccessResponse(totalExpense, "Succesful", "Succesfully calculated the total expenses of all purchases");
    }

    public async Task<DataResponse<decimal>> GetTotalTaxExpensesAsync()
    {
        ICollection<ReceiptPriceSummary> priceSummary = await _repository.GetReceiptPriceSummariesAsync();

        decimal totalTaxExpenses = 0m;

        foreach (var data in priceSummary)
        {
            totalTaxExpenses += data.TaxAmount;
        }

        return DataResponse<decimal>.CreateSuccessResponse(totalTaxExpenses, "Succesful", "Succesfully calculated the total tax expenses");
    }
}