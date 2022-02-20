using CleanArchitecture.Shared.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Core
{
    public interface IBudgetPlanDbContext : IDbContext
    {
        public DbSet<Income> Income { get; }

        public DbSet<FixedCost> FixedCost { get; }
    }
}
