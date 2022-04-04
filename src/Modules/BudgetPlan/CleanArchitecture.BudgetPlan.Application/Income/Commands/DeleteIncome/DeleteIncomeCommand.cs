using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands.DeleteIncome
{
    public class DeleteIncomeCommand : ICommand<Result>
    {
        public DeleteIncomeCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
