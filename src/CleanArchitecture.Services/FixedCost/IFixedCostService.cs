using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.FixedCost.Models;

namespace CleanArchitecture.Services.FixedCost
{
    public interface IFixedCostService
    {
        Task<IEnumerable<FixedCostModel>> GetByAccountIdsAsync(IEnumerable<int> accountIds);
        Task<FixedCostModel> AddFixedCostAsync(FixedCostCreateModel createModel);
        Task DeleteFixedCostAsync(int id);
    }
}