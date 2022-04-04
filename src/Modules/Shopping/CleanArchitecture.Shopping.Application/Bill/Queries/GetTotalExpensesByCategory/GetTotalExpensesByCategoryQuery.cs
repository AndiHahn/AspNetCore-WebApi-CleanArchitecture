using System;
using System.Collections.Generic;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.GetTotalExpensesByCategory
{
    public class GetTotalExpensesByCategoryQuery : IQuery<Result<IEnumerable<ExpensesDto>>>
    {
        public GetTotalExpensesByCategoryQuery(Guid currentUserId)
        {
            this.CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }
}
