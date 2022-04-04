using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.SearchBills
{
    public class SearchBillsQuery : IQuery<PagedResult<BillDto>>
    {
        public SearchBillsQuery(
            Guid currentUserId,
            int pageSize = 10,
            int pageIndex = 0,
            bool includeShared = false,
            string search = null)
        {
            this.CurrentUserId = currentUserId;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.IncludeShared = includeShared;
            this.Search = search;
        }

        public Guid CurrentUserId { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public bool IncludeShared { get; }

        public string Search { get; }
    }
}
