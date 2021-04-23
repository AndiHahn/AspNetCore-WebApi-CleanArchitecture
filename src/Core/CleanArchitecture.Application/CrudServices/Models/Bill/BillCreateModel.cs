using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.CrudServices.Models.Bill
{
    public class BillCreateModel
    {
        public Guid BankAccountId { get; set; }
        public string ShopName { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
#nullable enable
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
#nullable disable

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<BillCreateModel, BillEntity>()
                .ForMember(m => m.Date, opt => opt.MapFrom(b => b.Date ?? DateTime.UtcNow))
                .ForMember(m => m.Notes, opt => opt.MapFrom(b => b.Notes ?? string.Empty));
        }
    }
}
