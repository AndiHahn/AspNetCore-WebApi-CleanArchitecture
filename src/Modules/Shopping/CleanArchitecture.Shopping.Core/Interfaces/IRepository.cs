using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models;
using CSharpFunctionalExtensions;

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<TEntity>> ListAsync(int pageSize, int pageIndex, CancellationToken cancellationToken = default);

        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
