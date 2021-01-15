using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.GenericQuery;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Models;
using CleanArchitecture.Core.Interfaces.Services.Bill.Models;
using CleanArchitecture.Core.Interfaces.SqlQueries;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.SqlQueries
{
    public class BillQueries : IBillQueries
    {
        private IBudgetContext context;

        public void SetBudgetContext(IBudgetContext context)
        {
            this.context = context;
        }

        public virtual async Task<PagedResult<BillEntity>> QueryAsync(
            BillQueryParameter queryParameter)
        {
            var query = context.Bill
                .OrderByDescending(b => b.Date)
                .ApplyFilter(queryParameter.Filter)
                .ApplyOrderBy(queryParameter.Sorting);

            int totalCount = query.Count();

            var queryResult = await query.ApplyPaging(queryParameter).ToListAsync();

            return new PagedResult<BillEntity>(queryResult, totalCount);
        }

        public virtual async Task<PagedResult<BillEntity>> SearchAsync(
            BillSearchParameter searchParameter)
        {
            var query = context.Bill
                .OrderByDescending(b => b.Date)
                .Where(b => searchParameter.AccountIds.Contains(b.AccountId));

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
