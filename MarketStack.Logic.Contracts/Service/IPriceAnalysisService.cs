using MarketStack.Common.ResponseBase;

namespace MarketStack.Logic.Contracts.Service;

public interface IPriceAnalysisService
{
    public Task<DataResponse<decimal>> GetTotalExpensesAsync();

    public Task<DataResponse<decimal>> GetTotalTaxExpensesAsync();
}