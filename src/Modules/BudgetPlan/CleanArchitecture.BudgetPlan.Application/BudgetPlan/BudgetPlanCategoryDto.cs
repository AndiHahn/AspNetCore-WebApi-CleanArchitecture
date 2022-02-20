using CleanArchitecture.BudgetPlan.Core;

namespace CleanArchitecture.BudgetPlan.Application.BudgetPlan
{
    public enum BudgetPlanCategoryDto
    {
        FlatAndOperating,
        MotorVehicle,
        Insurance,
        Saving,
        LivingCosts,
        Other
    }

    public static class BudgetPlanCategoryDtoExtensions
    {
        public static BudgetPlanCategoryDto FromCategory(this CostCategory category)
        {
            return category.Name switch
            {
                nameof(CostCategory.FlatAndOperating) => BudgetPlanCategoryDto.FlatAndOperating,
                nameof(CostCategory.MotorVehicle) => BudgetPlanCategoryDto.MotorVehicle,
                nameof(CostCategory.Insurance) => BudgetPlanCategoryDto.Insurance,
                nameof(CostCategory.Saving) => BudgetPlanCategoryDto.Saving,
                nameof(CostCategory.Other) => BudgetPlanCategoryDto.Other,
                _ => throw new ArgumentException($"Value {category} is not valid.", nameof(category))
            };
        }
    }
}
