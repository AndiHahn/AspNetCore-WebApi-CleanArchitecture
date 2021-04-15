using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Interfaces.Data.Repositories;
using CleanArchitecture.Core.Interfaces.Services;
using CleanArchitecture.Core.Interfaces.SqlQueries;
using CleanArchitecture.Infrastructure.Database.Budget;
using CleanArchitecture.Infrastructure.Database.Identity;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Services.AzureStorage;
using CleanArchitecture.Infrastructure.Services.Email;
using CleanArchitecture.Infrastructure.SqlQueries;
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

            services.AddScoped<IBillQueries, BillQueries>();

            services.RegisterDatabase(configuration);
        }

        private static void RegisterDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IBudgetContext, BudgetContext>(
                options => options
                    .UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection")));

            services.AddDbContext<IdentityContext>(
                options => options
                    .UseSqlServer(configuration.GetConnectionString("IdentityDbConnection")));
        }
    }
}