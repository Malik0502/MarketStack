using MarketStack.Common.ResponseBase;
using MarketStack.Logic.Contracts.Dto;

namespace MarketStack.Logic.Contracts.Service;

public interface IPriceAnalysisService
{
    public Task<DataResponse<decimal>> GetTotalExpensesAsync();

    public Task<DataResponse<decimal>> GetTotalTaxExpensesAsync();

    public Task<DataResponse<decimal>> GetPercentageChangeSinceLastWeekAsync();

    public Task<DataResponse<decimal>> GetTaxPercentageChangeSinceLastWeekAsync();

    public Task<DataResponse<IDictionary<string, MonthlyExpenseSummary>>> GetExpenseHistory();

    public Task<DataResponse<IDictionary<string, MonthlyExpenseSummary>>> GetTaxExpenseHistory();
}