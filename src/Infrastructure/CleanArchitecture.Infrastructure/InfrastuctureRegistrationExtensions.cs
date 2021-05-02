using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Interfaces.Services;
using CleanArchitecture.Infrastructure.Database.Budget;
using CleanArchitecture.Infrastructure.Database.Identity;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Repositories.Sql;
using CleanArchitecture.Infrastructure.Repositories.TableStorage;
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
            services.AddScoped<IAzureStorageClientFactory, AzureStorageClientFactory>();
            services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();
            services.AddScoped<IEmailService, SendGridEmailService>();

            services.RegisterDatabase(configuration);

            services.RegisterRepositories();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
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