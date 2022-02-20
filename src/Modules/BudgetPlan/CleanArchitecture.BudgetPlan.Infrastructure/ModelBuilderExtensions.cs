using CleanArchitecture.BudgetPlan.Core;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Infrastructure
{
    internal static class ModelBuilderExtensions
    {
        public static void ConfigureEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Income>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Income>()
                .Property(i => i.Duration)
                .HasConversion(
                    d => d.Value,
                    d => Duration.FromValue(d));

            modelBuilder.Entity<FixedCost>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<FixedCost>()
                .Property(i => i.Duration)
                .HasConversion(
                    d => d.Value,
                    d => Duration.FromValue(d));

            modelBuilder.Entity<FixedCost>()
                .Property(i => i.Category)
                .HasConversion(
                    c => c.Value,
                    c => CostCategory.FromValue(c));
        }
    }
}
