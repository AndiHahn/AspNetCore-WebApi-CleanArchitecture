using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Common.Models.Query;
using CleanArchitecture.Common.Models.Resource.Bill;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Repositories.GenericQuery;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories.Sql
{
    public class BillRepository : EfRepository<BillEntity>, IBillRepository
    {
        private readonly IBudgetContext context;

        public BillRepository(IBudgetContext context)
        : base(context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<BillEntity>> ListByUserAsync<TEnumSort, TEnumFilter>(
            QueryParameter<TEnumSort, TEnumFilter> queryParams, Guid userId)
            where TEnumSort : Enum
            where TEnumFilter : Enum
        {
            var query = context.Set<BillEntity>()
                .ApplyFilter(queryParams.Filter)
                .ApplyOrderBy(queryParams.Sorting)
                .Where(b => b.CreatedByUserId == userId)
                .OrderByDescending(b => b.Date);

            int totalCount = query.Count();

            var queryResult = await query.ApplyPaging(queryParams).ToListAsync();

            return new PagedResult<BillEntity>(queryResult, totalCount);
        }

        public async Task<PagedResult<BillEntity>> SearchByUserAsync(BillSearchParameter searchParameter, Guid userId)
        {
            var query = context.UserBill
                .Include(ub => ub.Bill)
                .Where(ub => ub.UserId == userId ||
                             ub.Bill.CreatedByUserId == userId)
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

        public async Task<BillEntity> GetByIdWithUsersAsync(Guid id)
        {
            return await context.Bill
                .Include(b => b.UserBills)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
