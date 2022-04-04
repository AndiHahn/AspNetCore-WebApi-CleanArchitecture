using FluentValidation;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands.ShareAccountWithUser
{
    public class ShareAccountWithUserCommandValidator : AbstractValidator<ShareAccountWithUserCommand>
    {
        public ShareAccountWithUserCommandValidator()
        {
            RuleFor(c => c.AccountId).NotEmpty();
            RuleFor(c => c.ShareWithUserId).NotEmpty();
        }
    }
}
