using FluentValidation;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands.DeleteIncome
{
    public class DeleteIncomeCommandValidator : AbstractValidator<DeleteIncomeCommand>
    {
        public DeleteIncomeCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
