using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Models.Domain.BillCategory
{
    public class BillCategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<BillCategoryEntity, BillCategoryModel>();
        }
    }
}
