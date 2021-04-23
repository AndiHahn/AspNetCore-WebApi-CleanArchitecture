using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Common.Models.Query;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Database.Budget;
using CleanArchitecture.Infrastructure.Repositories.GenericQuery;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public abstract class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly BudgetContext context;

        protected EfRepository(BudgetContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<PagedResult<TEntity>> ListAsync<TEnumSort, TEnumFilter>(
            QueryParameter<TEnumSort, TEnumFilter> queryParams)
            where TEnumSort : Enum
            where TEnumFilter : Enum
        {
            var query = context.Set<TEntity>()
                .ApplyFilter(queryParams.Filter)
                .ApplyOrderBy(queryParams.Sorting);

            int totalCount = query.Count();

            var queryResult = await query.ApplyPaging(queryParams).ToListAsync();

            return new PagedResult<TEntity>(queryResult, totalCount);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var addedEntity = context.Set<TEntity>().Add(entity).Entity;
            await context.SaveChangesAsync();

            return addedEntity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
