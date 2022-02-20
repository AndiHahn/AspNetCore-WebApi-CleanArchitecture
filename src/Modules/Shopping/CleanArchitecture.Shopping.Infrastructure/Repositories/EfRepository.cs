using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Infrastructure.Repositories
{
    internal abstract class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        private readonly ShoppingDbContext context;

        protected EfRepository(ShoppingDbContext context)
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

        public TEntity Add(TEntity entity)
        {
            return context.Set<TEntity>().Add(entity).Entity;
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
