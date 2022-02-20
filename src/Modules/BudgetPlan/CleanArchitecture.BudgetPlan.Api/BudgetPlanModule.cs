using CleanArchitecture.BudgetPlan.Application;
using CleanArchitecture.BudgetPlan.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.BudgetPlan.Api
{
    public static class BudgetPlanModule
    {
        public static void AddBudgetPlanModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
        }

        public static IMvcBuilder AddBudgetPlanModule(this IMvcBuilder builder)
        {
            return builder.AddApplicationPart(Assembly.GetExecutingAssembly());
        }

        public static async Task MigrateAndSeedBudgetPlanDbContextAsync(IServiceProvider services, Guid userId)
        {
            var budgetPlanDbContext = services.GetRequiredService<BudgetPlanDbContext>();
            await budgetPlanDbContext.Database.MigrateAsync();
            await BudgetPlanDbContextSeed.SeedAsync(budgetPlanDbContext, userId);
        }
    }
}
