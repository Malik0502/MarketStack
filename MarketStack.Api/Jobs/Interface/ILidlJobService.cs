namespace MarketStack.Api.Jobs.Interface;

public interface ILidlJobService
{
    public Task ProcessLidlReceiptAsync();
}