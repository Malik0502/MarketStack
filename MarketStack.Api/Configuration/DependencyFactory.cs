using Hangfire;
using Hangfire.PostgreSql;
using MarketStack.Api.Jobs.Implementation;
using MarketStack.Api.Jobs.Interface;
using MarketStack.Data;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Data.Repositories;
using MarketStack.Library.Contracts.Helper;
using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Helper.Api;
using MarketStack.Library.Receipt.Lidl;
using MarketStack.Logic.Contracts.Service.Database;
using MarketStack.Logic.Contracts.Service;
using MarketStack.Logic.Service;
using MarketStack.Logic.Service.Database;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Api.Configuration;

public static class DependencyFactory
{
    public static WebApplicationBuilder CreateDatabase(this WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddDbContext<MarketStackContext>(options =>
            options.UseNpgsql(connectionString));

        return builder;
    }

    public static WebApplicationBuilder CreateHangfire(this WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddHangfire(x =>
        {
            x.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
        });

        builder.Services.AddHangfireServer();

        return builder;
    }

    public static WebApplicationBuilder CreateHangfireJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<HangFireJobFactory>();
        builder.Services.AddScoped<ILidlJobService, LidlJobService>();

        return builder;
    }

    public static WebApplicationBuilder CreateRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
        builder.Services.AddScoped<IReceiptItemRepository, ReceiptItemRepository>();
        builder.Services.AddScoped<IReceiptPriceSummaryRepository, ReceiptPriceSummaryRepository>();

        return builder;
    }

    public static WebApplicationBuilder CreateServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReceiptLibraryService, ReceiptLibraryService>();
        builder.Services.AddScoped<IReceiptImportService, ReceiptImportService>();
        builder.Services.AddScoped<IPriceAnalysisService, PriceAnalysisService>();

        return builder;
    }

    public static WebApplicationBuilder CreateClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IReceiptClient, LidlReceiptClient>();
        builder.Services.AddScoped<IFetchClient, FetchClient>();

        return builder;
    }
}