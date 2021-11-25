using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Shared;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;

namespace CleanArchitecture.Infrastructure.Database.Budget
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

            modelBuilder.ConfigureSmartEnum();

            modelBuilder.ApplyGlobalFilters<ISoftDeletableEntity>(s => !s.Deleted);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ChangeTracker.DetectChanges();
                SetExpectedVersionOnVersionableEntities();
                SetDeletedFlagOnDeletedSoftDeletableEntities();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    string entityName = entry.Entity.GetType().Name;
                    if (entry.Entity is Entity<Guid> entity)
                    {
                        throw new ConflictException($"Could not save {entityName} with Id {entity.Id}. Entity may have been modified or deleted since entities were loaded.", ex);
                    }
                }
                throw new ConflictException("Concurrency conflict.", ex);
            }
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
