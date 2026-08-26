namespace MarketStack.Logic.Contracts.Dto;

public class MonthlyExpenseSummary
{
    public DateOnly PurchasedAt { get; set; }

    public decimal Expense { get; set; }
}