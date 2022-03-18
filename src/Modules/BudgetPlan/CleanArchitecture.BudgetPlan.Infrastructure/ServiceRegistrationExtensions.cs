using CleanArchitecture.BudgetPlan.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.BudgetPlan.Infrastructure
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IBudgetPlanDbContext, BudgetPlanDbContext>(
                options => options
                    .UseSqlServer(
                        configuration.GetConnectionString("ApplicationDbConnection"),
                        opt => opt.MigrationsHistoryTable("__EFMigrationsHistory_BudgetPlan")));
        }
    }
}
