using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.DeleteImageFromBill
{
    public class DeleteImageFromBillCommandValidator : AbstractValidator<DeleteImageFromBillCommand>
    {
        public DeleteImageFromBillCommandValidator()
        {
            RuleFor(command => command.BillId).NotEmpty();
            RuleFor(command => command.CurrentUserId).NotEmpty();
        }
    }
}
