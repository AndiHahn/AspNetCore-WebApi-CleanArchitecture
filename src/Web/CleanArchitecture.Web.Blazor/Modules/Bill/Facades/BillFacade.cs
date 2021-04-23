using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.CrudServices;
using CleanArchitecture.Application.CrudServices.Models.Bill;
using CleanArchitecture.Web.Blazor.Modules.Core.Services;

namespace CleanArchitecture.Web.Blazor.Modules.Bill.Facades
{
    public class BillFacade : IBillFacade
    {
        private readonly IBillService billService;
        private readonly ICurrentUserService currentUserService;

        public BillFacade(
            IBillService billService,
            ICurrentUserService currentUserService)
        {
            this.billService = billService ?? throw new ArgumentNullException(nameof(billService));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<IEnumerable<BillModel>> GetBillsAsync(int pageSize, int pageIndex)
        {
            var queryParameter = new BillQueryParameter
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };

            Guid currentUserId = currentUserService.GetCurrentUserId();

            var result = await billService.QueryAsync(queryParameter, currentUserId);
            return result.Result;
        }
    }
}
