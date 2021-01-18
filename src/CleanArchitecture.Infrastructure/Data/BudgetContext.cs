using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.SqlQueries;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data
{
    public class BudgetContext : DbContext, IBudgetContext
    {
        //DB Set's
        public DbSet<BillEntity> Bill { get; set; }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<AccountEntity> Account { get; set; }
        public DbSet<UserAccountEntity> UserAccount { get; set; }
        public DbSet<FixedCostEntity> FixedCost { get; set; }
        public DbSet<IncomeEntity> Income { get; set; }
        public DbSet<BillCategoryEntity> BillCategory { get; set; }

        //Queries
        public IBillQueries BillQueries { get; }

        public BudgetContext(DbContextOptions<BudgetContext> options,
                             IBillQueries billQueries)
            : base(options)
        {
            BillQueries = billQueries;
            billQueries.SetBudgetContext(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            UserAccountModelBuilder.ApplyModelBuilder(modelBuilder);

            //Soft deletable Entities
            //modelBuilder.Entity<EntityType>().HasQueryFilter(p => !p.Deleted);
        }

        public async Task MigrateAsync()
        {
            await Database.MigrateAsync();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                SetExpectedVersionForVersionableEntities();
                SetDeletedFlagForDeletedSoftDeletableEntities();
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

        private void SetExpectedVersionForVersionableEntities()
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

        private void SetDeletedFlagForDeletedSoftDeletableEntities()
        {
            ChangeTracker.DetectChanges();
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var entityEntry in deletedEntities)
            {
                if (entityEntry.Entity is ISoftDeletableEntity softDeletableEntity)
                {
                    entityEntry.State = EntityState.Unchanged;
                    softDeletableEntity.Deleted = true;
                }
            }
        }
    }
}
