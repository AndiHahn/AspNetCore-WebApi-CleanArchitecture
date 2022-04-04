using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.DeleteBill
{
    public class DeleteBillCommandValidator : AbstractValidator<DeleteBillCommand>
    {
        public DeleteBillCommandValidator()
        {
            RuleFor(command => command.BillId).NotEmpty();
            RuleFor(command => command.CurrentUserId).NotEmpty();
        }
    }
}
