using FluentValidation;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands.DeleteFixedCost
{
    public class DeleteFixedCostCommandValidator : AbstractValidator<DeleteFixedCostCommand>
    {
        public DeleteFixedCostCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
