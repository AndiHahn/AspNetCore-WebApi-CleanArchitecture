using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Infrastructure.Database.Budget;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories
{
    internal abstract class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        private readonly BudgetContext context;

        protected EfRepository(BudgetContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<TEntity>> ListAsync(int pageSize, int pageIndex, CancellationToken cancellationToken = default)
        {
            var query = context.Set<TEntity>();
            int totalCount = await query.CountAsync(cancellationToken);
            var result = await query.Skip(pageSize * pageIndex).Take(pageSize).ToListAsync(cancellationToken);
            return new PagedResult<TEntity>(result, totalCount);
        }

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var addedEntity = context.Set<TEntity>().Add(entity).Entity;
            await context.SaveChangesAsync(cancellationToken);

            return addedEntity;
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
