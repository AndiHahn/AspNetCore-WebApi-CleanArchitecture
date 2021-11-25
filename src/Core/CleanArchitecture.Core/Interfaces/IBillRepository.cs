using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models;

#nullable enable

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBillRepository : IRepository<Bill>
    {
        Task<PagedResult<Bill>> SearchBillsAsync(Guid userId,
            string? searchString = null,
            int pageSize = 100,
            int pageIndex = 0,
            bool includeShared = false,
            CancellationToken cancellationToken = default);

        Task<Bill> GetByIdWithUsersAsync(Guid id, CancellationToken cancellationToken = default);

        Task<(DateTime MinDate, DateTime MaxDate)> GetMinAndMaxBillDateAsync(CancellationToken cancellationToken = default);

        Task<Dictionary<Category, double>> GetExpensesByCategoryAsync(Guid currentUserId, CancellationToken cancellationToken = default);
    }
}
