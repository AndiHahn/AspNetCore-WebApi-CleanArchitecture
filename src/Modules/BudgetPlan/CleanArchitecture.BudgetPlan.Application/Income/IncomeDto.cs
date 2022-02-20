using AutoMapper;

namespace CleanArchitecture.BudgetPlan.Application.Income
{
    public class IncomeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public DurationDto Duration { get; set; }

        internal static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<Core.Income, IncomeDto>();
        }
    }
}
