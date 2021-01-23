using System;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();

                try
                {
                    logger.LogInformation("Start database migration...");
                    var context = services.GetRequiredService<IBudgetContext>();
                    await context.MigrateAsync();
                    logger.LogInformation("Finished database migration.");

                    logger.LogInformation("Start seed database...");
                    await BudgetContextSeed.SeedAsync(context);
                    logger.LogInformation("Finished seeding database.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
