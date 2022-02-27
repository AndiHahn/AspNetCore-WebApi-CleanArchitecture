using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Result;
using CSharpFunctionalExtensions;

#nullable enable

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<TEntity>> ListAsync(int pageSize, int pageIndex, CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
