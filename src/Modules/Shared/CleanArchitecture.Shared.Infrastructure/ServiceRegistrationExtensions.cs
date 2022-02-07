using CleanArchitecture.Shared.Core.Interfaces;
using CleanArchitecture.Shared.Infrastructure.Database.Budget;
using CleanArchitecture.Shared.Infrastructure.Database.Identity;
using CleanArchitecture.Shared.Infrastructure.Email;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Shared.Infrastructure
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddSharedInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IEmailService, SendGridEmailService>();

            services.RegisterDatabase(configuration);
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
