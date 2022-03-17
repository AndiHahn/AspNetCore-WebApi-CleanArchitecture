using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.AzureStorage;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;
using CleanArchitecture.Shopping.Infrastructure.Database.Identity;
using CleanArchitecture.Shopping.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Shopping.Infrastructure
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAzureStorageClientFactory, AzureStorageClientFactory>();
            services.AddSingleton<IBlobStorageRepository, BlobStorageRepository>();
            services.AddSingleton<IBillImageRepository, BillImageRepository>();

            services.AddDbContext<IShoppingDbContext, ShoppingDbContext>(
                options => options
                    .UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection")));

            services.AddDbContext<IdentityContext>(
                options => options
                    .UseSqlServer(configuration.GetConnectionString("IdentityDbConnection")));
        }
    }
}