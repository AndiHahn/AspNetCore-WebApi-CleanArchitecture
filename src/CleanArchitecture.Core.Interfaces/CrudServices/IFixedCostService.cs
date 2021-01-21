using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.FixedCost;

namespace CleanArchitecture.Core.Interfaces.CrudServices
{
    public interface IFixedCostService
    {
        Task<IEnumerable<FixedCostModel>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds);
        Task<FixedCostModel> AddFixedCostAsync(FixedCostCreateModel createModel);
        Task DeleteFixedCostAsync(Guid id);
    }
}