using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.GetTotalExpensesByCategory
{
    public class GetTotalExpensesByCategoryQueryValidator : AbstractValidator<GetTotalExpensesByCategoryQuery>
    {
        public GetTotalExpensesByCategoryQueryValidator()
        {
            RuleFor(query => query.CurrentUserId).NotEmpty();
        }
    }
}
