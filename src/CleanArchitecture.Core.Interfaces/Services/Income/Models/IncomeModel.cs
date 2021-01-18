using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Core.Interfaces.Services.Income.Models
{
    public class IncomeModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<IncomeEntity, IncomeModel>();
        }
    }
}