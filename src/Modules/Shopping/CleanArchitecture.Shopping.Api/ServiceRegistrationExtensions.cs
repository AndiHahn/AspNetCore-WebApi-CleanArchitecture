using CleanArchitecture.Shared.Infrastructure;
using CleanArchitecture.Shopping.Application;
using CleanArchitecture.Shopping.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.Shopping.Api
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddShoppingModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure();
            services.AddSharedInfrastructure(configuration);
        }

        public static IMvcBuilder AddShoppingModule(this IMvcBuilder builder)
        {
            return builder.AddApplicationPart(Assembly.GetExecutingAssembly());
        }
    }
}
