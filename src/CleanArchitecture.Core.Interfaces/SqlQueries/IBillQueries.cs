using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Core.Models.Domain.Bill;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Interfaces.SqlQueries
{
    public interface IBillQueries
    {
        void SetBudgetContext(IBudgetContext context);
        Task<PagedResult<BillEntity>> QueryAsync(BillQueryParameter queryParameter);
        Task<PagedResult<BillEntity>> SearchAsync(BillSearchParameter searchParameter);
    }
}
