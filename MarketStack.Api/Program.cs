using MarketStack.Data;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Data.Repositories;
using MarketStack.Library.Contracts.Helper;
using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Helper.Api;
using MarketStack.Library.Receipt.Lidl;
using MarketStack.Logic;
using MarketStack.Logic.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddOpenApiDocument(config =>
            {
                config.Title = "Market Stack Api";
            });

            CreateBuilder(builder);
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void CreateBuilder(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MarketStackContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IReceiptClient, LidlReceiptClient>();
            builder.Services.AddScoped<IReceiptInformationManager, ReceiptInformationManager>();
            builder.Services.AddScoped<IReceiptDatabaseManager, ReceiptDatabaseManager>();
            builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
            builder.Services.AddScoped<IFetchClient, FetchClient>();
        }
    }
}
