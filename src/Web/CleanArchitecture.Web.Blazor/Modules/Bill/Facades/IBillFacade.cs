using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Shopping.Application.Bill;

namespace CleanArchitecture.Web.Blazor.Modules.Bill.Facades
{
    public interface IBillFacade
    {
        Task<IEnumerable<BillDto>> GetBillsAsync(int pageSize, int pageIndex);
    }
}
