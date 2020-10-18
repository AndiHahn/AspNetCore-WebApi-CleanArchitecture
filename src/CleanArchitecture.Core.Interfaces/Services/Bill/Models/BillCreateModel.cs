using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Interfaces.Services.Bill.Models
{
    public class BillCreateModel
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string ShopName { get; set; }
        public double Price { get; set; }
#nullable enable
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
#nullable disable

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<BillCreateModel, BillEntity>()
                .ForMember(m => m.BillCategoryId, opt => opt.MapFrom(b => b.CategoryId))
                .ForMember(m => m.Date, opt => opt.MapFrom(b => b.Date ?? DateTime.UtcNow))
                .ForMember(m => m.Notes, opt => opt.MapFrom(b => b.Notes ?? string.Empty));
        }
    }
}
