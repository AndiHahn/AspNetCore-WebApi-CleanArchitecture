using AutoMapper;
using CleanArchitecture.Shared.Application.Mapping;

namespace CleanArchitecture.BudgetPlan.Application.Income
{
    public class IncomeDto : IMappableDto<Core.Income>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public DurationDto Duration { get; set; }

        public void MappingConfig(Profile profile)
        {
            profile.CreateMap<Core.Income, IncomeDto>()
                .ForMember(i => i.Duration, i => i.MapFrom(m => m.Duration.ToDto()));
        }
    }
}
