using System;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CleanArchitecture.BudgetPlan.Api;
using CleanArchitecture.Shared.Infrastructure.Database;
using CleanArchitecture.Shopping.Api;
using CleanArchitecture.Shopping.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();

                try
                {
                    logger.LogInformation("Start shopping db context migration and seed...");

                    await ShoppingModule.MigrateAndSeedShoppingDbContextAsync(services);
                    await BudgetPlanModule.MigrateAndSeedBudgetPlanDbContextAsync(services, new Guid(DatabaseSeed.TestUser1.Id));

                    logger.LogInformation("Finished seeding shopping db context.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((webHost, configBuilder) =>
                    {
                        var config = configBuilder.Build();

                        bool useKeyVault = config.GetSection("KeyVault").GetValue<bool>("Enabled");
                        if (useKeyVault)
                        {
                            string keyVaultUri = config.GetSection("KeyVault").GetValue<string>("Uri");

                            var secretClient = new SecretClient(
                                new Uri(keyVaultUri),
                                new DefaultAzureCredential());

                            configBuilder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                        }
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
