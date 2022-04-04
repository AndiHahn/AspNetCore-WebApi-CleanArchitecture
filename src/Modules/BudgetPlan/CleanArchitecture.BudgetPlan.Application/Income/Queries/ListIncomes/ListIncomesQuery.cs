using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.Income.Queries.ListIncomes
{
    public class ListIncomesQuery : IQuery<Result<IEnumerable<IncomeDto>>>
    {
        public ListIncomesQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
