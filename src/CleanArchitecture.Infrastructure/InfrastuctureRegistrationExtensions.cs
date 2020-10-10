using CleanArchitecture.Core.Interfaces.BlobStorage;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure
{
    public static class InfrastuctureRegistrationExtensions
    {
        public static void RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBlobClientFactory, BlobClientFactory>();
        }
    }
}