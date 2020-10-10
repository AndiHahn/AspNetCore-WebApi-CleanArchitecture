using CleanArchitecture.Core.Interfaces.Image;
using CleanArchitecture.Core.Interfaces.Queries;
using CleanArchitecture.Core.Queries;
using CleanArchitecture.Core.Services.Image;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Core
{
    public static class CoreRegistrationExtensions
    {
        public static void RegisterCore(this IServiceCollection services)
        {
            services.AddScoped<IImageStorageService, ImageStorageService>();
            services.AddScoped<IBillQueries, BillQueries>();
        }
    }
}