using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Infrastructure
{
    public class BudgetPlanDbContext : BaseDbContext<BudgetPlanDbContext>, IBudgetPlanDbContext
    {
        public DbSet<Income> Income { get; set; }

        public DbSet<FixedCost> FixedCost { get; set; }

        public BudgetPlanDbContext(DbContextOptions<BudgetPlanDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureEntities();
        }
    }
}