using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.Bill;
using CleanArchitecture.Web.Blazor.Modules.Core.Services;
using MediatR;

namespace CleanArchitecture.Web.Blazor.Modules.Bill.Facades
{
    public class BillFacade : IBillFacade
    {
        private readonly ISender sender;
        private readonly ICurrentUserService currentUserService;

        public BillFacade(
            ISender sender,
            ICurrentUserService currentUserService)
        {
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<IEnumerable<BillDto>> GetBillsAsync(int pageSize, int pageIndex)
        {
            Guid currentUserId = currentUserService.GetCurrentUserId();

            var result = await this.sender.Send(new SearchBillsQuery(currentUserId, pageSize, pageIndex));

            return result.Value.Result;
        }
    }
}
