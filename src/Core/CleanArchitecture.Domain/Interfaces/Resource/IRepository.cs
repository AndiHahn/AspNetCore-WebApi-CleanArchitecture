using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Common.Models.Query;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> ListAllAsync();
        Task<PagedResult<TEntity>> ListAsync<TEnumSort, TEnumFilter>(
            QueryParameter<TEnumSort, TEnumFilter> queryParams)
            where TEnumSort : Enum
            where TEnumFilter : Enum;
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
