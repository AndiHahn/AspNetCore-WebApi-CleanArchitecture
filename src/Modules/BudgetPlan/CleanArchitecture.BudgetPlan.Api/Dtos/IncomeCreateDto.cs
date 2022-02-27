using CleanArchitecture.BudgetPlan.Application;

namespace CleanArchitecture.BudgetPlan.Api.Dtos
{
    public class IncomeCreateDto
    {
        public string Name { get; set; }

        public double Value { get; set; }

        public DurationDto Duration { get; set; }
    }
}
