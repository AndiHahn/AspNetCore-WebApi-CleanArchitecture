using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Core.Models.Domain.FixedCost
{
    public class FixedCostCreateModel
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }
        public CostCategory CostCategory { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<FixedCostCreateModel, FixedCostEntity>();
        }

        public override string ToString()
        {
            return $"AccountId: {AccountId}, Name: {Name}, Value: {Value}, Duration: {Duration}, Category: {CostCategory}";
        }
    }
}