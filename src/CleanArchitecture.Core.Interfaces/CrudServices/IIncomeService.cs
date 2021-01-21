using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.Income;

namespace CleanArchitecture.Core.Interfaces.CrudServices
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeModel>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds);
        Task<IncomeModel> AddIncomeAsync(IncomeCreateModel createModel);
        Task DeleteIncomeAsync(Guid id);
    }
}