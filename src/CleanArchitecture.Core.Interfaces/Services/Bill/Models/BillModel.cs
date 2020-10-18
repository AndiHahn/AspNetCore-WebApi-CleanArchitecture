using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Interfaces.Services.Bill.Models
{
    public class BillModel
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string User { get; set; }
        public string ShopName { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public byte[] Version { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<BillEntity, BillModel>()
                .ForMember(m => m.User, opt => opt.MapFrom(b => $"{b.User.FirstName} {b.User.LastName}"))
                .ForMember(m => m.Account, opt => opt.MapFrom(b => b.Account.Name))
                .ForMember(m => m.Category, opt => opt.MapFrom(b => b.BillCategory.Name));
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