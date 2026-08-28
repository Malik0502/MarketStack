using Hangfire;
using MarketStack.Api.Configuration;
using MarketStack.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowedOrigins",
                    policy =>
                    {
                        policy
                            .WithOrigins("http://localhost:5173")
                            .WithMethods("GET", "POST", "PUT")
                            .AllowAnyHeader();
                    });
            });

            builder.Services.AddControllers();

            builder.Services.AddOpenApiDocument(config =>
            {
                config.Title = "Market Stack Api";
            });

            CreateBuilder(builder);
            
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseCors("AllowedOrigins");

            CreateOrUpdateHangfireJobs(app);
            CreateOrUpdateDatabase(app);

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

        public static void CreateOrUpdateDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<MarketStackContext>();
            context.Database.Migrate();
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
                .CreateServices()
                .CreateClients();
        }
    }
}
