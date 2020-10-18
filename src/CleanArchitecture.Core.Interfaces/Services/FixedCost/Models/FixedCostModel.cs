using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Core.Interfaces.Services.FixedCost.Models
{
    public class FixedCostModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }
        public CostCategory CostCategory { get; set; }
        public string AccountName { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<FixedCostEntity, FixedCostModel>()
                .ForMember(m => m.AccountName, opt => opt.MapFrom(f => f.Account.Name));
        }
    }
}