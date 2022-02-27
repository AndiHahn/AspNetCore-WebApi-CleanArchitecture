using CleanArchitecture.BudgetPlan.Application;

namespace CleanArchitecture.BudgetPlan.Api.Dtos
{
    public class FixedCostCreateDto
    {
        public string Name { get; set; }

        public double Value { get; set; }

        public DurationDto Duration { get; set; }

        public CostCategoryDto Category { get; set; }
    }
}
