using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.BudgetPlan.Queries.GetBudgetPlan
{
    public class GetBudgetPlanQuery : IQuery<Result<BudgetPlanDto>>
    {
        public GetBudgetPlanQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; }
    }
}
