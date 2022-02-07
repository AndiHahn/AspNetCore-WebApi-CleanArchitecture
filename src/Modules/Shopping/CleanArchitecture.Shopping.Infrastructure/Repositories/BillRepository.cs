using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shared.Infrastructure.Database.Budget;
using CleanArchitecture.Shopping.Core.Bill;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace CleanArchitecture.Shopping.Infrastructure.Repositories
{
    internal class BillRepository : EfRepository<Bill>, IBillRepository
    {
        private readonly BudgetContext context;

        public BillRepository(BudgetContext context)
        : base(context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<Bill>> SearchBillsAsync(
            Guid userId,
            string? searchString = null,
            int pageSize = 100,
            int pageIndex = 0,
            bool includeShared = false,
            CancellationToken cancellationToken = default)
        {
            var query = context.Bill
                .Include(b => b.SharedWithUsers)
                .OrderByDescending(b => b.Date)
                .Where(b => b.CreatedByUserId == userId);

            if (includeShared)
            {
                query = query.Where(b => b.CreatedByUserId == userId ||
                                         b.SharedWithUsers.Any(ub => ub.UserId == userId));
            }

            if (searchString != null)
            {
                query = query.Where(b => b.ShopName.Contains(searchString) ||
                                         b.Notes.Contains(searchString));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var queryResult = await query
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Bill>(queryResult, totalCount);
        }

        public Task<Bill> GetByIdWithUsersAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return context.Bill
                .Include(b => b.SharedWithUsers)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<(DateTime MinDate, DateTime MaxDate)> GetMinAndMaxBillDateAsync(CancellationToken cancellationToken = default)
        {
            return (await this.context.Bill
                .Select(b => new Tuple<DateTime, DateTime>(
                    this.context.Bill.Min(b => b.Date),
                    this.context.Bill.Max(b => b.Date)
                ))
                .FirstOrDefaultAsync(cancellationToken))
                .ToValueTuple();
        }

        public Task<Dictionary<Category, double>> GetExpensesByCategoryAsync(
            Guid currentUserId,
            CancellationToken cancellationToken = default)
        {
            return this.context.Bill
                .Where(b => b.CreatedByUserId == currentUserId ||
                            b.SharedWithUsers.Any(ub => ub.UserId == currentUserId))
                .GroupBy(b => b.Category)
                .Select(b => new
                {
                    Category = b.Key,
                    Total = b.Select(x => x.Price).Sum()
                })
                .ToDictionaryAsync(k => k.Category, v => v.Total, cancellationToken);
        }
    }
}
