using CleanArchitecture.Core.Interfaces.Queries;
using CleanArchitecture.Core.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Core
{
    public static class CoreRegistrationExtensions
    {
        public static void RegisterCore(this IServiceCollection services)
        {
            services.AddScoped<IBillQueries, BillQueries>();
        }
    }
}