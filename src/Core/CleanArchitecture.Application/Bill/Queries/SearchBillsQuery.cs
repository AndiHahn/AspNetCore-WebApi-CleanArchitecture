using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Models;
using MediatR;

namespace CleanArchitecture.Application.Bill
{
    public class SearchBillsQuery : IRequest<Result<PagedResult<BillDto>>>
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

    internal class SearchBillsQueryHandler : IRequestHandler<SearchBillsQuery, Result<PagedResult<BillDto>>>
    {
        private readonly IBillRepository billRepository;
        private readonly IMapper mapper;

        public SearchBillsQueryHandler(
            IBillRepository billRepository,
            IMapper mapper)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<PagedResult<BillDto>>> Handle(SearchBillsQuery request, CancellationToken cancellationToken)
        {
            var result = await billRepository.SearchBillsAsync(
                request.CurrentUserId,
                request.Search,
                request.PageSize,
                request.PageIndex,
                request.IncludeShared,
                cancellationToken);

            return new PagedResult<BillDto>(
                result.Result.Select(this.mapper.Map<BillDto>),
                result.TotalCount);
        }
    }
}
