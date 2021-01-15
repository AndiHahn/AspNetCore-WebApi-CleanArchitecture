using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Models;
using CleanArchitecture.Core.Interfaces.Services.Bill.Models;
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
