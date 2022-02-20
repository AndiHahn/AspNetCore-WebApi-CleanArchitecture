using CleanArchitecture.BudgetPlan.Application;
using CleanArchitecture.BudgetPlan.Application.FixedCost;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.BudgetPlan.Api.Dtos
{
    public class FixedCostCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Value { get; set; }

        [Required]
        public DurationDto Duration { get; set; }

        [Required]
        public CostCategoryDto Category { get; set; }
    }
}
