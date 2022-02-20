using CleanArchitecture.Shared.Core.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shared.Infrastructure.Database
{
    public class BaseDbContext<TContext> : DbContext
        where TContext : DbContext
    {
        public BaseDbContext(DbContextOptions<TContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyGlobalFilters<ISoftDeletableEntity>(s => !s.Deleted);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            SetExpectedVersionOnVersionableEntities();
            SetDeletedFlagOnDeletedSoftDeletableEntities();
            return base.SaveChangesAsync(cancellationToken);
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
