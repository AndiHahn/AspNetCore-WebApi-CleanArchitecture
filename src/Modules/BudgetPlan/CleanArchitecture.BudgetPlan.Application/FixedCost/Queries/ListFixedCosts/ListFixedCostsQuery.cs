using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Queries.ListFixedCosts
{
    public class ListFixedCostsQuery : IQuery<Result<IEnumerable<FixedCostDto>>>
    {
        public ListFixedCostsQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
