using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Database.Budget;
using CleanArchitecture.Infrastructure.Database.Identity;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Services.AzureStorage;
using CleanArchitecture.Infrastructure.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure
{
    public static class InfrastuctureRegistrationExtensions
    {
        public static void RegisterInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAzureStorageClientFactory, AzureStorageClientFactory>();
            services.AddSingleton<IBlobStorageRepository, BlobStorageRepository>();
            services.AddScoped<IEmailService, SendGridEmailService>();

            services.RegisterDatabase(configuration);

            services.RegisterRepositories();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddSingleton<IBillImageRepository, BillImageRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
        }

        private static void RegisterDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BudgetContext>(
                options => options
                    .UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection")));

            services.AddDbContext<IdentityContext>(
                options => options
                    .UseSqlServer(configuration.GetConnectionString("IdentityDbConnection")));
        }
    }
}