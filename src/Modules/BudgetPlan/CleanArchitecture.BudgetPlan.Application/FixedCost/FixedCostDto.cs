using AutoMapper;
using CleanArchitecture.Shared.Application.Mapping;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost
{
    public class FixedCostDto : IMappableDto<Core.FixedCost>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public DurationDto Duration { get; set; }

        public CostCategoryDto CostCategory { get; set; }

        public void MappingConfig(Profile profile)
        {
            profile.CreateMap<Core.FixedCost, FixedCostDto>()
                .ForMember(f => f.Duration, f => f.MapFrom(m => m.Duration.ToDto()))
                .ForMember(f => f.CostCategory, f => f.MapFrom(m => m.Category.ToDto()));
        }
    }
}
