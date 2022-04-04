using FluentValidation;

#nullable enable

namespace CleanArchitecture.Shopping.Application.Bill.Commands.CreateBill
{
    public class CreateBillCommandValidator : AbstractValidator<CreateBillCommand>
    {
        public CreateBillCommandValidator()
        {
            RuleFor(c => c.BankAccountId).NotEmpty();
            RuleFor(c => c.ShopName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(c => c.Price).GreaterThan(0);
        }
    }
}
