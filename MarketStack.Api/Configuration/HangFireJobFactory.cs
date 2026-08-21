using Hangfire;
using MarketStack.Api.Jobs.Interface;

namespace MarketStack.Api.Configuration;

public class HangFireJobFactory
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ILidlJobService _lidlJobService;

    public HangFireJobFactory(IRecurringJobManager recurringJobManager, ILidlJobService lidlJobService)
    {
        _recurringJobManager = recurringJobManager;
        _lidlJobService = lidlJobService;
    }

    public void CreateRecurringJobs()
    {
        _recurringJobManager.AddOrUpdate("LidlReceiptImport", () => _lidlJobService.ProcessLidlReceiptAsync(), Cron.Daily());
    }
}