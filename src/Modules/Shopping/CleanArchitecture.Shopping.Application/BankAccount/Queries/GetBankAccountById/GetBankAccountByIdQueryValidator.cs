using FluentValidation;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries.GetBankAccountById
{
    public class GetBankAccountByIdQueryValidator : AbstractValidator<GetBankAccountByIdQuery>
    {
        public GetBankAccountByIdQueryValidator()
        {
            RuleFor(query => query.Id).NotEmpty();
            RuleFor(query => query.CurrentUserId).NotEmpty();
        }
    }
}
