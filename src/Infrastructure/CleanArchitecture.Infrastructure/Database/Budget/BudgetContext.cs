using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Database.Budget.Config;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database.Budget
{
    public class BudgetContext : DbContext, IBudgetContext
    {
        //DB Set's
        public DbSet<BillEntity> Bill { get; set; }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<BankAccountEntity> BankAccount { get; set; }
        public DbSet<UserBankAccountEntity> UserBankAccount { get; set; }
        public DbSet<UserBillEntity> UserBill { get; set; }

        public BudgetContext(DbContextOptions<BudgetContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            UserBankAccountModelBuilder.ApplyModelBuilder(modelBuilder);
            UserBillModelBuilder.ApplyModelBuilder(modelBuilder);

            modelBuilder.ApplyGlobalFilters<ISoftDeletableEntity>(s => !s.Deleted);
        }

        public async Task MigrateAsync()
        {
            await Database.MigrateAsync();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                SetExpectedVersionOnVersionableEntities();
                SetDeletedFlagOnDeletedSoftDeletableEntities();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    string entityName = entry.Entity.GetType().Name;
                    if (entry.Entity is BaseEntity be)
                    {
                        throw new ConflictException($"Could not save {entityName} with Id {be.Id}. Entity may have been modified or deleted since entities were loaded.");
                    }
                }
                throw new ConflictException("Concurrency conflict.");
            }
        }

        private void SetExpectedVersionOnVersionableEntities()
        {
            ChangeTracker.DetectChanges();
            var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            foreach (var entityEntry in modifiedEntities)
            {
                if (entityEntry.Entity is IVersionableEntity versionableEntity)
                {
                    var versionProperty = Entry(entityEntry.Entity).Property(nameof(IVersionableEntity.Version));
                    versionProperty.OriginalValue = versionableEntity.Version;
                }
            }
        }

        private void SetDeletedFlagOnDeletedSoftDeletableEntities()
        {
            ChangeTracker.DetectChanges();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var entityEntry in deletedEntities)
            {
                if (entityEntry.Entity is ISoftDeletableEntity softDeletableEntity)
                {
                    entityEntry.State = EntityState.Modified;
                    softDeletableEntity.Deleted = true;
                }
            }
        }
    }
}
