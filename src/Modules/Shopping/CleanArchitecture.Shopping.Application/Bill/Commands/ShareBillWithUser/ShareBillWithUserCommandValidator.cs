using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.ShareBillWithUser
{
    public class ShareBillWithUserCommandValidator : AbstractValidator<ShareBillWithUserCommand>
    {
        public ShareBillWithUserCommandValidator()
        {
            RuleFor(command => command.BillId).NotEmpty();
            RuleFor(command => command.CurrentUserId).NotEmpty();
            RuleFor(command => command.ShareWithUserId).NotEmpty();
        }
    }
}
