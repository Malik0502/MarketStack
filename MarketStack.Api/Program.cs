using Hangfire;
using MarketStack.Api.Configuration;

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

            CreateOrUpdateHangfireJobs(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseHangfireDashboard();
            app.MapControllers();

            app.Run();
        }

        public static void CreateOrUpdateHangfireJobs(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var hangfireJobFactory = scope.ServiceProvider.GetRequiredService<HangFireJobFactory>();

            hangfireJobFactory.CreateRecurringJobs();
        }

        private static void CreateBuilder(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
                return;

            builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));

            builder.CreateDatabase(connectionString)
                .CreateHangfire(connectionString)
                .CreateHangfireJobs()
                .CreateRepositories()
                .CreateManager()
                .CreateClients();
        }
    }
}
