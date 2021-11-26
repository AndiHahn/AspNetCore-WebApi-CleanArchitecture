using System;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Database.Budget;
using CleanArchitecture.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                    logger.LogInformation("Start database migration...");

                    var budgetContext = services.GetRequiredService<BudgetContext>();
                    await budgetContext.Database.MigrateAsync();
                    var identityContext = services.GetRequiredService<IdentityContext>();
                    await identityContext.Database.MigrateAsync();

                    logger.LogInformation("Finished database migration.");

                    logger.LogInformation("Start seed database...");
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await DatabaseSeed.SeedAsync(budgetContext, userManager);
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
