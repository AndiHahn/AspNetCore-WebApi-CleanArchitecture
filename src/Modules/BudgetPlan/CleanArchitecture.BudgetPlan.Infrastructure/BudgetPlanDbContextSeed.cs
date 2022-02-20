using CleanArchitecture.BudgetPlan.Core;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Infrastructure
{
    public static class BudgetPlanDbContextSeed
    {
        public static async Task SeedAsync(BudgetPlanDbContext budgetPlanDbContext, Guid userId)
        {
            await InsertIncomesAsync(budgetPlanDbContext, userId);
            await InsertFixedCostsAsync(budgetPlanDbContext, userId);

            await budgetPlanDbContext.SaveChangesAsync();
        }

        private static async Task InsertIncomesAsync(BudgetPlanDbContext dbContext, Guid userId)
        {
            if (await dbContext.Income.AnyAsync())
            {
                return;
            }

            var income1 = new Income(userId, "Salary", 2000, Duration.Monthly);
            var income2 = new Income(userId, "Rent", 700, Duration.Monthly);

            await dbContext.Income.AddAsync(income1);
            await dbContext.Income.AddAsync(income2);
        }

        private static async Task InsertFixedCostsAsync(BudgetPlanDbContext dbContext, Guid userId)
        {
            if (await dbContext.FixedCost.AnyAsync())
            {
                return;
            }

            var fixedCost1 = new FixedCost(userId, "Flat", 600, Duration.Monthly, CostCategory.FlatAndOperating);
            var fixedCost2 = new FixedCost(userId, "Netflix", 12, Duration.Monthly, CostCategory.FlatAndOperating);
            var fixedCost3 = new FixedCost(userId, "Tires", 300, Duration.Year, CostCategory.MotorVehicle);
            var fixedCost4 = new FixedCost(userId, "Accident Insurance", 12, Duration.Year, CostCategory.Insurance);
            var fixedCost5 = new FixedCost(userId, "Retirement", 30, Duration.QuarterYear, CostCategory.Insurance);

            await dbContext.FixedCost.AddAsync(fixedCost1);
            await dbContext.FixedCost.AddAsync(fixedCost2);
            await dbContext.FixedCost.AddAsync(fixedCost3);
            await dbContext.FixedCost.AddAsync(fixedCost4);
            await dbContext.FixedCost.AddAsync(fixedCost5);
        }
    }
}
