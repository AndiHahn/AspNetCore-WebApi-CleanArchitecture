using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.AddImageToBill
{
    public class AddImageToBillCommandValidator : AbstractValidator<AddImageToBillCommand>
    {
        public AddImageToBillCommandValidator()
        {
            RuleFor(command => command.BillId).NotEmpty();
            RuleFor(command => command.CurrentUserId).NotEmpty();
            RuleFor(command => command.Image).NotNull();
        }
    }
}
