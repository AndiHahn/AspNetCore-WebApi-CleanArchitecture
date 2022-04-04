using FluentValidation;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Queries.ListFixedCosts
{
    public class ListFixedCostsQueryValidator : AbstractValidator<ListFixedCostsQuery>
    {
        public ListFixedCostsQueryValidator()
        {
            RuleFor(query => query.UserId).NotEmpty();
        }
    }
}
