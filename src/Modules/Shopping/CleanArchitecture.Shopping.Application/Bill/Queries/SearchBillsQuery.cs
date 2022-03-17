using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Queries
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

    internal class SearchBillsQueryHandler : IQueryHandler<SearchBillsQuery, PagedResult<BillDto>>
    {
        private readonly IShoppingDbContext dbContext;
        private readonly IMapper mapper;

        public SearchBillsQueryHandler(
            IShoppingDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResult<BillDto>> Handle(SearchBillsQuery request, CancellationToken cancellationToken)
        {
            var query = this.dbContext.Bill
                .Include(b => b.SharedWithUsers)
                .OrderByDescending(b => b.Date)
                .Where(b => b.CreatedByUserId == request.CurrentUserId);

            if (request.IncludeShared)
            {
                query = query.Where(b => b.CreatedByUserId == request.CurrentUserId ||
                                         b.SharedWithUsers.Any(ub => ub.UserId == request.CurrentUserId));
            }

            if (request.Search is not null)
            {
                query = query.Where(b => b.ShopName.Contains(request.Search) ||
                                         b.Notes.Contains(request.Search));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var queryResult = await query
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<BillDto>(
                queryResult.Select(this.mapper.Map<BillDto>),
                totalCount);
        }
    }
}
