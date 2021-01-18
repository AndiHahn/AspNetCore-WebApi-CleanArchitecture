using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.FixedCost.Models;

namespace CleanArchitecture.Core.Interfaces.Services.FixedCost
{
    public interface IFixedCostService
    {
        Task<IEnumerable<FixedCostModel>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds);
        Task<FixedCostModel> AddFixedCostAsync(FixedCostCreateModel createModel);
        Task DeleteFixedCostAsync(Guid id);
    }
}