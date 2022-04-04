using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands.AddIncome
{
    public class AddIncomeCommand : ICommand<Result<IncomeDto>>
    {
        public AddIncomeCommand(Guid userId, string name, double value, Duration duration)
        {
            UserId = userId;
            Name = name;
            Value = value;
            Duration = duration;
        }

        public Guid UserId { get; }

        public string Name { get; }

        public double Value { get; }

        public Duration Duration { get; }
    }
}
