using CleanArchitecture.Shared.Core.Interfaces.Entities;
using CleanArchitecture.Shopping.Core.BankAccount;
using CleanArchitecture.Shopping.Core.Bill;
using CleanArchitecture.Shopping.Core.User;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shared.Infrastructure.Database.Budget
{
    public class BudgetContext : DbContext
    {
        public DbSet<Bill> Bill { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<BankAccount> BankAccount { get; set; }

        public DbSet<UserBankAccount> UserBankAccount { get; set; }

        public DbSet<UserBill> UserBill { get; set; }

        public BudgetContext(DbContextOptions<BudgetContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureEntities();

            modelBuilder.ApplyGlobalFilters<ISoftDeletableEntity>(s => !s.Deleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            SetExpectedVersionOnVersionableEntities();
            SetDeletedFlagOnDeletedSoftDeletableEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetExpectedVersionOnVersionableEntities()
        {
            ChangeTracker.Entries<IVersionableEntity>()
                .Where(e => e.State == EntityState.Modified).ToList()
                .ForEach(e =>
                {
                    var versionProperty = Entry(e.Entity).Property(nameof(IVersionableEntity.Version));
                    versionProperty.OriginalValue = e.Entity.Version;
                });
        }

        private void SetDeletedFlagOnDeletedSoftDeletableEntities()
        {
            ChangeTracker.Entries<ISoftDeletableEntity>()
                .Where(e => e.State == EntityState.Modified).ToList()
                .ForEach(e =>
                {
                    e.State = EntityState.Modified;
                    e.Entity.Deleted = true;
                });
        }
    }
}
