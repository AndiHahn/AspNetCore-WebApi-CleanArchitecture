using FluentValidation;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands.CreateBankAccount
{
    public class CreateBankAccountCommandValidator : AbstractValidator<CreateBankAccountCommand>
    {
        public CreateBankAccountCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty();
        }
    }
}
