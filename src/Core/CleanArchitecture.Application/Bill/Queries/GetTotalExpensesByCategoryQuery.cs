using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Bill
{
    public class GetTotalExpensesByCategoryQuery : IRequest<Result<IEnumerable<ExpensesDto>>>
    {
        public GetTotalExpensesByCategoryQuery(Guid currentUserId)
        {
            this.CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetTotalExpensesQueryHandler : IRequestHandler<GetTotalExpensesByCategoryQuery, Result<IEnumerable<ExpensesDto>>>
    {
        private readonly IBillRepository billRepository;

        public GetTotalExpensesQueryHandler(
            IBillRepository billRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        }

        public async Task<Result<IEnumerable<ExpensesDto>>> Handle(
            GetTotalExpensesByCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var expensesByCategory = await this.billRepository.GetExpensesByCategoryAsync(request.CurrentUserId, cancellationToken);

            return Result<IEnumerable<ExpensesDto>>.Success(expensesByCategory.Select(e => new ExpensesDto
            {
                Category = e.Key,
                TotalAmount = e.Value
            }));
        }
    }
}
