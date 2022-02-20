using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost
{
    public class FixedCostDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public DurationDto Duration { get; set; }

        public CostCategoryDto CostCategory { get; set; }

        internal static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<CostCategory, CostCategoryDto>();
        }
    }
}
