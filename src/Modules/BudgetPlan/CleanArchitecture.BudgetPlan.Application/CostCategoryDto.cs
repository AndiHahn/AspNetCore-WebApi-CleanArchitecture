using CleanArchitecture.BudgetPlan.Core;

namespace CleanArchitecture.BudgetPlan.Application
{
    public enum CostCategoryDto
    {
        FlatAndOperating,
        MotorVehicle,
        Insurance,
        Saving,
        Other
    }

    public static class CostCategoryExtensions
    {
        public static CostCategory FromDto(this CostCategoryDto category)
        {
            return category switch
            {
                CostCategoryDto.FlatAndOperating => CostCategory.FlatAndOperating,
                CostCategoryDto.MotorVehicle => CostCategory.MotorVehicle,
                CostCategoryDto.Insurance => CostCategory.Insurance,
                CostCategoryDto.Saving => CostCategory.Saving,
                CostCategoryDto.Other => CostCategory.Other,
                _ => throw new ArgumentException($"Value {category} is not valid.", nameof(category))
            };
        }

        public static CostCategoryDto ToDto(this CostCategory category)
        {
            return category.Name switch
            {
                nameof(CostCategory.FlatAndOperating) => CostCategoryDto.FlatAndOperating,
                nameof(CostCategory.MotorVehicle) => CostCategoryDto.MotorVehicle,
                nameof(CostCategory.Insurance) => CostCategoryDto.Insurance,
                nameof(CostCategory.Saving) => CostCategoryDto.Saving,
                nameof(CostCategory.Other) => CostCategoryDto.Other,
                _ => throw new ArgumentException($"Value {category} is not valid.", nameof(category))
            };
        }
    }
}
