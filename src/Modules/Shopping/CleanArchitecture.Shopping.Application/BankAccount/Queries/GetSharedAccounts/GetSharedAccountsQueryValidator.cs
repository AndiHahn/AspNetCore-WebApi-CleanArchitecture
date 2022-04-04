using FluentValidation;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries.GetSharedAccounts
{
    public class GetSharedAccountsQueryValidator : AbstractValidator<GetSharedAccountsQuery>
    {
        public GetSharedAccountsQueryValidator()
        {
            RuleFor(query => query.CurrentUserId).NotEmpty();
        }
    }
}
