using CleanArchitecture.BudgetPlan.Application;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.BudgetPlan.Api.Dtos
{
    public class IncomeCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Value { get; set; }

        [Required]
        public DurationDto Duration { get; set; }
    }
}
