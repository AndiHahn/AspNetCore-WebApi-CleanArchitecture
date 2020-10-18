using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Queries;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public IBillQueries BillQueries { get; private set; }

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
        }

        public async Task CreateAndMigrateAsync()
        {
            await Database.EnsureCreatedAsync();
            await Database.MigrateAsync();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
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

        public override EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }
    }
}
