using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Queries
{
    public class GetTotalExpensesByCategoryQuery : IQuery<Result<IEnumerable<ExpensesDto>>>
    {
        public GetTotalExpensesByCategoryQuery(Guid currentUserId)
        {
            this.CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetTotalExpensesQueryHandler : IQueryHandler<GetTotalExpensesByCategoryQuery, Result<IEnumerable<ExpensesDto>>>
    {
        private readonly IShoppingDbContext dbContext;

        public GetTotalExpensesQueryHandler(
            IShoppingDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<IEnumerable<ExpensesDto>>> Handle(
            GetTotalExpensesByCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var expensesByCategory = await this.dbContext.Bill
                .Where(b => b.CreatedByUserId == request.CurrentUserId ||
                            b.SharedWithUsers.Any(ub => ub.UserId == request.CurrentUserId))
                .GroupBy(b => b.Category)
                .Select(b => new
                {
                    Category = b.Key,
                    Total = b.Select(x => x.Price).Sum()
                })
                .ToDictionaryAsync(k => k.Category, v => v.Total, cancellationToken);

            return Result<IEnumerable<ExpensesDto>>.Success(expensesByCategory.Select(e => new ExpensesDto
            {
                Category = e.Key,
                TotalAmount = e.Value
            }));
        }
    }
}
