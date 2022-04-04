using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.SearchBills
{
    public class SearchBillsQueryValidator : AbstractValidator<SearchBillsQuery>
    {
        public SearchBillsQueryValidator()
        {
            RuleFor(query => query.CurrentUserId).NotEmpty();
            RuleFor(query => query.PageSize).GreaterThan(0);
            RuleFor(query => query.PageIndex).GreaterThanOrEqualTo(0);
        }
    }
}
