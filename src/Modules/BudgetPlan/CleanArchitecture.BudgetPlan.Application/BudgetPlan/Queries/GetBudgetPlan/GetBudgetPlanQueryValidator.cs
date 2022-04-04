using FluentValidation;

namespace CleanArchitecture.BudgetPlan.Application.BudgetPlan.Queries.GetBudgetPlan
{
    public class GetBudgetPlanQueryValidator : AbstractValidator<GetBudgetPlanQuery>
    {
        public GetBudgetPlanQueryValidator()
        {
            RuleFor(query => query.UserId).NotEmpty();
        }
    }
}
