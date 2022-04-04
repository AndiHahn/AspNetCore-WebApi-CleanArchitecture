using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.UpdateBill
{
    public class UpdateBillCommandValidator : AbstractValidator<UpdateBillCommand>
    {
        public UpdateBillCommandValidator()
        {
            RuleFor(command => command.CurrentUserId).NotEmpty();
            RuleFor(command => command.BillId).NotEmpty();
        }
    }
}
