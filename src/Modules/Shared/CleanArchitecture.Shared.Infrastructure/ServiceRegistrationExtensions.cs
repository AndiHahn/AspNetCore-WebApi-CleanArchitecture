using CleanArchitecture.Shared.Core.Interfaces;
using CleanArchitecture.Shared.Infrastructure.Email;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Shared.Infrastructure
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddSharedInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, SendGridEmailService>();
        }
    }
}
