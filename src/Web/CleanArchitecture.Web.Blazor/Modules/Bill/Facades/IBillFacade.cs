using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.Bill;

namespace CleanArchitecture.Web.Blazor.Modules.Bill.Facades
{
    public interface IBillFacade
    {
        Task<IEnumerable<BillModel>> GetBillsAsync(int pageSize, int pageIndex);
    }
}
