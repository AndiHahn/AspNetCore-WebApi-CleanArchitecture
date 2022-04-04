using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands.DeleteFixedCost
{
    public class DeleteFixedCostCommand : ICommand<Result>
    {
        public DeleteFixedCostCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
