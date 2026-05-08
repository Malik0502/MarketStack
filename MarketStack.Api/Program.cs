using MarketStack.Data;

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
            builder.Services.AddDbContext<MarketStackContext>();
        }
    }
}
