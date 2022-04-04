using FluentValidation;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries.GetBankAccounts
{
    public class GetBankAccountsQueryValidator : AbstractValidator<GetBankAccountsQuery>
    {
        public GetBankAccountsQueryValidator()
        {
            RuleFor(query => query.CurrentUserId).NotEmpty();
        }
    }
}
