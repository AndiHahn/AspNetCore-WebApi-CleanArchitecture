using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.Income.Models;

namespace CleanArchitecture.Services.Income
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeModel>> GetByAccountIdsAsync(IEnumerable<int> accountIds);
        Task<IncomeModel> AddIncomeAsync(IncomeCreateModel createModel);
        Task DeleteIncomeAsync(int id);
    }
}