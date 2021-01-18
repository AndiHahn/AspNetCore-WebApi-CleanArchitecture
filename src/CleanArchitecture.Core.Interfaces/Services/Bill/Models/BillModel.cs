using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Interfaces.Services.Bill.Models
{
    public class BillModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public string ShopName { get; set; }
        public Guid CategoryId { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public byte[] Version { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<BillEntity, BillModel>()
                .ForMember(m => m.CategoryId, opt => opt.MapFrom(b => b.BillCategoryId));
        }
    }

    public enum BillSort
    {
        ShopName,
        Price,
        Date,
        Notes
    }

    public enum BillFilter
    {
        ShopName,
        Price,
        Date,
        Notes
    }
}