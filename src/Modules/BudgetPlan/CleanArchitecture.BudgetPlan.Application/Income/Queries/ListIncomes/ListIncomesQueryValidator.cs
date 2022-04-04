using FluentValidation;

namespace CleanArchitecture.BudgetPlan.Application.Income.Queries.ListIncomes
{
    internal class ListIncomesQueryValidator : AbstractValidator<ListIncomesQuery>
    {
        public ListIncomesQueryValidator()
        {
            RuleFor(query => query.UserId).NotEmpty();
        }
    }
}
