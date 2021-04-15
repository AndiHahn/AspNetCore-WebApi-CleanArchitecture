using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.GenericQuery;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Interfaces.SqlQueries;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Core.Models.Domain.Bill;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.SqlQueries
{
    public class BillQueries : IBillQueries
    {
        private IBudgetContext context;
        private readonly ICurrentUserService currentUserService;

        public BillQueries(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public void SetBudgetContext(IBudgetContext context)
        {
            this.context = context;
        }

        public virtual async Task<PagedResult<BillEntity>> QueryAsync(
            BillQueryParameter queryParameter)
        {
            Guid currentUserId = currentUserService.GetCurrentUserId();

            var query = context.Bill
                .OrderByDescending(b => b.Date)
                .ApplyFilter(queryParameter.Filter)
                .ApplyOrderBy(queryParameter.Sorting)
                .Where(b => b.CreatedByUserId == currentUserId);

            int totalCount = query.Count();

            var queryResult = await query.ApplyPaging(queryParameter).ToListAsync();

            return new PagedResult<BillEntity>(queryResult, totalCount);
        }

        public virtual async Task<PagedResult<BillEntity>> SearchAsync(
            BillSearchParameter searchParameter)
        {
            Guid currentUserId = currentUserService.GetCurrentUserId();

            var query = context.UserBill
                .Include(ub => ub.Bill)
                .Where(ub => ub.UserId == currentUserId ||
                             ub.Bill.CreatedByUserId == currentUserId)
                .Select(ub => ub.Bill);

            if (searchParameter.Search != null)
            {
                query = query.Where(b => b.ShopName.Contains(searchParameter.Search) ||
                                         b.Notes.Contains(searchParameter.Search));
            }

            int totalCount = query.Count();

            var queryResult = await query.ApplyPaging(searchParameter).ToListAsync();

            return new PagedResult<BillEntity>(queryResult, totalCount);
        }
    }
}
