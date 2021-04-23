using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Common.Models.Query;
using CleanArchitecture.Common.Models.Resource.Bill;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IBillRepository : IRepository<BillEntity>
    {
        Task<PagedResult<BillEntity>> ListByUserAsync<TEnumSort, TEnumFilter>(
            QueryParameter<TEnumSort, TEnumFilter> queryParams, Guid userId)
            where TEnumSort : Enum
            where TEnumFilter : Enum;

        Task<PagedResult<BillEntity>> SearchByUserAsync(BillSearchParameter searchParameter, Guid userId);

        Task<BillEntity> GetByIdWithUsersAsync(Guid id);
        Task<IEnumerable<BillEntity>> GetSharedBillsAsync(Guid currentUserId);
    }
}
