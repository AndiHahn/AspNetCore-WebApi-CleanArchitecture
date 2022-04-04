using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands.AddFixedCost
{
    public class AddFixedCostCommand : ICommand<Result<FixedCostDto>>
    {
        public AddFixedCostCommand(
            Guid userId,
            string name,
            double value,
            Duration duration,
            CostCategory category)
        {
            UserId = userId;
            Name = name;
            Value = value;
            Duration = duration;
            Category = category;
        }

        public Guid UserId { get; }

        public string Name { get; }

        public double Value { get; }

        public Duration Duration { get; }

        public CostCategory Category { get; }
    }
}
