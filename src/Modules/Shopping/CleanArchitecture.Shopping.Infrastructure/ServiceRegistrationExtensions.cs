using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.AzureStorage;
using CleanArchitecture.Shopping.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Shopping.Infrastructure
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IAzureStorageClientFactory, AzureStorageClientFactory>();
            services.AddSingleton<IBlobStorageRepository, BlobStorageRepository>();

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
    }
}